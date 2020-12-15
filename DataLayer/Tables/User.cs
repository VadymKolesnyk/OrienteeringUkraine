using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Tables
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public int RoleId { get; set; }
        public int RegionId { get; set; }
        public int? ClubId { get; set; }
    }
}
