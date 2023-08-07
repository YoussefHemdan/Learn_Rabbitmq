using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using static Azure.Core.HttpHeader;

namespace OrderService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
