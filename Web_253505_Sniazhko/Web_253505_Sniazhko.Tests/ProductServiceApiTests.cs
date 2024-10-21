using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Web_253505_Sniazhko.API.Data;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.API.Services.ProductService;
using Web_253505_Sniazhko.Domain.Models;

namespace Web_253505_Sniazhko.Tests
{
    public class ProductServiceApiTests
    {
        private AppDbContext CreateContext()
        {
            // Создаем подключение к SQLite in-memory
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            SeedData(context);
            return context;
        }
        private void SeedData(AppDbContext context)
        {
            var categories = new List<Category>
            {
                    new Category { Id = 1, Name = "Стартеры", NormalizedName = "starters" },
                    new Category { Id = 2, Name = "Салаты", NormalizedName = "salads" },
                    new Category { Id = 3,Name = "Супы", NormalizedName = "soups" },
                    new Category { Id = 4,Name = "Основные блюда", NormalizedName = "main-dishes" },
                    new Category { Id = 5,Name = "Напитки", NormalizedName = "drinks" },
                    new Category { Id = 6,Name = "Десерты", NormalizedName = "desserts" }
            };
            context.Categories.AddRange(categories);
            var dishes = new List<Dish>
            {
                    new Dish { Name = "Суп-харчо", Description = "Очень острый, невкусный", Calories = 200, Image = $"", Category = null, CategoryId =  3},
                    new Dish { Name = "Борщ", Description = "Много сала, без сметаны", Calories = 330, Image = $"", Category = null, CategoryId =  3},
                    new Dish { Name = "Креветки в чесночном соусе", Description = "Сочные креветки с ароматом чеснока", Calories = 250, Image = $"", Category = null, CategoryId = 1 },
                    new Dish { Name = "Брускетта с томатами", Description = "Хрустящий хлеб с свежими помидорами", Calories = 150, Image = $"", Category = null, CategoryId = 1 },
                    new Dish { Name = "Цезарь", Description = "Классический салат с курицей и соусом Цезарь", Calories = 350, Image = $"", Category = null, CategoryId = 2 },
                    new Dish { Name = "Греческий салат", Description = "Свежие овощи с сыром фета и оливками", Calories = 220, Image = $"", Category = null, CategoryId = 2 },
                    new Dish { Name = "Стейк из говядины", Description = "Сочный стейк средней прожарки", Calories = 700, Image = $"", Category = null, CategoryId = 4 },
                    new Dish { Name = "Паста Карбонара", Description = "Кремовая паста с беконом и пармезаном", Calories = 550, Image = $"", Category = null, CategoryId = 4 },
                    new Dish { Name = "Куриные крылышки BBQ", Description = "Пряные куриные крылышки в соусе BBQ", Calories = 280, Image = $"", Category = null, CategoryId = 1 },
                    new Dish { Name = "Фалафель с хумусом", Description = "Классический фалафель с хумусом и питой", Calories = 300, Image = $"", Category = null, CategoryId = 1 }
            };
            context.Dishes.AddRange(dishes);
            context.SaveChanges();
        }
        [Fact]
        public async Task GetProductListAsync_ReturnsFirstPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new ProductService(context);
            var result = await service.GetProductListAsync(null);
            Assert.IsType<ResponseData<ListModel<Dish>>>(result);
            Assert.True(result.Successful);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(4, result.Data.TotalPages);
            Assert.Equal(context.Dishes.First(), result.Data.Items[0]);
        }
        [Fact]
        public async Task GetProductListAsync_ReturnsCorrectPage()
        {
            using var context = CreateContext();
            var service = new ProductService(context);
            var result = await service.GetProductListAsync(null, pageNo: 4, pageSize: 3);
            Assert.True(result.Successful);
            Assert.Equal(4, result.Data.CurrentPage);
            Assert.Single(result.Data.Items); // На 4 странице будет один объект
        }
        [Fact]
        public async Task GetProductListAsync_FiltersByCategory()
        {
            using var context = CreateContext();
            var service = new ProductService(context);
            var result = await service.GetProductListAsync("salads");
            Assert.True(result.Successful);
            Assert.Equal(2, result.Data.Items.Count);
            Assert.All(result.Data.Items, item => Assert.Equal("salads", item.Category.NormalizedName));
        }
        [Fact]
        public async Task GetProductListAsync_PageSizeExceedsMax_ReturnsMaxPageSize()
        {
            using var context = CreateContext();
            var service = new ProductService(context);
            var result = await service.GetProductListAsync(null, pageSize: 100);
            Assert.True(result.Successful);
            Assert.Equal(10, result.Data.Items.Count);
        }
        [Fact]
        public async Task GetProductListAsync_PageNumberExceedsTotalPages_ReturnsError()
        {
            using var context = CreateContext();
            var service = new ProductService(context);
            var result = await service.GetProductListAsync(null, pageNo: 10);
            Assert.False(result.Successful);
            Assert.Equal("Нет такой страницы", result.ErrorMessage);
        }
    }
}
