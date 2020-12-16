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
        public EventApplicationsModel GetApplicationsById(int id)
        {
            if (id == 0 || id > 10)
            {
                id = 10;
            }
            var model = new EventApplicationsModel()
            {
                AmountOfPeople = 100,
                AmountOfRentChips = 52,
                Id = id,
            };
            if (id >= 5)
            {
                model.Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання";
                model.InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1";
                model.ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2";
                model.Date = DateTime.Parse("2020-10-30");
                model.OrganizerLogin = "organizer";
                model.Organizer = "Сережа Переяславский";
            }
            else
            {
                model.Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)";
                model.InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1";
                model.ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2";
                model.Date = DateTime.Parse("2020-12-04");
                model.OrganizerLogin = "organizer2";
                model.Organizer = "Макс Гапонюк";
            }
            var groups = new[] { "Ж12", "Ж14", "М12", "М14" };
            Random random = new Random();
            model.Applications = new Dictionary<string, List<EventApplication>>();
            foreach (var item in groups)
            {
                var a = random.Next(5, 15);
                var applications = new List<EventApplication>();
                for (int i = 0; i < a; i++)
                {
                    applications.Add(new EventApplication()
                    {
                        Birthday = DateTime.Parse("2001-03-24"),
                        Chip = i + 10 * i + 100 * i + 1000000 * a,
                        Club = "}{мельницькі пацыки",
                        Name = "Олександр Дзюбчик " + i,
                        Region = "Хмельницька",
                        GroupId = 1,
                    });
                }
                model.Applications.Add(item, applications);
            }
            model.Region = "Хмельницкая";

            return model;
        }

        public bool IsExistsEvent(int id)
        {
            return manager.IsExistsEvent(id);
            //return (id >= 1 && id <= 10);
        }
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
        private EFDataManager manager;
        public TempDataManager(EFDataManager manager)
        {
            this.manager = manager;
            int i = 1;
            regions.ForEach(reg => reg.Id = i++);
            i = 1;
            clubs.ForEach(reg => reg.Id = i++);
        }
        private List<LoginInfo> logins = new List<LoginInfo>()
        {
            new LoginInfo() { Login = "admin",     Password = "admin",     Role = "admin",     Name = "Саша",   Surname = "Дзюбчик",       RegionId = 22, Birthday = DateTime.Parse("2001-03-24")},
            new LoginInfo() { Login = "moderator", Password = "moderator", Role = "moderator", Name = "Катя",   Surname = "Кубышка",       RegionId = 18, },
            new LoginInfo() { Login = "organizer", Password = "organizer", Role = "organizer", Name = "Сережа", Surname = "Переяславский", RegionId = 15, ClubId = 2},
            new LoginInfo() { Login = "organizer2",Password = "organizer2",Role = "organizer", Name = "Макс",   Surname = "Гапонюк",       RegionId = 22, ClubId = 2},
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
            await manager.AddNewUserAsync(data);
            //logins.Add(new LoginInfo()
            //{
            //    Login = data.Login,
            //    Password = data.Password,
            //    Role = "sportsman",
            //    Name = data.Name,
            //    Surname = data.Surname,
            //    ClubId = data.ClubId,
            //    Birthday = data.Birthday,
            //    RegionId = data.RegionId,
            //});
        }


        public async Task<AccountUserModel> GetUserAsync(string login)
        {
            return await manager.GetUserAsync(login);
            //var user = await Task.Run(() => logins.FirstOrDefault((u => u.Login == login)));
            //return user == null ? null : new AccountUserModel()
            //{
            //    Login = user.Login,
            //    Role = user.Role,
            //    Name = user.Name,
            //    Surname = user.Surname,
            //    Club = clubs.FirstOrDefault(club => user.ClubId == club.Id)?.Name,
            //    Birthday = user.Birthday,
            //    RegionId = user.RegionId,
            //    Region = regions.FirstOrDefault(reg => user.RegionId == reg.Id)?.Name,
            //    ClubId = user.ClubId
            //};
        }

        public async Task<AccountUserModel> GetUserAsync(string login, string password)
        {
            return await manager.GetUserAsync(login, password);
            //var user = await Task.Run(() => logins.FirstOrDefault((u => u.Login == login && u.Password == password)));
            //return user == null ? null : new AccountUserModel()
            //{
            //    Login = user.Login,
            //    Role = user.Role,
            //    Name = user.Name,
            //    Surname = user.Surname,
            //    ClubId = user.ClubId,
            //    Club = clubs.FirstOrDefault(club => user.ClubId == club.Id)?.Name,
            //    Birthday = user.Birthday,
            //    RegionId = user.RegionId,
            //    Region = regions.FirstOrDefault(reg => user.RegionId == reg.Id)?.Name
            //};
        }

        public IEnumerable<Region> GetAllRegions()
        {
            return manager.GetAllRegions();
            //return regions.Select(x => new Region() { Id = x.Id, Name= x.Name});
        }

        public IEnumerable<Club> GetAllClubs()
        {
            return manager.GetAllClubs();
            //return clubs;
        }

        public async Task<AccountUserModel> UpdateUser(string login, AccountUserModel data)
        {
            return await manager.UpdateUser(login, data);
            //var user = logins.Find(x => x.Login == login);
            //user.Login = data.Login;
            //user.Name = data.Name;
            //user.Surname = data.Surname;
            //user.ClubId = data.ClubId;
            //user.Birthday = data.Birthday;
            //user.RegionId = data.RegionId;
            //return new AccountUserModel()
            //{
            //    Login = user.Login,
            //    Role = user.Role,
            //    Name = user.Name,
            //    Surname = user.Surname,
            //    ClubId = user.ClubId,
            //    Club = clubs.FirstOrDefault(club => user.ClubId == club.Id)?.Name,
            //    Birthday = user.Birthday,
            //    RegionId = user.RegionId,
            //    Region = regions.FirstOrDefault(reg => user.RegionId == reg.Id)?.Name,
            //};
        }

        public int AddNewEvent(EventData data)
        {
            return manager.AddNewEvent(data);
        }

        public EventData GetEventById(int id)
        {
            return manager.GetEventById(id);
            //if (id == 0 || id > 10)
            //{
            //    return null;
            //}
            //var model = new EventData();
            //if (id >= 5)
            //{
            //    model.Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання";
            //    model.InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1";
            //    model.ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2";
            //    model.Date = DateTime.Parse("2020-10-30");
            //    model.OrganizerLogin = "organizer";
            //    model.RegionId = 6;
            //    model.Location = "м. Львів";
            //}
            //else
            //{
            //    model.Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)";
            //    model.InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1";
            //    model.ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2";
            //    model.Date = DateTime.Parse("2020-12-04");
            //    model.OrganizerLogin = "organizer2";
            //    model.RegionId = 5;
            //}
            //model.Groups = "Ж12;Ж14;М12;М14;";
            //return model;
        }

        public void UpdateEvent(int id, EventData data)
        {

        }
        public HomeIndexModel GetEventsInfo(HomeIndexData data)
        {
            var events = new List<HomeEvent>
            {
                new HomeEvent
                {
                    Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання",
                    EventDate = DateTime.Parse("30-10-2020"),
                    Region = "Львівська",
                    Location = "м. Львів",
                    Organizer = "Бадан Ю.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2"
                },
                new HomeEvent
                {
                    Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання",
                    EventDate = DateTime.Parse("30-10-2020"),
                    Region = "Львівська",
                    Location = "м. Львів",
                    Organizer = "Бадан Ю.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2"
                },
                new HomeEvent
                {
                    Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання",
                    EventDate = DateTime.Parse("30-10-2020"),
                    Region = "Львівська",
                    Location = "м. Львів",
                    Organizer = "Бадан Ю.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2"
                },
                new HomeEvent
                {
                    Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання",
                    EventDate = DateTime.Parse("30-10-2020"),
                    Region = "Львівська",
                    Location = "м. Львів",
                    Organizer = "Бадан Ю.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2"
                },
                new HomeEvent
                {
                    Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання",
                    EventDate = DateTime.Parse("30-10-2020"),
                    Region = "Львівська",
                    Location = "м. Львів",
                    Organizer = "Бадан Ю.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2"
                },
                new HomeEvent
                {
                    Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)",
                    EventDate = DateTime.Parse("04-12-2020"),
                    Region = "м. Київ",
                    Location = "Пуща-Водиця",
                    Organizer = "Гавриленко В.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2"
                },
                new HomeEvent
                {
                    Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)",
                    EventDate = DateTime.Parse("04-12-2020"),
                    Region = "м. Київ",
                    Location = "Пуща-Водиця",
                    Organizer = "Гавриленко В.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2"
                },
                new HomeEvent
                {
                    Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)",
                    EventDate = DateTime.Parse("04-12-2020"),
                    Region = "м. Київ",
                    Location = "Пуща-Водиця",
                    Organizer = "Гавриленко В.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2"
                },
                new HomeEvent
                {
                    Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)",
                    EventDate = DateTime.Parse("04-12-2020"),
                    Region = "м. Київ",
                    Location = "Пуща-Водиця",
                    Organizer = "Гавриленко В.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2"
                },
                new HomeEvent
                {
                    Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)",
                    EventDate = DateTime.Parse("04-12-2020"),
                    Region = "м. Київ",
                    Location = "Пуща-Водиця",
                    Organizer = "Гавриленко В.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2"
                }
            };
            var model = new HomeIndexModel()
            {
                CountPages = 10,
                CurrentPage = data.Page,
                Events = events,
            };
            int id = 1;
            foreach (var item in events)
            {
                item.Id = id++;
            }
            if (model.CurrentPage > model.CountPages)
            {
                model.CurrentPage = model.CountPages;
            }
            if (model.CurrentPage < 1)
            {
                model.CurrentPage = 1;
            }
            return model;
        }
    }
}
