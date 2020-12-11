using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using OrienteeringUkraine.Models;

namespace OrienteeringUkraine
{
    public class EFDataManager : IDataManager
    {
        private readonly EFContext db;
        public EFDataManager(EFContext db)
        {
            this.db = db;
        }

        public EventApplicationsModel GetApplicationsById(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsExistsEvent(int id)
        {
            throw new NotImplementedException();
        }
    }
}
