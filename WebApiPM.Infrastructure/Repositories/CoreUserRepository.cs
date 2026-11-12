using Dapper;
using WebApiPM.Domain.Entities;
using WebApiPM.Domain.Interfaces;
using WebApiPM.Infrastructure.Data;
using WebApiPM.Infrastructure.Security;

namespace WebApiPM.Infrastructure.Repositories;

public class CoreUserRepository : ICoreUserRepository
{
    private readonly IDapperContext _dapper;

    public CoreUserRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<CoreUser?> GetCoreUserAsync(CoreUser coreUser, CancellationToken cancellationToken)
    {
        // La comparacion se hace por hash: la base nunca guarda el token crudo.
        var commandDefinition = new CommandDefinition(
            cancellationToken: cancellationToken,
            commandText: "SELECT User_Id, HashPM, SaltPM FROM Auth_Users WHERE User_Id = @User_Id AND SqlTokenHash = @SqlTokenHash",
            parameters: new { coreUser.User_Id, SqlTokenHash = SqlTokenHasher.Hash(coreUser.SqlToken) });

        using var connection = _dapper.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<CoreUser>(commandDefinition);
    }

    public async Task RegisterCoreUserPasswordAsync(CoreUser coreUser, CancellationToken cancellationToken)
    {
        // El WHERE va solo por User_Id a proposito: la sesion ya la valido el
        // servicio con GetCoreUserAsync, y el CoreUser que devuelve ese SELECT
        // ya no trae el token crudo (en BD solo queda su hash), por lo que
        // revalidar el hash aqui nunca coincidiria.
        var commandDefinition = new CommandDefinition(
            cancellationToken: cancellationToken,
            commandText: "UPDATE Auth_Users SET HashPM = @HashPM, SaltPM = @SaltPM WHERE User_Id = @User_Id",
            parameters: new
            {
                coreUser.User_Id,
                coreUser.HashPM,
                coreUser.SaltPM
            });

        using var connection = _dapper.CreateConnection();
        await connection.ExecuteAsync(commandDefinition);
    }

    public async Task ChangeCorePasswordAsync(Guid userId, string hash, string salt, Guid newSqlToken, IEnumerable<CoreData> replacementRecords, CancellationToken cancellationToken)
    {
        // Cambio de clave maestra: rotacion de hash/sal/token + reemplazo de
        // los datos re-cifrados, todo en UNA transaccion con UNA conexion.
        // Una conexion por transaccion evita la promocion a transaccion
        // distribuida (TransactionScope multi-conexion exige MSDTC, no
        // disponible en el runner de CI de Linux).
        using var connection = _dapper.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(
            new CommandDefinition(
                cancellationToken: cancellationToken,
                transaction: transaction,
                commandText: "UPDATE Auth_Users SET HashPM = @HashPM, SaltPM = @SaltPM, SqlTokenHash = @SqlTokenHash WHERE User_Id = @User_Id",
                parameters: new
                {
                    User_Id = userId,
                    HashPM = hash,
                    SaltPM = salt,
                    SqlTokenHash = SqlTokenHasher.Hash(newSqlToken)
                }));

        await connection.ExecuteAsync(
            new CommandDefinition(
                cancellationToken: cancellationToken,
                transaction: transaction,
                commandText: "DELETE FROM PM_CoreData WHERE User_Id = @User_Id",
                parameters: new { User_Id = userId }));

        foreach (var record in replacementRecords)
        {
            await connection.ExecuteAsync(
                new CommandDefinition(
                    cancellationToken: cancellationToken,
                    transaction: transaction,
                    commandText: "INSERT INTO PM_CoreData (Data_Id, Data01, Data02, Data03, User_Id) VALUES (@Data_Id, @Data01, @Data02, @Data03, @User_Id)",
                    parameters: new
                    {
                        record.Data_Id,
                        record.Data01,
                        record.Data02,
                        record.Data03,
                        User_Id = userId
                    }));
        }

        transaction.Commit();
    }
}
