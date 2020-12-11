using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    public class EventController : ControllerBase
    {
        public EventController(IDataManager dataManager) : base(dataManager)
        {

        }
        public IActionResult Applications(int id = 0)
        {
            if (!dataManager.IsExistsEvent(id))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(dataManager.GetApplicationsById(id));
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
