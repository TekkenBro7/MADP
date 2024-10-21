

using Microsoft.AspNetCore.Mvc;
//using Web_253505_Sniazhko.Domain.Entities;
//using Web_253505_Sniazhko.UI.Extensions;

//namespace Web_253505_Sniazhko.UI.ViewComponents
//{
//    public class CartViewComponent : ViewComponent
//    {
//        public IViewComponentResult Invoke()
//        {
//            var cart = HttpContext.Session.Get<CartContainer>("cart") ?? new CartContainer();

//            // Рассчитываем общее количество товаров и общую стоимость
//            var totalItems = cart.CartItems.Sum(item => item.Value.Amount);
//            var totalPrice = cart.CartItems.Sum(item => item.Value.Amount * item.Value.Item.Calories);

//            // Передаём объект с информацией в представление
//            var cartInfo = new { TotalItems = totalItems, TotalPrice = totalPrice };
            
//            return View(cartInfo);
//        }
//    }
//}
 

using Microsoft.AspNetCore.Mvc;
using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.UI.Extensions;

namespace Web_253505_Sniazhko.UI.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private CartContainer _cart;
        public CartViewComponent(CartContainer cart)
        {
            _cart = cart;
        }
        public IViewComponentResult Invoke()
        {
        //    _cart = HttpContext.Session.Get<CartContainer>("cart") ?? new CartContainer();
            var totalItems = _cart.CartItems.Sum(item => item.Value.Amount);
            var totalPrice = _cart.CartItems.Sum(item => item.Value.Amount * item.Value.Item.Calories);
            var cartInfo = new { TotalItems = totalItems, TotalPrice = totalPrice };
            return View(cartInfo);                   
        }
    }
}
