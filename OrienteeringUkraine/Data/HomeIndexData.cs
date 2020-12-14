using OrienteeringUkraine.Models;

namespace OrienteeringUkraine.Data
{
    public class HomeIndexData
    {
        public int Page { get; set; } = 1;
        public int? Year { get; set; }
        public int? RegionId { get; set; }
        public Months? Month { get; set; } 
    }
}
