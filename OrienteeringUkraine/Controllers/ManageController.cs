using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrienteeringUkraine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    [Authorize(Roles = "admin, moderator")]
    public class ManageController : ControllerBase
    {
        public ManageController(IDataManager dataManager) : base(dataManager) { }
        private void SetSelectLists()
        {
            ViewBag.Roles = new SelectList(dataManager.GetAllRoles(), "Id", "Name");
        }

        public IActionResult Users()
        {
            SetSelectLists();
            var model = dataManager.GetAllUsers();
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ManageEditData data)
        {
            dataManager.UpdateUserRole(data);
            return RedirectToAction("Users");
        }

        public IActionResult Delete(string login)
        {
            dataManager.DeleteUser(login);
            return RedirectToAction("Users");
        }
    }
}
