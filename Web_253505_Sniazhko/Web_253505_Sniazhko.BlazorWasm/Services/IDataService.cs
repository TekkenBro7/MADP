using Web_253505_Sniazhko.Domain.Entities;

namespace Web_253505_Sniazhko.BlazorWasm.Services
{
    public interface IDataService
    {
        event Action DataLoaded;
        List<Category> Categories { get; set; }
        List<Dish> Dishes { get; set; }
        bool Success { get; set; }
        string ErrorMessage { get; set; }
        int TotalPages { get; set; }
        int CurrentPage { get; set; }
        Category SelectedCategory { get; set; }
        public Task GetProductListAsync(int pageNo = 1);
        public Task GetCategoryListAsync();
    }
}
