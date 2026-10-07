using URLShortener.Dtos;
using UrlShortener.Services;

namespace UrlShortener.Tests.Service
{
    public class CreateAsyncTests
    {
        [Fact]
        public async Task CreateAsync_WithValidUrl_CreatesShortUrl()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();
            var service = new ShortUrlService(context);

            var request = new RequestDto
            {
                OriginalUrl = "https://example.com"
            };
            var result = await service.CreateAsync(request);
            Assert.NotNull(result);
            Assert.Equal("https://example.com",result.OriginalUrl);
            Assert.Equal(6,result.ShortCode.Length);
            Assert.Null(result.ExpiresAt);
            Assert.Equal(0, result.ClickCount);

        }

        [Fact]
        public async Task CreateAsync_WithWhiteSpace_ThrowsArgumentException()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();
            var service = new ShortUrlService(context);

            var request = new RequestDto
            {
                OriginalUrl = " "
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(()=>service.CreateAsync(request));
            Assert.Equal("Original Url is required.",exception.Message);
         
        }

        [Fact]
        public async Task CreateAsync_WithEmptyUrl_ThrowsArgumentException()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();
            var service = new ShortUrlService(context);

            var request = new RequestDto
            {
                OriginalUrl = ""
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(()=>service.CreateAsync(request));
            Assert.Equal("Original Url is required.",exception.Message);
         
        }

        [Fact]
        public async Task CreateAsync_WithInvalidUrl_ThrowsArgumentException()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();
            var service = new ShortUrlService(context);

            var request = new RequestDto
            {
                OriginalUrl = "invalid_url_string"
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(()=>service.CreateAsync(request));
            Assert.Equal("Original URL must be a valid HTTP or HTTPS URL.",exception.Message);
         
        }

        
        [Fact]
        public async Task CreateAsync_WithValidExpiryDate_CreatesShortUrl()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();
            var service = new ShortUrlService(context);

            var request = new RequestDto
            {
                OriginalUrl = "https://example.com",
                ExpiresAt = new DateTime(2027,10,7,0,0,0)
            };
            var result = await service.CreateAsync(request);
            Assert.NotNull(result);
            Assert.Equal(new DateTime(2027,10,7,0,0,0), result.ExpiresAt);

            
        }

        [Fact]
        public async Task CreateAsync_WithInvalidExpiryDate_ThrowsArgumentException()
        {
            using var factory = new TestDbContextFactory();
            using var context = factory.CreateContext();
            var service = new ShortUrlService(context);

            var request = new RequestDto
            {
                OriginalUrl = "https://example.com",
                ExpiresAt = new DateTime(2020,10,7,0,0,0)
            };
            var exception = await Assert.ThrowsAsync<ArgumentException>(()=>service.CreateAsync(request));
            Assert.Equal("Expiration date must be in the future.", exception.Message);
            
        }

    }
}