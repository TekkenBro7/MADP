using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_253505_Sniazhko.Domain.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category? Category { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public int Calories { get; set; }
        public string? Image { get; set; }
    }
}
