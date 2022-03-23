using System;
using System.Collections.Generic;

namespace Game.Models
{
    public partial class TbFriend
    {
        public int Id { get; set; }
        public string Account { get; set; } = null!;
        public string Friend { get; set; } = null!;
    }
}
