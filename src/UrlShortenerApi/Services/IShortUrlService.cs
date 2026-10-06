using URLShortener.Models;

namespace UrlShortener.Services{

    public interface IShortUrlService
    {
        public Task<ShortUrl> CreateAsync(string originalUrl);
    }
}