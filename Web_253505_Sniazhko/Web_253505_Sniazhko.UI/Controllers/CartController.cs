using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.UI.Extensions;
using Web_253505_Sniazhko.UI.Services.ProductService;

namespace Web_253505_Sniazhko.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private CartContainer _cart;
        public CartController(IProductService productService, CartContainer cart)
        {
            _productService = productService;
            _cart = cart;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            CartContainer cart = HttpContext.Session.Get<CartContainer>("cart") ?? new();
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        //[Route("[controller]/Add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _productService.GetProductByIdAsync(id);
            if (data.Successful)
            {
                _cart.AddToCart(data.Data);
            }
            return Redirect(returnUrl);
        }
        [HttpPost]
        [Authorize]
        //[Route("[controller]/Add/{id:int}")]
        public async Task<ActionResult> Remove(int id, string returnUrl)
        {
            _cart.RemoveItems(id);
            return Redirect(returnUrl);
        }
        [HttpPost]
        [Authorize]
        //[Route("[controller]/Add/{id:int}")]
        public async Task<ActionResult> Clear(string returnUrl)
        {
            _cart.ClearAll();
            return Redirect(returnUrl);
        }
    }
}
