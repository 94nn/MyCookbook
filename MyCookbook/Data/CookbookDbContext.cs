using Microsoft.EntityFrameworkCore;
using MyCookbook.API.Models;

namespace MyCookbook.API.Data
{
    public class CookbookDbContext(DbContextOptions<CookbookDbContext> options) : DbContext(options)
    {
        public DbSet<Dish> Dishes => Set<Dish>();
        public DbSet<DishStep> DishSteps => Set<DishStep>();
    }
}
