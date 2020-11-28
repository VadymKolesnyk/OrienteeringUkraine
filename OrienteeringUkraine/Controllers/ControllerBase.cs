using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly IDataManager dataManager;

    }
}
