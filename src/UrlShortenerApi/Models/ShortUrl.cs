namespace URLShortener.Models
{
    public class ShortUrl
    {
        public Guid Id {get;set;}
        public required string ShortCode {get;set;}
        public required string OriginalUrl {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
        public DateTime? ExpiresAt {get;set;}
        public int ClickCount{get;set;}=0;
    }
}