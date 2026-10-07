using Microsoft.EntityFrameworkCore;
using URLShortener.Data;
using URLShortener.Dtos;
using URLShortener.Models;

namespace UrlShortener.Services
{

    public class ShortUrlService(URLShortenerDbContext context) : IShortUrlService
    {
        private const int MAX_ATTEMPTS = 10;
        
        public async Task<ShortUrl> CreateAsync(RequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.OriginalUrl))
            {
                throw new ArgumentException("Original Url is required.");
            }

            if(!Uri.TryCreate(request.OriginalUrl,UriKind.Absolute,out var uri)
            || (uri.Scheme!= Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException("Original URL must be a valid HTTP or HTTPS URL.");
            }
            if(request.ExpiresAt.HasValue&& request.ExpiresAt <= DateTime.UtcNow)
            {
                throw new ArgumentException("Expiration date must be in the future.");
            }
            var shortCode = await CreateShortCode();


            var shortUrl = new ShortUrl
            {
                ShortCode = shortCode,
                OriginalUrl  = request.OriginalUrl,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = request.ExpiresAt
            };

            context.ShortUrls.Add(shortUrl);
            await context.SaveChangesAsync();
            return shortUrl;

        }

        private async Task<string> CreateShortCode()
        {
            int attempts = 0;
            while (attempts < MAX_ATTEMPTS)
            {
                var shortCode = GenerateShortCode();
                if (await IsShortCodeAvailable(shortCode))
                {
                    return shortCode;
                }
                attempts++;
            }
         
            throw new Exception("Was unable to create a shortCode");

        }
        private string GenerateShortCode()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(
                Enumerable.Range(0,6).Select(_ => chars[Random.Shared.Next(chars.Length)]).ToArray());
        }

        private async Task<bool> IsShortCodeAvailable(string shortCode)
        {
            return !await context.ShortUrls.AnyAsync(s=>s.ShortCode == shortCode);
        }
        public async Task<ShortUrl?> RedirectAsync(string shortCode)
        {
            var shortUrl = await context.ShortUrls
            .FirstOrDefaultAsync(s=>s.ShortCode==shortCode && (s.ExpiresAt > DateTime.UtcNow || s.ExpiresAt==null));
            if (shortUrl is null)
            {
                return null;
            }

            shortUrl.ClickCount++;
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

        public async Task DeleteAsync(string shortCode)
        {
            var shortUrl = await context.ShortUrls.FirstOrDefaultAsync(s=>s.ShortCode == shortCode);
            if (shortUrl is null)
            {
                return;
            }
            context.ShortUrls.Remove(shortUrl);
            await context.SaveChangesAsync();
        }

    }
}