using Microsoft.EntityFrameworkCore;
using UrlShortener.Services;
using URLShortener.Dtos;

namespace UrlShortener.Tests
{
    public class DeleteAsyncTests
    {
        [Fact]
        public async Task DeleteAsync_WithValidShortCode_DeletesShortUrl()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();
            var service = new ShortUrlService(context);

            var request = new RequestDto
            {
                OriginalUrl = "https://example.com"
            };

            var shortUrl = await service.CreateAsync(request);
            await service.DeleteAsync(shortUrl.ShortCode);
            
            var result = await context.ShortUrls.FirstOrDefaultAsync(s=>s.ShortCode==shortUrl.ShortCode);
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidShortCode_ReturnsNull()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();
            var service = new ShortUrlService(context);

            var exception = await Record.ExceptionAsync(()=> service.DeleteAsync("doesNotExist"));
            Assert.Null(exception);
        }
    }
}