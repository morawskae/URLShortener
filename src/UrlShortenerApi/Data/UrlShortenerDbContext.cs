using Microsoft.EntityFrameworkCore;
using URLShortener.Models;

namespace URLShortener.Data
{
    public class URLShortenerDbContext: DbContext
    {
        public DbSet<ShortUrl> ShortUrls {get;set;}

        public URLShortenerDbContext(DbContextOptions<URLShortenerDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entity = modelBuilder.Entity<ShortUrl>();
            entity.ToTable("short_urls");
            
            entity.Property(e=>e.Id).HasColumnName("short_url_id");
            entity.HasKey(e=>e.Id);
            entity.Property(e=>e.ShortCode).IsRequired().HasMaxLength(10)
            .HasColumnName("short_code");
            entity.HasIndex(e=>e.ShortCode).IsUnique();
            entity.Property(e=>e.OriginalUrl).IsRequired().HasColumnName("original_url");
            entity.Property(e=>e.CreatedAt).HasColumnName("created_at");
            entity.Property(e=>e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e=>e.ClickCount).HasColumnName("click_count");

        }
    }
}