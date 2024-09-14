using Microsoft.EntityFrameworkCore;
using Web_253505_Sniazhko.Domain.Entities;

namespace Web_253505_Sniazhko.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
