using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrienteeringUkraine.Data;
using OrienteeringUkraine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrienteeringUkraine.Controllers
{
    public class AccountController : ControllerBase
    {
        public AccountController(IDataManager dataManager) : base(dataManager) { }

        [HttpGet]
        public IActionResult Register()
        {
            SetSelectLists();
            return View();
        }

        private void SetSelectLists()
        {
            ViewBag.Regions = new SelectList(dataManager.GetAllRegions(), "Id", "Name");
            ViewBag.Clubs = new SelectList(dataManager.GetAllClubs(), "Id", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountRegisterData data)
        {
            SetSelectLists();
            if (ModelState.IsValid)
            {
                var user = await dataManager.GetUserAsync(data.Login);
                if (user == null)
                {
                    await dataManager.AddNewUserAsync(data); // добавляем пользователя в бд
                    await Authenticate(new AccountUserModel() { Login = data.Login, Role = "sportsman", Name = data.Name, Surname = data.Surname }); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Такой логин уже существует");
            }
            return View(data);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginData data, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await dataManager.GetUserAsync(data.Login, data.Password);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация

                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(data);
        }

        private async Task Authenticate(AccountUserModel user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
                new Claim("FullName", user.Name + " " + user.Surname)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Profile(string login)
        {
            var user = await dataManager.GetUserAsync(login);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            SetSelectLists();
            var user = await dataManager.GetUserAsync(User.Identity.Name);
            return View(user);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(AccountUserModel data)
        {
            SetSelectLists();
            if (ModelState.IsValid)
            {
                var user = await dataManager.GetUserAsync(data.Login);
                if (user == null || user.Login == User.Identity.Name)
                {
                    var updated = await dataManager.UpdateUser(User.Identity.Name, data);
                    await Authenticate(updated); // аутентификация
                    return RedirectToAction("Profile", "Account", new { login = data.Login});
                }
                ModelState.AddModelError("", "Такой логин уже занят");
            }
            return View(data);
        }
    }
}
