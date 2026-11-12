using System.Security.Cryptography;
using System.Text;

namespace WebApiPM.Infrastructure.Security;

public static class SqlTokenHasher
{
    /// <summary>
    /// Devuelve el SHA-256 en hexadecimal minusculas del token de sesion.
    /// El token crudo nunca se persiste: la base solo guarda este hash.
    /// </summary>
    /// <remarks>
    /// El formato de entrada es el canonico de GUID ("D", con guiones y en
    /// minusculas), que es exactamente lo que produce
    /// <c>CONVERT(VARCHAR(36), SqlToken)</c> en SQL Server. La salida en
    /// minusculas coincide con <c>HASHBYTES(..., 2)</c> para que la
    /// comparacion en SQL no dependa del collation de la base.
    /// Ver SqlServerPM_Migrate_SqlTokenHash.sql.
    /// </remarks>
    public static string Hash(Guid sqlToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(sqlToken.ToString("D")));

        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
