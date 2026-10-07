namespace UrlShortener.Dtos
{
    public class ResponseDto
    {
        public Guid Id {get;set;}
        public required string ShortCode {get;set;}
        public required string OriginalUrl {get;set;}
        public DateTime? ExpiresAt {get;set;}
    }
}