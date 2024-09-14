using Web_253505_Sniazhko.UI.ApiInteraction;
using Web_253505_Sniazhko.UI.ApiInteraction.Services;
using Web_253505_Sniazhko.UI.Services.CategoryService;
using Web_253505_Sniazhko.UI.Services.ProductService;

namespace Web_253505_Sniazhko.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder, UriData? uriData)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            builder.Services.AddScoped<IProductService, MemoryProductService>();
            builder.Services
            .AddHttpClient<IProductService, ApiProductService>(opt =>
            opt.BaseAddress = new Uri(uriData.ApiUri));
            builder.Services
                .AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
                opt.BaseAddress = new Uri(uriData.ApiUri));
        }
    }
}
