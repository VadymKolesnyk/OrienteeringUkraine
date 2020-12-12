using OrienteeringUkraine.Data;
using OrienteeringUkraine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine
{
    public class TempDataManager : IDataManager // Потом удалить (тестовый DataManager)
    {
        private class LoginInfo
        {
            public string Login { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        private List<LoginInfo> logins = new List<LoginInfo>()
        {
            new LoginInfo() { Login = "admin", Password = "admin", Role = "admin" },
            new LoginInfo() { Login = "moderator", Password = "moderator", Role = "moderator" },
            new LoginInfo() { Login = "organizator", Password = "organizator", Role = "organizator" },
            new LoginInfo() { Login = "sportsmen", Password = "sportsmen", Role = "sportsmen" }
        };

        public async Task AddNewUserAsync(AccountRegisterData data)
        {
            logins.Add(new LoginInfo()
            {
                Login = data.Login,
                Password = data.Password,
                Role = "sportsmen"
            });
        }


        public async Task<AccountUser> GetUserAsync(string login)
        {
            var user = await Task.Run(() => logins.FirstOrDefault((u => u.Login == login)));
            return user == null ? null : new AccountUser()
            {
                Login = user.Login,
                Role = user.Role
            };
        }

        public async Task<AccountUser> GetUserAsync(string login, string password)
        {
            var user = await Task.Run(() => logins.FirstOrDefault((u => u.Login == login && u.Password == password)));
            return user == null ? null : new AccountUser()
            {
                Login = user.Login,
                Role = user.Role
            };
        }
    }
}
