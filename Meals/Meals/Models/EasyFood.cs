using System;
using System.Collections.Generic;

namespace Meals.Models
{
    public partial class EasyFood
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Price { get; set; }
        public string MealStyle { get; set; } = null!;
    }
}
