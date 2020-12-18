using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Models
{
    public class AccountUserModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }
        public string Role { get; set; }
        [Required(ErrorMessage = "Не указано имя")]
        public int RoleId { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указана фамилия")]
        public string Surname { get; set; }
        public DateTime? Birthday { get; set; }
        [Required(ErrorMessage = "Не указан регион")]
        public int RegionId { get; set; }
        public string Region { get; set; }
        public int? ClubId { get; set; }
        public string Club { get; set; }
    }
}
