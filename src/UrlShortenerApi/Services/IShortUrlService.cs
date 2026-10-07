using UrlShortener.Dtos;
using UrlShortener.Models;

namespace UrlShortener.Services{

    public interface IShortUrlService
    {
        public Task<ResponseDto> CreateAsync(RequestDto request);
        public Task<ResponseDto?> RedirectAsync(string shortCode);
        public Task<ShortUrlDetailsDto?> DetailShortUrlAsync(string shortCode);

        public Task DeleteAsync(string shortCode);
    }
}