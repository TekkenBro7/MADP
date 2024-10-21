using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_253505_Sniazhko.Domain.Entities
{
    public class CartItem
    {
        public int Amount { get; set; }
        public Dish Item { get; set; }
    }
}
