using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.Repositories;

public class SqlServerError
{
    public static bool IsSqlException(DbUpdateException ex, int number)
    {
        var sqlEx = ex.InnerException as SqlException
                    ?? ex.InnerException?.InnerException as SqlException;

        return sqlEx?.Number == number;
    }

    public static bool IsUniqueConstraintViolation(DbUpdateException ex)
        => IsSqlException(ex, 2601) || IsSqlException(ex, 2627);

    public static bool IsForeignKeyViolation(DbUpdateException ex)
        => IsSqlException(ex, 547);

    public static bool IsPrimaryKeyViolation(DbUpdateException ex)
        => IsSqlException(ex, 2627);
}