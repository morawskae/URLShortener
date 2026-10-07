using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using URLShortener.Data;
using URLShortener.Dtos;
using URLShortener.Models;

namespace UrlShortener.Services
{

    public class ShortUrlService(URLShortenerDbContext context) : IShortUrlService
    {        public async Task<ShortUrl> CreateAsync(string originalUrl)
        {
            var shortCode = generateShortCode();
            var shortUrl = new ShortUrl
            {
                ShortCode = shortCode,
                OriginalUrl  = originalUrl,
                CreatedAt = DateTime.UtcNow
            };

            context.ShortUrls.Add(shortUrl);
            await context.SaveChangesAsync();
            return shortUrl;

        }

        private string generateShortCode()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(
                Enumerable.Range(0,6).Select(_ => chars[Random.Shared.Next(chars.Length)]).ToArray());
        }

        public async Task<ShortUrl?> RedirectAsync(string shortCode)
        {
            var shortUrl = await context.ShortUrls.FirstOrDefaultAsync(s=>s.ShortCode==shortCode);
            if (shortUrl is null)
            {
                return null;
            }

            if(shortUrl.ExpiresAt.HasValue  && shortUrl.ExpiresAt <= DateTime.UtcNow)
            {
                return null;
            }
            shortUrl.ClickCount ++;
            await context.SaveChangesAsync();
            return shortUrl;
        }

        public async Task<ShortUrlDetailsDto?> DetailShortUrlAsync(string shortCode)
        {
            var shortUrl = await context.ShortUrls.FirstOrDefaultAsync(s=>s.ShortCode==shortCode);
            if(shortUrl is null)
            {
                return null;
            }
            return new ShortUrlDetailsDto
            {
                Id = shortUrl.Id,
                ShortCode = shortUrl.ShortCode,
                OriginalUrl = shortUrl.OriginalUrl,
                CreatedAt = shortUrl.CreatedAt,
                ExpiresAt = shortUrl.ExpiresAt,
                ClickCount = shortUrl.ClickCount,
            
            };
        }
    }
}