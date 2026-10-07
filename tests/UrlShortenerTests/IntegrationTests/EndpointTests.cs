using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using UrlShortener.Dtos;
using UrlShortener.Models;

namespace UrlShortener.Tests.IntegrationTests
{
    
    public class EndpointTests: IClassFixture<TestWebAppFactory>
    {

        private readonly TestWebAppFactory factory;
        public EndpointTests(TestWebAppFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task CreateShortUrl_WithValidRequest_Created()
        {
            var client = factory.CreateClient();
            var request = new RequestDto
            {
                OriginalUrl = "https://example.com"
            };

            var response = await client.PostAsJsonAsync("/api/urls",request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var result = await response.Content.ReadFromJsonAsync<ResponseDto>();

            Assert.NotNull(result);
            Assert.Equal("https://example.com", result.OriginalUrl);
            Assert.Equal(6, result.ShortCode.Length);
        }


        [Fact]
        public async Task CreateShortUrl_WithInvalidRequest_BadRequest()
        {
            var client = factory.CreateClient();
            var request = new RequestDto
            {
                OriginalUrl = "invalid_url_string"
            };

            var response = await client.PostAsJsonAsync("/api/urls",request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateShortUrl_WithInvalidExpiryDate_BadRequest()
        {
            var client = factory.CreateClient();
            var request = new RequestDto
            {
                OriginalUrl = "https://example.com",
                ExpiresAt = new DateTime(2020,10,7,0,0,0)

            };

            var response = await client.PostAsJsonAsync("/api/urls",request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task Redirect_WithValidShortCode_Redirects()
        {
            var client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect=false
            });

            var request = new RequestDto
            {
                OriginalUrl = "https://example.com"
            };

            var createResponse = await client.PostAsJsonAsync("/api/urls",request);
            Assert.Equal(HttpStatusCode.Created,createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<ResponseDto>();

            Assert.NotNull(created);

            var response = await client.GetAsync($"/{created.ShortCode}");
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("https://example.com/", response.Headers.Location?.ToString());
        }

        
        [Fact]
        public async Task Redirect_WithInvalidShortCode_NotFound()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync($"/doesNotExist");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task  Detail_WithValidShortCode_OK()
        {
            var client = factory.CreateClient();

            var request = new RequestDto
            {
                OriginalUrl = "https://example.com"
            };

            var createResponse = await client.PostAsJsonAsync("/api/urls",request);
            Assert.Equal(HttpStatusCode.Created,createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<ResponseDto>();

            Assert.NotNull(created);

            var response = await client.GetAsync($"/api/urls/{created.ShortCode}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<ShortUrlDetailsDto>();

            
            Assert.NotNull(result);
            Assert.Equal(created.Id, result.Id);
            Assert.Equal(created.ShortCode, result.ShortCode);
            Assert.Equal(created.OriginalUrl, result.OriginalUrl);
            Assert.Equal(created.ExpiresAt, result.ExpiresAt);
            Assert.Equal(0, result.ClickCount);
            Assert.NotEqual(default, result.CreatedAt);

        }
        
        [Fact]
        public async Task Detail_WithInvalidShortCode_NotFound()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync($"/api/urls/doesNotExist");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task Delete_WithValidShortCode_NoContent()
        {
             var client = factory.CreateClient();

            var request = new RequestDto
            {
                OriginalUrl = "https://example.com"
            };

            var createResponse = await client.PostAsJsonAsync("/api/urls",request);
            Assert.Equal(HttpStatusCode.Created,createResponse.StatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<ResponseDto>();

            Assert.NotNull(created);

            var response = await client.DeleteAsync($"/api/urls/{created.ShortCode}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }

                [Fact]
        public async Task Delete_WithInvalidShortCode_NoContent()
        {
             var client = factory.CreateClient();

            var response = await client.DeleteAsync($"/api/urls/doesNotExist");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);


        }

        
    }
}