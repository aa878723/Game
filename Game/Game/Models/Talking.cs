using System;
using System.Collections.Generic;

namespace Game.Models
{
    public partial class Talking
    {
        public int Id { get; set; }
        public string Account { get; set; } = null!;
        public DateTime MessageTime { get; set; }
        public string Message { get; set; } = null!;
        public string Room { get; set; } = null!;
    }
}
