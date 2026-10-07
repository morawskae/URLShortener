using UrlShortener.Services;
using URLShortener.Models;

namespace UrlShortener.Tests
{
    public class RedirectAsyncTests
    {
        [Fact]
        public async Task RedirectAsync_WithValidShortCode_ReturnsShortUrl()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();

            var shortUrl = new ShortUrl
            {
                ShortCode="abc123",
                OriginalUrl="https://example.com"
            };
            context.ShortUrls.Add(shortUrl);
            await context.SaveChangesAsync();

            var service = new ShortUrlService(context);

            var result = await service.RedirectAsync("abc123");

            Assert.NotNull(result);
            Assert.Equal("https://example.com", result.OriginalUrl);
            Assert.Equal("abc123", result.ShortCode);
            
        }

        [Fact]
        public async Task RedirectAsync_WithValidShortCode_IncreasesClickCount()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();

            var shortUrl = new ShortUrl
            {
                ShortCode="abc123",
                OriginalUrl="https://example.com"
            };
            context.ShortUrls.Add(shortUrl);
            await context.SaveChangesAsync();

            var service = new ShortUrlService(context);

            var result = await service.RedirectAsync("abc123");

            Assert.NotNull(result);
            Assert.Equal(1, result.ClickCount);
            
        }


        
        [Fact]
        public async Task RedirectAsync_WithExpiredUrl_ReturnsNull()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();

            var shortUrl = new ShortUrl
            {
                ShortCode="abc123",
                OriginalUrl="https://example.com",
                ExpiresAt = new DateTime(2020,10,7,0,0,0)
            };
            context.ShortUrls.Add(shortUrl);
            await context.SaveChangesAsync();

            var service = new ShortUrlService(context);

            var result = await service.RedirectAsync("abc123");

            Assert.Null(result);            
        }


        [Fact]
        public async Task RedirectAsync_WithUnknownShortCode_ReturnsNull()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();

            var service = new ShortUrlService(context);

            var result = await service.RedirectAsync("doesNotExist");

            Assert.Null(result);            
        }
    }
}