using OrienteeringUkraine.Models;

namespace OrienteeringUkraine.Data
{
    public class HomeIndexData
    {
        public int Page { get; set; } = 1;
        public int Year { get; set; } = 0;
        public string Region { get; set; } = null;
        public Months Month { get; set; } = Months.All;
    }
}
