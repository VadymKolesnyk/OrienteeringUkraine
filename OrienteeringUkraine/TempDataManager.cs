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

        public TempDataManager()
        {
            int i = 1;
            regions.ForEach(reg => reg.Id = i++);
            i = 1;
            clubs.ForEach(reg => reg.Id = i++);
        }
        private List<LoginInfo> logins = new List<LoginInfo>()
        {
            new LoginInfo() { Login = "admin", Password = "admin", Role = "admin" },
            new LoginInfo() { Login = "moderator", Password = "moderator", Role = "moderator" },
            new LoginInfo() { Login = "organizator", Password = "organizator", Role = "organizator" },
            new LoginInfo() { Login = "sportsmen", Password = "sportsmen", Role = "sportsmen" }
        };

        private List<Region> regions = new List<Region>()
            {
                new Region() {Name = "АР Крым"},
                new Region() {Name = "Винницкая"},
                new Region() {Name = "Волынская"},
                new Region() {Name = "Днепропетровская"},
                new Region() {Name = "Донецкая"},
                new Region() {Name = "Житомирская"},
                new Region() {Name = "Закарпатская"},
                new Region() {Name = "Запорожская"},
                new Region() {Name = "Ивано-Франковская"},
                new Region() {Name = "Киевская"},
                new Region() {Name = "Кировоградская"},
                new Region() {Name = "Луганская"},
                new Region() {Name = "Львовская"},
                new Region() {Name = "Николаевская"},
                new Region() {Name = "Одесская"},
                new Region() {Name = "Полтавская"},
                new Region() {Name = "Ровненская"},
                new Region() {Name = "Сумская"},
                new Region() {Name = "Тернопольская"},
                new Region() {Name = "Харьковская"},
                new Region() {Name = "Херсонская"},
                new Region() {Name = "Хмельницкая"},
                new Region() {Name = "Черкасская"},
                new Region() {Name = "Черниговская"},
                new Region() {Name = "Черновицкая"},
                new Region() {Name = "г. Киев"},
                new Region() {Name = "г. Севастополь"},
            };
        private List<Club> clubs = new List<Club>()
        {
            new Club() {Name = "OdessaTeam"},
            new Club() {Name = "}{мельницькі пацыки"},
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

        public IEnumerable<Region> GetAllRegions()
        {
            return regions;
        }

        public IEnumerable<Club> GetAllClubs()
        {
            return clubs;
        }
    }
}
