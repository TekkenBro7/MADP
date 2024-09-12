using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.Domain.Models;

namespace Web_253505_Sniazhko.UI.Services.ProductService
{
    public interface IProductService
    {
        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
        public Task<ResponseData<Dish>> GetProductByIdAsync(int id);
        public Task UpdateProductAsync(int id, Dish product, IFormFile? formFile);
        public Task DeleteProductAsync(int id);
        public Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile);
    }
}
