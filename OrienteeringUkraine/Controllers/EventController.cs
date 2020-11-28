using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    public class EventController : ControllerBase
    {
        public IActionResult Applications()
        {
            return View();
        }
        public IActionResult New()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Delete()
        {
            throw new Exception();
        }
        public IActionResult Export()
        {
            throw new Exception();
        }
    }
}
