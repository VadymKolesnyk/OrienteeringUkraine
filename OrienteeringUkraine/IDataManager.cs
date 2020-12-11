using OrienteeringUkraine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine
{
    public interface IDataManager
    {

        public EventApplicationsModel GetApplicationsById(int id);
        public bool IsExistsEvent(int id);
    }
}
