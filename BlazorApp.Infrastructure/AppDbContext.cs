using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

    }
}