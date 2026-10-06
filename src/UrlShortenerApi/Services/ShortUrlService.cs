using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using URLShortener.Data;
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
    }
}