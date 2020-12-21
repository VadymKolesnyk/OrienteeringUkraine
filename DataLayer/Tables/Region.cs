using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Tables
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> User { get; set; }
        public List<Event> Event { get; set; }
    }
}
