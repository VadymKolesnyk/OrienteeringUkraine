using System;
using System.ComponentModel.DataAnnotations;

namespace OrienteeringUkraine.Data
{
    public class AccountRegisterData
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указана фамилия")]
        public string Surname { get; set; }
        public DateTime? Bithday { get; set; }
        [Required(ErrorMessage = "Не указан регион")]
        public int RegionId { get; set; }
        
        public int? ClubId { get; set; }

    }
}
