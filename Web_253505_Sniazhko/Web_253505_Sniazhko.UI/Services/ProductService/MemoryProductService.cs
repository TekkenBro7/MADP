using Microsoft.AspNetCore.Mvc;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.Domain.Models;
using Web_253505_Sniazhko.UI.Services.CategoryService;

namespace Web_253505_Sniazhko.UI.Services.ProductService
{
    public class MemoryProductService : IProductService
    {
        private readonly IConfiguration _config;
        private List<Dish> _dishes;
        private List<Category> _categories;
        public MemoryProductService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync()
            .Result
            .Data;
            SetupData();
            _config = config;
        }

        public Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            if(pageNo < 1)
                return Task.FromResult(ResponseData<ListModel<Dish>>.Error("No such page"));
            var data = _dishes
            .Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName))
            .ToList();
            int itemsPerPage = _config.GetValue<int>("ItemsPerPage");
            int totalItems = data.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);
            var pagedItems = data
            .Skip((pageNo - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToList();
            var result = new ListModel<Dish>
            {
                Items = pagedItems,
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            return Task.FromResult(ResponseData<ListModel<Dish>>.Success(result));
        }

        public Task UpdateProductAsync(int id, Dish product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        private void SetupData()
        {
            _dishes = new List<Dish>
            {
                new Dish { Id = 1, Name = "Суп-харчо", Description = "Очень острый, невкусный", Calories = 200, Image = "Images/Суп-харчо.jpg", Category = _categories.Find(c => c.NormalizedName.Equals("soups")), CategoryId = _categories.Find(c => c.NormalizedName.Equals("soups"))?.Id ?? 0  },
                new Dish { Id = 2, Name = "Борщ", Description = "Много сала, без сметаны", Calories = 330, Image = "Images/Борщ.jpg", Category = _categories.Find(c => c.NormalizedName.Equals("soups")), CategoryId = _categories.Find(c => c.NormalizedName.Equals("soups"))?.Id ?? 0 },
                new Dish { Id = 3, Name = "Креветки в чесночном соусе", Description = "Сочные креветки с ароматом чеснока", Calories = 250, Image = "Images/Креветки.jpg", Category = _categories.Find(c => c.NormalizedName.Equals("starters")), CategoryId = _categories.Find(c => c.NormalizedName.Equals("starters"))?.Id ?? 0 },
                new Dish { Id = 4, Name = "Брускетта с томатами", Description = "Хрустящий хлеб с свежими помидорами", Calories = 150, Image = "Images/Брускетта.jpg", Category = _categories.Find(c => c.NormalizedName.Equals("starters")), CategoryId = _categories.Find(c => c.NormalizedName.Equals("starters"))?.Id ?? 0 },             
                new Dish { Id = 5, Name = "Цезарь", Description = "Классический салат с курицей и соусом Цезарь", Calories = 350, Image = "Images/Цезарь.jpg", Category = _categories.Find(c => c.NormalizedName.Equals("salads")), CategoryId = _categories.Find(c => c.NormalizedName.Equals("salads"))?.Id ?? 0 },
                new Dish { Id = 6, Name = "Греческий салат", Description = "Свежие овощи с сыром фета и оливками", Calories = 220, Image = "Images/Греческий.jpg", Category = _categories.Find(c => c.NormalizedName.Equals("salads")), CategoryId = _categories.Find(c => c.NormalizedName.Equals("salads"))?.Id ?? 0 },
                new Dish { Id = 7, Name = "Стейк из говядины", Description = "Сочный стейк средней прожарки", Calories = 700, Image = "Images/СтейкГавядина.jpg", Category = _categories.Find(c => c.NormalizedName.Equals("main-dishes")), CategoryId = _categories.Find(c => c.NormalizedName.Equals("main-dishes"))?.Id ?? 0 },
                new Dish { Id = 8, Name = "Паста Карбонара", Description = "Кремовая паста с беконом и пармезаном", Calories = 550, Image = "Images/ПастаКарбонара.jpg", Category = _categories.Find(c => c.NormalizedName.Equals("main-dishes")), CategoryId = _categories.Find(c => c.NormalizedName.Equals("main-dishes"))?.Id ?? 0 },
                new Dish { Id = 9, Name = "Куриные крылышки BBQ", Description = "Пряные куриные крылышки в соусе BBQ", Calories = 280, Image = "Images/КрылышкиBBQ.jpg", Category = _categories.Find(c => c.NormalizedName.Equals("starters")), CategoryId = _categories.Find(c => c.NormalizedName.Equals("starters"))?.Id ?? 0 },
                new Dish { Id = 10, Name = "Фалафель с хумусом", Description = "Классический фалафель с хумусом и питой", Calories = 300, Image = "Images/Фалафель.jpg", Category = _categories.Find(c => c.NormalizedName.Equals("starters")), CategoryId = _categories.Find(c => c.NormalizedName.Equals("starters"))?.Id ?? 0 }
            };
        }
    }
}
