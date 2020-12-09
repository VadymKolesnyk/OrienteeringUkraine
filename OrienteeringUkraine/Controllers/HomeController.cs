using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrienteeringUkraine.Data;
using OrienteeringUkraine.Models;

namespace OrienteeringUkraine.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(IDataManager dataManager) : base(dataManager) { }

        public IActionResult Index(HomeIndexData data)
        {
            var model = dataManager.GetEventsInfo(data);
            return View(model);
        }

    }
}
