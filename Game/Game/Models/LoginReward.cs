using System;
using System.Collections.Generic;

namespace Game.Models
{
    public partial class LoginReward
    {
        public int Id { get; set; }
        public string Account { get; set; } = null!;
        public DateTime RewardDate { get; set; }
        public int ContinuousLoginDay { get; set; }
        public int TotalLoginDay { get; set; }
        public decimal RewardMoney { get; set; }
    }
}
