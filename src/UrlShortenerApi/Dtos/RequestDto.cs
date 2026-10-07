namespace UrlShortener.Dtos{
    
    public class RequestDto
    {
        public required string OriginalUrl {get;set;}
        public DateTime? ExpiresAt {get;set;}
    }


}