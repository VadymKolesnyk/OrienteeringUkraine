using OrienteeringUkraine.Data;
using OrienteeringUkraine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine
{
    public interface IDataManager
    {
        public HomeIndexModel GetEventsInfo(HomeIndexData data);
        public IEnumerable<Region> GetAllRegions();
    }
}
