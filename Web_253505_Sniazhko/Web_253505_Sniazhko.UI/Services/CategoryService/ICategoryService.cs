using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.Domain.Models;

namespace Web_253505_Sniazhko.UI.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
