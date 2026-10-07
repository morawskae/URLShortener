using URLShortener.Dtos;
using URLShortener.Models;

namespace UrlShortener.Services{

    public interface IShortUrlService
    {
        public Task<ShortUrl> CreateAsync(RequestDto request);
        public Task<ShortUrl?> RedirectAsync(string shortCode);
        public Task<ShortUrlDetailsDto?> DetailShortUrlAsync(string shortCode);

        public Task DeleteAsync(string shortCode);
    }
}