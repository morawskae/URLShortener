using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using URLShortener.Data;

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

        public URLShortenerDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<URLShortenerDbContext>().UseSqlite(connection).Options;
            var context = new URLShortenerDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}