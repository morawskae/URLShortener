using UrlShortener.Services;
using UrlShortener.Dtos;

namespace UrlShortener.Endpoints
{
    public static class ShortUrlEndpoints
    {
        public static void MapShortUrlEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/urls",(
                async (RequestDto request, IShortUrlService service) =>
                {
                    try{
                    var shortUrl = await service.CreateAsync(request);
                    return Results.Created($"/{shortUrl.ShortCode}",shortUrl);
                    }
                    catch (ArgumentException e)
                    {
                        return Results.BadRequest(new
                        {
                            error = e.Message
                        });
                    }

                }
            ));

            app.MapGet("/{shortCode}",(
                async (string shortCode, IShortUrlService service)=>
                {
                    var shortUrl = await service.RedirectAsync(shortCode);
                    if (shortUrl is null)
                    {
                        return Results.NotFound();
                    }
                    return Results.Redirect(shortUrl.OriginalUrl);
                }
            ));
            app.MapGet("/api/urls/{shortCode}",(
                async (string shortCode, IShortUrlService service) =>
                {
                    var response = await service.DetailShortUrlAsync(shortCode);
                    if(response is null)
                    {
                        return Results.NotFound();
                    }
                    return Results.Ok(response);
                }
            ));
            app.MapDelete("/api/urls/{shortCode}",(
                async (string shortCode, IShortUrlService service) =>
                {
                    await service.DeleteAsync(shortCode);  
                    return Results.NoContent();
                }
            ));
        }
    }
}