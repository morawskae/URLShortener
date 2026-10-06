using UrlShortener.Services;
using URLShortener.Dtos;

namespace UrlShortener.Endpoints
{
    public static class ShortUrlEndpoints
    {
        public static void MapShortUrlEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/urls",(
                async (RequestDto request, IShortUrlService service) =>
                {
                    var shortUrl = await service.CreateAsync(request.OriginalUrl);
                    return Results.Created($"/{shortUrl.ShortCode}",shortUrl);
                }
            ));
        }
    }
}