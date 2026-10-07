using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using URLShortener.Data;

namespace UrlShortener.Tests
{
    public class TestWebAppFactory: WebApplicationFactory<Program>
    {
        private readonly SqliteConnection connection;

        public TestWebAppFactory()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<URLShortenerDbContext>>();
                services.AddDbContext<URLShortenerDbContext>(options =>
                {
                    options.UseSqlite(connection);
                });
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<URLShortenerDbContext>();

                context.Database.EnsureCreated();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                connection.Dispose();
            }
        }
    }
}