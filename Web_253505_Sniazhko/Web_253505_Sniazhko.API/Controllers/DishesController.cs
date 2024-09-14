using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_253505_Sniazhko.API.Data;
using Web_253505_Sniazhko.API.Services.ProductService;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.Domain.Models;

namespace Web_253505_Sniazhko.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IProductService _productService;
        public DishesController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: api/Dishes/category/{category}?pageNo=1&pageSize=3
        [HttpGet("category/{category}")]
        public async Task<ActionResult<ResponseData<List<Dish>>>> GetDishesByCategory(string category, int pageNo = 1, int pageSize = 3)
        {
            return Ok(await _productService.GetProductListAsync(
                category,
                pageNo,
                pageSize));
        }
        // GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<Dish>>>> GetDishes(string? category, int pageNo = 1, int pageSize = 3)
        {
            return Ok(await _productService.GetProductListAsync(
                category,
                pageNo,
                pageSize));
        }
        //GET: api/Dishes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<Dish>>> GetDish(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        // PUT: api/Dishes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            if (id != dish.Id)
            {
                return BadRequest();
            }
            await _productService.UpdateProductAsync(id, dish);
            //    return Ok();
            return NoContent();
        }
        // POST: api/Dishes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResponseData<Dish>>> PostDish(Dish dish)
        {
            var result = await _productService.CreateProductAsync(dish);
            return Ok(result);
        }
        // DELETE: api/Dishes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
        // POST: api/Products/5/SaveImage
        [HttpPost("{id}/SaveImage")]
        public async Task<ActionResult<ResponseData<string>>> SaveImage(int id, IFormFile formFile)
        {
            var result = await _productService.SaveImageAsync(id, formFile);
            return Ok(result);
        }
    }
}
