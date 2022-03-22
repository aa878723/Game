using System;
using System.Collections.Generic;

namespace Game.Models
{
    public partial class PersonalTalking
    {
        public int Id { get; set; }
        public string Account { get; set; } = null!;
        public string Picture { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime SendTime { get; set; }
        public string Friend { get; set; } = null!;
    }
}
