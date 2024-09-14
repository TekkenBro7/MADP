using Microsoft.EntityFrameworkCore;
using Web_253505_Sniazhko.Domain.Entities;

namespace Web_253505_Sniazhko.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Стартеры", NormalizedName = "starters" },
                    new Category { Name = "Салаты", NormalizedName = "salads" },
                    new Category { Name = "Супы", NormalizedName = "soups" },
                    new Category { Name = "Основные блюда", NormalizedName = "main-dishes" },
                    new Category { Name = "Напитки", NormalizedName = "drinks" },
                    new Category { Name = "Десерты", NormalizedName = "desserts" }
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
            var baseUrl = app.Configuration.GetValue<string>("ApplicationUrl");
            if (!context.Dishes.Any())
            {
                var categories = await context.Categories.ToListAsync();
                var dishes = new List<Dish>
                {
                    new Dish { Name = "Суп-харчо", Description = "Очень острый, невкусный", Calories = 200, Image = $"{baseUrl}/Images/Суп-харчо.jpg", Category = categories.First(c => c.NormalizedName == "soups"), CategoryId = categories.Find(c => c.NormalizedName.Equals("soups"))?.Id ?? 0 },
                    new Dish { Name = "Борщ", Description = "Много сала, без сметаны", Calories = 330, Image = $"{baseUrl}/Images/Борщ.jpg", Category = categories.First(c => c.NormalizedName == "soups"), CategoryId = categories.Find(c => c.NormalizedName.Equals("soups"))?.Id ?? 0 },
                    new Dish { Name = "Креветки в чесночном соусе", Description = "Сочные креветки с ароматом чеснока", Calories = 250, Image = $"{baseUrl}/Images/Креветки.jpg", Category = categories.First(c => c.NormalizedName == "starters"), CategoryId = categories.Find(c => c.NormalizedName.Equals("starters"))?.Id ?? 0 },
                    new Dish { Name = "Брускетта с томатами", Description = "Хрустящий хлеб с свежими помидорами", Calories = 150, Image = $"{baseUrl}/Images/Брускетта.jpg", Category = categories.First(c => c.NormalizedName == "starters"), CategoryId = categories.Find(c => c.NormalizedName.Equals("starters"))?.Id ?? 0 },
                    new Dish { Name = "Цезарь", Description = "Классический салат с курицей и соусом Цезарь", Calories = 350, Image = $"{baseUrl}/Images/Цезарь.jpg", Category = categories.Find(c => c.NormalizedName.Equals("salads")), CategoryId = categories.Find(c => c.NormalizedName.Equals("salads"))?.Id ?? 0 },
                    new Dish { Name = "Греческий салат", Description = "Свежие овощи с сыром фета и оливками", Calories = 220, Image = $"{baseUrl}/Images/Греческий.jpg", Category = categories.Find(c => c.NormalizedName.Equals("salads")), CategoryId = categories.Find(c => c.NormalizedName.Equals("salads"))?.Id ?? 0 },
                    new Dish { Name = "Стейк из говядины", Description = "Сочный стейк средней прожарки", Calories = 700, Image = $"{baseUrl}/Images/СтейкГавядина.jpg", Category = categories.Find(c => c.NormalizedName.Equals("main-dishes")), CategoryId = categories.Find(c => c.NormalizedName.Equals("main-dishes"))?.Id ?? 0 },
                    new Dish { Name = "Паста Карбонара", Description = "Кремовая паста с беконом и пармезаном", Calories = 550, Image = $"{baseUrl}/Images/ПастаКарбонара.jpg", Category = categories.Find(c => c.NormalizedName.Equals("main-dishes")), CategoryId = categories.Find(c => c.NormalizedName.Equals("main-dishes"))?.Id ?? 0 },
                    new Dish { Name = "Куриные крылышки BBQ", Description = "Пряные куриные крылышки в соусе BBQ", Calories = 280, Image = $"{baseUrl}/Images/КрылышкиBBQ.jpg", Category = categories.Find(c => c.NormalizedName.Equals("starters")), CategoryId = categories.Find(c => c.NormalizedName.Equals("starters"))?.Id ?? 0 },
                    new Dish { Name = "Фалафель с хумусом", Description = "Классический фалафель с хумусом и питой", Calories = 300, Image = $"{baseUrl}/Images/Фалафель.jpg", Category = categories.Find(c => c.NormalizedName.Equals("starters")), CategoryId = categories.Find(c => c.NormalizedName.Equals("starters"))?.Id ?? 0 }
                };
                await context.Dishes.AddRangeAsync(dishes);
                await context.SaveChangesAsync();
            }
        }
    }
}
