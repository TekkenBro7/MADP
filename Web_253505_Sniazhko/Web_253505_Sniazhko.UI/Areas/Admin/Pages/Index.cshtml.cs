using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.UI.Services.ProductService;

namespace Web_253505_Sniazhko.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IList<Dish> Dish { get; set; } = new List<Dish>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;

        public async Task OnGetAsync(int? pageNo)
        {
            CurrentPage = pageNo ?? 1;
            var response = await _productService.GetProductListAsync(null, CurrentPage);
            if (response.Successful)
            {
                Dish = response.Data.Items;
                TotalPages = response.Data.TotalPages;
            }
        }
    }
}
