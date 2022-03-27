using System;
using System.Collections.Generic;

namespace Game.Models
{
    public partial class LoginRewardViewModel
    {
        public string Account { get; set; } = null!;
        public DateTime RewardDate { get; set; }
        public int ContinuousLoginDay { get; set; }
        public int TotalLoginDay { get; set; }
        public decimal RewardMoney { get; set; }
        public decimal TotalMoney { get; set; }

        public LoginRewardViewModel(LoginReward dbReward, decimal totalMoney)
        {
            this.Account = dbReward.Account;
            this.RewardMoney = dbReward.RewardMoney;
            this.ContinuousLoginDay = dbReward.ContinuousLoginDay;
            this.TotalLoginDay = dbReward.TotalLoginDay;
            this.RewardDate = dbReward.RewardDate;
            this.TotalMoney = totalMoney;
        }
    }
}
