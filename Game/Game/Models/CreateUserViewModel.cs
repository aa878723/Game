using System.ComponentModel.DataAnnotations;

namespace Game.Models
{
    public class ValidateBirthday : ValidationAttribute
    {
        protected override ValidationResult IsValid(object dateObj, ValidationContext validationContext)
        {
            if (dateObj == null)
                return new ValidationResult("Invalid date range");
            var birthDay = DateTime.MinValue;
            if (DateTime.TryParse(dateObj.ToString(), out birthDay))
            {
                if (DateTime.Now.Year - birthDay.Year < 18)
                    return new ValidationResult("必須成年才能加入 LoveGame");
                if (DateTime.Now.Year - birthDay.Year > 60)
                    return new ValidationResult("60 歲以上有點太老捏~");
                return ValidationResult.Success;
            }

            return new ValidationResult($"Failed To Parse {dateObj}");
        }
    }

    public class CreateUserViewModel
    {
        public string Gender { get; set; } = null!;
        [ValidateBirthday]
        public DateTime Birthday { get; set; }
        [StringLength(12, MinimumLength = 6)]
        public string Name { get; set; } = null!;
        public string SexualOrientation { get; set; } = null!;
        public string? Career { get; set; }
        public string Hobby { get; set; } = null!;
        public string City { get; set; } = null!;
        [StringLength(12, MinimumLength = 6)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "只能使用英文字母與數字")]
        public string Account { get; set; } = null!;
        [StringLength(12, MinimumLength = 6)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "只能使用英文字母與數字")]
        public string Password { get; set; } = null!;
        public IFormFile ProfileImage { get; set; } = null!;

        public LoveGame ConvertToLoveGameEntity(string role)
        {
            return new LoveGame
            {
                Gender = Gender,
                Birthday = Birthday,
                Name = Name,
                SexualOrientation = SexualOrientation,
                Career = Career,
                Hobby = Hobby,
                City = City,
                Account = Account,
                Password = Password,
                Role = role,
                Age = DateTime.Now.Year - Birthday.Year,
                LastLogin = DateTime.Now
            };
        }
    }
}
