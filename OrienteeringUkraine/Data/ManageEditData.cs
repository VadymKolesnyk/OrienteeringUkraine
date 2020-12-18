using OrienteeringUkraine.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OrienteeringUkraine.Data
{
    public class ManageEditData
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public int RoleId { get; set; }


    }
}
