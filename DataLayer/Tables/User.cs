using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public List<Application> Application { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        [ForeignKey("ClubId")]
        public Club Club { get; set; }
        [ForeignKey("RegionId")]
        public Region Region { get; set; }
        public LoginData LoginData { get; set; }
    }
}
