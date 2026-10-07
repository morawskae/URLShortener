using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;

namespace UrlShortener.Tests
{
    
    public class TestDbContextFactory: IDisposable
    {
        private readonly SqliteConnection connection;
        public TestDbContextFactory()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
        }

        public UrlShortenerDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<UrlShortenerDbContext>().UseSqlite(connection).Options;
            var context = new UrlShortenerDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}