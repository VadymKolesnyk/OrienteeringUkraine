using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    [Authorize(Roles = "admin, moderator")]
    public class ManageController : ControllerBase
    {
        public ManageController(IDataManager dataManager) : base(dataManager)
        {

        }
        public IActionResult Users()
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
