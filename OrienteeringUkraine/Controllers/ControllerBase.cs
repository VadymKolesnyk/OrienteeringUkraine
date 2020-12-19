using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected readonly IDataManager dataManager;
        protected readonly ICacheManager cacheManager;
        protected ControllerBase(IDataManager dataManager, ICacheManager cacheManager = null)
        {
            this.dataManager = dataManager;
            this.cacheManager = cacheManager;
        }

    }
}
