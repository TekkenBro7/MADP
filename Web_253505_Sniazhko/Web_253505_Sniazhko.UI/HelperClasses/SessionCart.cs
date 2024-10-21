using Web_253505_Sniazhko.Domain.Entities;
using Web_253505_Sniazhko.UI.Extensions;

namespace Web_253505_Sniazhko.UI.HelperClasses
{
    public class SessionCart : CartContainer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionCart(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private CartContainer GetCartFromSession()
        {
            return _httpContextAccessor.HttpContext.Session.Get<CartContainer>("cart") ?? new CartContainer();
        }
        private void SaveCartToSession(CartContainer cart)
        {
            _httpContextAccessor.HttpContext.Session.Set("cart", cart);
        }
        public override void AddToCart(Dish dish)
        {
            var cart = GetCartFromSession();
            cart.AddToCart(dish);
            SaveCartToSession(cart);
        }
        public override void RemoveItems(int id)
        {
            var cart = GetCartFromSession();
            cart.RemoveItems(id);
            SaveCartToSession(cart);
        }
        public override void ClearAll()
        {
            var cart = GetCartFromSession();
            cart.ClearAll();
            SaveCartToSession(cart);
        }
    }
}
