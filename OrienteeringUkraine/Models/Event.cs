using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string InfoLink { get; set; }
        public string ResultsLink { get; set; }
        public string Location { get; set; }
        public string Region { get; set; }
        public string Organizer { get; set; }

    }
}
