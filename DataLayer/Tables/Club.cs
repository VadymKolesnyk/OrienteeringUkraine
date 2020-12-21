using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Tables
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> User { get; set; }
    }
}
