using Microsoft.AspNetCore.Mvc;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.UI.Extensions;
using Web_253505_Sniazhko.UI.Services.CategoryService;
using Web_253505_Sniazhko.UI.Services.ProductService;

namespace Web_253505_Sniazhko.UI.Controllers
{
    [Route("menu")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        [HttpGet("{category?}")]
        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            var productResponse = await _productService.GetProductListAsync(category, pageNo);
            if (!productResponse.Successful)
                return NotFound(productResponse.ErrorMessage);
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Successful)
                return NotFound(categoriesResponse.ErrorMessage);
            var categories = categoriesResponse.Data;
            ViewData["currentCategory"] = categories.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";
            ViewData["categories"] = categories;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DishListPartial", productResponse.Data);
            }
            return View(productResponse.Data);
        }
    }
}