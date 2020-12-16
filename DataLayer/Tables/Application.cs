using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Tables
{
    public class Application
    {
        public int Id { get; set; }
        public int EventGroupId { get; set; }
        public int? ChipId { get; set; }
        public int UserId { get; set; }
    }
}
