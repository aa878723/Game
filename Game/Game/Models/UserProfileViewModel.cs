using System.ComponentModel;

namespace Game.Models
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }
        [DisplayName("性別")]
        public string Gender { get; set; } = null!;
        [DisplayName("年齡")]
        public int Age { get; set; }
        [DisplayName("暱稱")]
        public string Name { get; set; } = null!;
        [DisplayName("性向")]
        public string SexualOrientation { get; set; } = null!;
        [DisplayName("工作")]
        public string? Career { get; set; }
        [DisplayName("興趣")]
        public string Hobby { get; set; } = null!;
        [DisplayName("出沒地")]
        public string City { get; set; } = null!;
        [DisplayName("帳號")]
        public string Account { get; set; } = null!;
        [DisplayName("角色")]
        public string Role { get; set; } = null!;
        [DisplayName("金幣")]
        public decimal Money { get; set; }

        public UserProfileViewModel (LoveGame dbModel, decimal money)
        {
            Id = dbModel.Id;
            Gender = dbModel.Gender;
            Age = dbModel.Age;
            Name = dbModel.Name;
            SexualOrientation = dbModel.SexualOrientation;
            City = dbModel.City;
            Account = dbModel.Account;
            Role = dbModel.Role;
            Career = dbModel.Career;
            Hobby = dbModel.Hobby;
            Money = money;
        }
    }
}
