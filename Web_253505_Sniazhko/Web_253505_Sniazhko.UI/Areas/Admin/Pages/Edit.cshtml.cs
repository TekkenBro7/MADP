using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.UI.Services.CategoryService;
using Web_253505_Sniazhko.UI.Services.ProductService;

namespace Web_253505_Sniazhko.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public IFormFile? Image { get; set; }
        public SelectList Categories { get; set; }

        [BindProperty]
        public Dish Dish { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var response = await _productService.GetProductByIdAsync(id.Value);
            //if (!response.Successful || response.Data == null)
            //{
            //    return NotFound(response.ErrorMessage);
            //}

            //Dish = response.Data;
            //return Page();

            if (id == null)
            {
                return NotFound();
            }

            // Загрузка блюда по ID
            var response = await _productService.GetProductByIdAsync(id.Value);
            if (!response.Successful || response.Data == null)
            {
                return NotFound(response.ErrorMessage);
            }

            Dish = response.Data;

            // Загрузка списка категорий
            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (!categoryResponse.Successful)
            {
                return NotFound(categoryResponse.ErrorMessage);
            }

            // Привязка категорий к SelectList
            Categories = new SelectList(categoryResponse.Data, "Id", "Name", Dish.CategoryId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var existingDishResponse = await _productService.GetProductByIdAsync(id);
            if (!existingDishResponse.Successful || existingDishResponse.Data == null)
            {
                return NotFound(existingDishResponse.ErrorMessage);
            }
            var existingDish = existingDishResponse.Data;
            Dish.Image = existingDish.Image;
            if (Image != null)
            {
                await _productService.UpdateProductAsync(id, Dish, Image);
            }
            else
            {       
                await _productService.UpdateProductAsync(id, Dish, null);
            }
            return RedirectToPage("./Index");
        }
    }
}
