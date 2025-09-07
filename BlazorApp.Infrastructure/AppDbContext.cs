using Microsoft.EntityFrameworkCore;
using BlazorApp.Domain.Entities;

namespace BlazorApp.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

    }
}