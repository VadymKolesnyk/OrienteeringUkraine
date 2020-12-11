
using System;
using System.Collections.Generic;

namespace OrienteeringUkraine.Models
{
    public class EventApplicationsModel
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int AmountOfPeople { get; set; }
        public int AmountOfRentChips { get; set; }
        public string ResultsLink { get; set; }
        public string InfoLink { get; set; }
        public int Id { get; set; }
        public Dictionary<string,List<EventApplication>> Applications { get; set; }
    }
}
