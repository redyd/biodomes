using BioDomes.Infrastructures;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.Tests.TestSupport;

internal static class TestDbContextFactory
{
    public static BiodomeContext Create(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<BiodomeContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;

        var context = new BiodomeContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
