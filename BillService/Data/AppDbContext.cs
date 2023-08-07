using BillService.Models;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

namespace BillService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Bill> Bills { get; set; }
    }
}
