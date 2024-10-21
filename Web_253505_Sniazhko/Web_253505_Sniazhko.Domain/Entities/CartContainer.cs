using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_253505_Sniazhko.Domain.Entities
{
    public class CartContainer
    {
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        public virtual void AddToCart(Dish dish)
        {
            if (dish == null)
                throw new ArgumentNullException(nameof(dish));
            int id = dish.Id;
            if (CartItems.ContainsKey(id))
            {
                CartItems[id].Amount++;
            }
            else
            {
                CartItems[id] = new CartItem { Amount = 1, Item = dish };
            }
        }
        public virtual void RemoveItems(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                var cartItem = CartItems[id];

                if (cartItem.Amount > 1)
                {
                    cartItem.Amount--;
                }
                else
                {
                    CartItems.Remove(id);
                }
            }
        }
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        public int Count { get => CartItems.Sum(item => item.Value.Amount); }
        public double TotalCalories { get => CartItems.Sum(item => item.Value.Item.Calories * item.Value.Amount); }
    }
}