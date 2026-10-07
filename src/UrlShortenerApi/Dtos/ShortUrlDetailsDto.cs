namespace UrlShortener.Dtos
{
    public class ShortUrlDetailsDto
    {
        public Guid Id {get;set;}
        public required string ShortCode {get;set;}
        public required string OriginalUrl {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime? ExpiresAt {get;set;}
        public  int ClickCount{get;set;}

    }
}