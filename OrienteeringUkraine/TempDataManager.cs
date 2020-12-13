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
            public string Name { get; set; }
            public string Surname { get; set; }
            public DateTime? Birthday { get; set; }
            public int RegionId { get; set; }
            public int? ClubId { get; set; }
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
            new LoginInfo() { Login = "admin",     Password = "admin",     Role = "admin",     Name = "Саша",   Surname = "Дзюбчик",       RegionId = 22, Birthday = DateTime.Parse("2001-03-24")},
            new LoginInfo() { Login = "moderator", Password = "moderator", Role = "moderator", Name = "Катя",   Surname = "Кубышка",       RegionId = 22, },
            new LoginInfo() { Login = "organizer", Password = "organizer", Role = "organizer", Name = "Сережа", Surname = "Переясвавский", RegionId = 18, ClubId = 2},
            new LoginInfo() { Login = "sportsman", Password = "sportsman", Role = "sportsman", Name = "Вадик",  Surname = "Колесник",      RegionId = 15, ClubId = 1, Birthday = DateTime.Parse("2001-02-19")}
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
                Role = "sportsmen",
                Name = data.Name ,
                Surname = data.Surname,
                ClubId = data.ClubId,
                Birthday = data.Birthday,
                RegionId = data.RegionId,
            });
        }


        public async Task<AccountUserModel> GetUserAsync(string login)
        {
            var user = await Task.Run(() => logins.FirstOrDefault((u => u.Login == login)));
            return user == null ? null : new AccountUserModel()
            {
                Login = user.Login,
                Role = user.Role,
                Name = user.Name,
                Surname = user.Surname,
                Club = clubs.FirstOrDefault(club => user.ClubId == club.Id)?.Name,
                Birthday = user.Birthday,
                RegionId = user.RegionId,
                Region = regions.FirstOrDefault(reg => user.RegionId == reg.Id)?.Name,
            };
        }

        public async Task<AccountUserModel> GetUserAsync(string login, string password)
        {
            var user = await Task.Run(() => logins.FirstOrDefault((u => u.Login == login && u.Password == password)));
            return user == null ? null : new AccountUserModel()
            {
                Login = user.Login,
                Role = user.Role,
                Name = user.Name,
                Surname = user.Surname,
                ClubId = user.ClubId,
                Club = clubs.FirstOrDefault(club => user.ClubId == club.Id)?.Name,
                Birthday = user.Birthday,
                RegionId = user.RegionId,
                Region = regions.FirstOrDefault(reg => user.RegionId == reg.Id)?.Name,
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

        public async Task<AccountUserModel> UpdateUser(string login, AccountUserModel data)
        {
            var user = logins.Find(x => x.Login == login);
            user.Login = data.Login;
            user.Name = data.Name;
            user.Surname = data.Surname;
            user.ClubId = data.ClubId;
            user.Birthday = data.Birthday;
            user.RegionId = data.RegionId;
            return new AccountUserModel()
            {
                Login = user.Login,
                Role = user.Role,
                Name = user.Name,
                Surname = user.Surname,
                ClubId = user.ClubId,
                Club = clubs.FirstOrDefault(club => user.ClubId == club.Id)?.Name,
                Birthday = user.Birthday,
                RegionId = user.RegionId,
                Region = regions.FirstOrDefault(reg => user.RegionId == reg.Id)?.Name,
            };
        }
    }
}
