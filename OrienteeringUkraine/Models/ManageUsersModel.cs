using System.Collections.Generic;

namespace OrienteeringUkraine.Models
{
    public class ManageUsersModel
    {
        public int AmountOfUsers { get; set; }
        public int AmountOfAdmins { get; set; }
        public int AmountOfModerators { get; set; }
        public int AmountOfOrganizers { get; set; }
        public int AmountOfSportsmen { get; set; }
        public IEnumerable<AccountUserModel> Users { get; set; }
    }
}
