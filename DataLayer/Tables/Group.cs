using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Tables
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<EventGroup> EventGroup { get; set; }
    }
}
