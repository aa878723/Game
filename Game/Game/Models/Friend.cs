using System;
using System.Collections.Generic;

namespace Game.Models
{
    public partial class Friend
    {
        public int Id { get; set; }
        public string Account { get; set; } = null!;
        public string Friend1 { get; set; } = null!;
    }
}
