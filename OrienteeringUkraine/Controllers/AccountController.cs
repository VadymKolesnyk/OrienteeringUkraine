using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    public class AccountController : ControllerBase
    {
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            throw new Exception();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
