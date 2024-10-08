﻿using Microsoft.AspNetCore.Mvc;

namespace Web_253505_Sniazhko.UI.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cartInfo = new { TotalItems = 0, TotalPrice = 0 };
            return View(cartInfo);
        }
    }
}
