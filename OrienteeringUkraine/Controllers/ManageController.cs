using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrienteeringUkraine.Data;
using OrienteeringUkraine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    [Authorize(Roles = "admin, moderator")]
    public class ManageController : ControllerBase
    {
        public ManageController(IDataManager dataManager, ICacheManager cacheManager) : base(dataManager, cacheManager) { }
        private void SetSelectLists()
        {
            var roles = cacheManager.GetRoles();
            if (roles == null)
            {
                roles = dataManager.GetAllRoles();
                cacheManager.SetRoles(roles);
            }
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
        }

        public IActionResult Users()
        {
            SetSelectLists();
            var model = dataManager.GetAllUsers() ?? new ManageUsersModel();
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
            dataManager.DeleteUser(login, User.Identity.Name);
            return RedirectToAction("Users");
        }
    }
}
