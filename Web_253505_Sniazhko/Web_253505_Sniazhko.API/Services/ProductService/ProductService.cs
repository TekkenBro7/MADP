using Microsoft.EntityFrameworkCore;
using Web_253505_Sniazhko.API.Data;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.Domain.Models;

namespace Web_253505_Sniazhko.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseData<Dish>> CreateProductAsync(Dish product)
        {
            await _context.Dishes.AddAsync(product);
            await _context.SaveChangesAsync();

            return ResponseData<Dish>.Success(product);
        }
        public async Task DeleteProductAsync(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                throw new Exception("Блюдо не найдено");
            }
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
        }
        public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            var dish = await _context.Dishes.Include(d => d.Category).FirstOrDefaultAsync(d => d.Id == id);
            if (dish == null)
            {
                return ResponseData<Dish>.Error("Товар не найден");
            }
            return ResponseData<Dish>.Success(dish);
        }
        public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;
            var query = _context.Dishes.AsQueryable();
            var dataList = new ListModel<Dish>();
            query = query.Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName));
            var count = await query.CountAsync();
            if (count == 0)
            {
                return ResponseData<ListModel<Dish>>.Success(dataList);
            }
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return ResponseData<ListModel<Dish>>.Error("Нет такой страницы");
            dataList.Items = await query
                .OrderBy(d => d.Id)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            return ResponseData<ListModel<Dish>>.Success(dataList);
        }
        public async Task UpdateProductAsync(int id, Dish product)
        {
            var existingDish = await _context.Dishes.FindAsync(id);
            if (existingDish == null)
            {
                throw new Exception("Блюдо не найдено");
            }
            existingDish.Name = product.Name;
            existingDish.Description = product.Description;
            existingDish.Calories = product.Calories;
            existingDish.CategoryId = product.CategoryId;
            existingDish.Image = product.Image;
            await _context.SaveChangesAsync();
        }
    }
}
