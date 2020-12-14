using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    public class ApplyController : ControllerBase
    {
        public ApplyController(IDataManager dataManager) : base(dataManager)
        {

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
    }
}
