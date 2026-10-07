using Microsoft.EntityFrameworkCore;
using UrlShortener.Dtos;
using UrlShortener.Services;

namespace UrlShortener.Tests.Service
{
    public class ShortUrlServiceTests
    {

        [Fact]
        public async Task DetailShortUrlAsync_WithInvalidShortCode_ReturnsNull()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();
            var service = new ShortUrlService(context);

            var result = await service.DetailShortUrlAsync("doesNotExist");
            Assert.Null(result);
         
        }

        [Fact]
        public async Task DetailShortUrlAsync_WithValidShortCode_ReturnsShortUrlDetailsDto()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();
            var service = new ShortUrlService(context);

            var request = new RequestDto
            {
                OriginalUrl ="https://example.com"
            };

            var created = await service.CreateAsync(request);

            Assert.NotNull(created);

            var shortUrl = await context.ShortUrls.FirstOrDefaultAsync(s=>s.ShortCode == created.ShortCode);
            Assert.NotNull(shortUrl);

            var result = await service.DetailShortUrlAsync(shortUrl.ShortCode);

            Assert.NotNull(result);
            Assert.Equal(shortUrl.Id, result.Id);
            Assert.Equal(shortUrl.ShortCode, result.ShortCode);
            Assert.Equal(shortUrl.OriginalUrl, result.OriginalUrl);
            Assert.Equal(shortUrl.CreatedAt, result.CreatedAt);
            Assert.Equal(shortUrl.ExpiresAt, result.ExpiresAt);
            Assert.Equal(shortUrl.ClickCount, result.ClickCount);
         
        }
    }

}