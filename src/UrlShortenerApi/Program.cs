using Microsoft.EntityFrameworkCore;
using UrlShortener.Services;
using URLShortener.Data;
using UrlShortener.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IShortUrlService,ShortUrlService>();

builder.Services.AddDbContext<URLShortenerDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapShortUrlEndpoints();
app.UseHttpsRedirection();
app.Run();

public partial class Program {}

