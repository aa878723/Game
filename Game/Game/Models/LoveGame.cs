using System;
using System.Collections.Generic;

namespace Game.Models
{
    public partial class LoveGame
    {
        public int Id { get; set; }
        public string Gender { get; set; } = null!;
        public int Age { get; set; }
        public DateTime? Birthday { get; set; }
        public string Name { get; set; } = null!;
        public string SexualOrientation { get; set; } = null!;
        public string? Career { get; set; }
        public string Hobby { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime LastLogin { get; set; }
    }
}
