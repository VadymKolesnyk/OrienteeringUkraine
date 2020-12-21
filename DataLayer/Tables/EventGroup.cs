using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Tables
{
    public class EventGroup
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; }
    }
}
