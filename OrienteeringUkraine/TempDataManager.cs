using OrienteeringUkraine.Data;
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
        }

        private List<LoginInfo> logins = new List<LoginInfo>()
        {
            new LoginInfo() { Login = "vadik", Password = "1234" },
            new LoginInfo() { Login = "admin", Password = "admin" },
            new LoginInfo() { Login = "katya", Password = "qwerty" }
        };

        public async Task AddNewUserAsync(AccountRegisterData data)
        {
            Console.WriteLine("-------------------");
            Console.WriteLine(data.Login);
            Console.WriteLine(data.Password);
            Console.WriteLine("-------------------");
        }

        public async Task<bool> IsExistsLoginAsyns(string login)
        {
            return false;
        }

        public async Task<bool> IsValidAuthorizeAsyns(string login, string password)
        {
            return await Task.Run(() => logins.FirstOrDefault((u => u.Login == login && u.Password == password))) != null;
        }
    }
}
