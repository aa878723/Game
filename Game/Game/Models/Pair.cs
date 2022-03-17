using System;
using System.Collections.Generic;

namespace Game.Models
{
    public partial class Pair
    {
        public int Id { get; set; }
        public string Account { get; set; } = null!;
        public string Lover { get; set; } = null!;
        public bool Want { get; set; }
    }
}
