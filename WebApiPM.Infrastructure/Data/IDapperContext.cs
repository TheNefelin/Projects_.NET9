using System.Data;

namespace WebApiPM.Infrastructure.Data;

public interface IDapperContext
{
    IDbConnection CreateConnection();
}