using System;
using System.Collections.Generic;

namespace Game.Models
{
    public partial class Money
    {
        public int Id { get; set; }
        public string Account { get; set; } = null!;
        public string Card { get; set; } = null!;
        public decimal Qe { get; set; }
    }
}
