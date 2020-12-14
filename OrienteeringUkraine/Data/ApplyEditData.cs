using System;
using System.Collections.Generic;

namespace OrienteeringUkraine.Models
{
    public class ApplyEditData
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }
        public string OrganizerLogin { get; set; }
        public string Region { get; set; }
        public string Location { get; set; }
        public IEnumerable<EventApplication> Applications { get; set; }
    }
}
