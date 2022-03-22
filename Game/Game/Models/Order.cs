using System;
using System.Collections.Generic;

namespace Game.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public string Account { get; set; } = null!;
        public string OrderNumber { get; set; } = null!;
        public DateTime OrderTime { get; set; }
        public decimal Amount { get; set; }
    }
}
