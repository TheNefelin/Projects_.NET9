using System.Data;

namespace Infrastructure.Data;

public interface IDapperContext
{
    IDbConnection CreateConnection();
}
