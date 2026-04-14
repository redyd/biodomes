using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.Repositories;

public class SqlServerError
{
    public static bool IsUniqueConstraintViolation(DbUpdateException ex) =>
        ex.InnerException is SqlException { Number: 2601 or 2627 };

    public static bool IsForeignKeyViolation(DbUpdateException ex) =>
        ex.InnerException is SqlException { Number: 547 };
}