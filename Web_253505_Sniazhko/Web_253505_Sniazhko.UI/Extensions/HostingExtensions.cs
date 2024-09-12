using Web_253505_Sniazhko.UI.Services.CategoryService;
using Web_253505_Sniazhko.UI.Services.ProductService;

namespace Web_253505_Sniazhko.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            builder.Services.AddScoped<IProductService, MemoryProductService>();
        }
    }
}
