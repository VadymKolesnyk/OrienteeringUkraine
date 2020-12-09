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
        protected ControllerBase(IDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

    }
}
