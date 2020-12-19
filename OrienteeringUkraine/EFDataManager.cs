using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OrienteeringUkraine.Data;
using OrienteeringUkraine.Models;

namespace OrienteeringUkraine
{
    public class EFDataManager : IDataManager
    {
        #region Инициализация

        private readonly EFContext db;
        private const int numberOfEventsOnPage = 10;
        public EFDataManager(EFContext db) : base()
        {
            this.db = db;
            StartInitializationDatabase.SeedDB(db, HashPassword);
        }

        #endregion

        #region Пользователи

        public async Task AddNewUserAsync(AccountRegisterData data)
        {
            DataLayer.Tables.Role defaulftRole = await db.Roles.FirstOrDefaultAsync(role => role.Name == "sportsman");
            int defaultRoleId = defaulftRole.Id;

            DataLayer.Tables.User newUser = new DataLayer.Tables.User
            {
                Name = data.Name,
                Surname = data.Surname,
                BirthDate = data.Birthday,
                RoleId = defaultRoleId,
                RegionId = data.RegionId,
                ClubId = data.ClubId
            };

            await db.Users.AddAsync(newUser);
            await db.SaveChangesAsync();

            int userId = newUser.Id;

            DataLayer.Tables.LoginData newLogin = new DataLayer.Tables.LoginData
            {
                Login = data.Login,
                UserId = userId,
                HashedPassword = HashPassword(data.Password)
            };

            await db.Logins.AddAsync(newLogin);
            await db.SaveChangesAsync();
        }
        public async Task<AccountUserModel> GetUserAsync(string login)
        {
            DataLayer.Tables.LoginData userLoginData = await db.Logins.FirstOrDefaultAsync(@user => @user.Login == login);

            if (userLoginData == null)
                return null;

            DataLayer.Tables.User userInDB = await db.Users.FirstOrDefaultAsync(@user => @user.Id == userLoginData.UserId);

            if (userInDB == null)
                return null;

            AccountUserModel user = new AccountUserModel
            {
                Login = userLoginData.Login,
                Role = (await db.Roles.FirstOrDefaultAsync(role => role.Id == userInDB.RoleId)).Name,
                Name = userInDB.Name,
                Surname = userInDB.Surname,
                Birthday = userInDB.BirthDate,
                RegionId = userInDB.RegionId,
                Region = (await db.Regions.FirstOrDefaultAsync(region => region.Id == userInDB.RegionId))?.Name,
                ClubId = userInDB.ClubId,
                Club = (await db.Clubs.FirstOrDefaultAsync(club => club.Id == userInDB.ClubId))?.Name
            };

            return user;
        }
        public async Task<AccountUserModel> GetUserAsync(string login, string password)
        {
            DataLayer.Tables.LoginData userLoginData = await db.Logins.FirstOrDefaultAsync(@user => @user.Login == login);

            if (userLoginData == null)
                return null;

            if (!VerifyHashedPassword(userLoginData.HashedPassword, password))
                return null;

            return await GetUserAsync(login);
        }
        public async Task<AccountUserModel> UpdateUser(string login, AccountUserModel user)
        {
            DataLayer.Tables.LoginData userLoginData = db.Logins.FirstOrDefault(@user => @user.Login == login);
            if (userLoginData == null)
                return null;

            DataLayer.Tables.User userInDB = db.Users.FirstOrDefault(@user => @user.Id == userLoginData.UserId);
            if (userInDB == null)
                return null;


            user.Role = db.Roles.FirstOrDefault(role => role.Id == userInDB.RoleId).Name;
            user.Club = db.Clubs.FirstOrDefault(club => club.Id == user.ClubId)?.Name;
            user.Region = db.Regions.FirstOrDefault(region => region.Id == user.RegionId).Name;

            userInDB.Name = user.Name;
            userInDB.Surname = user.Surname;
            userInDB.BirthDate = user.Birthday;
            userInDB.RegionId = user.RegionId;
            userInDB.ClubId = user.ClubId;

            if (userLoginData.Login != user.Login)
            {
                db.Logins.Remove(userLoginData);

                db.Logins.Add(
                    new DataLayer.Tables.LoginData
                    {
                        Login = user.Login,
                        UserId = userInDB.Id,
                        HashedPassword = userLoginData.HashedPassword
                    });
            }

            await db.SaveChangesAsync();

            return user;
        }

        #endregion

        #region Хеширование пароля
 
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }
        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }
        
        #endregion

        #region Соревнования
 
        public int AddNewEvent(EventData data)
        {
            DataLayer.Tables.Event newEvent = new DataLayer.Tables.Event
            {
                Title = data.Title,
                EventDate = data.Date,
                InfoLink = data.InfoLink,
                ResultsLink = data.ResultsLink,
                Location = data.Location,
                RegionId = data.RegionId,
                OrganizerId = db.Logins.First(user => user.Login == data.OrganizerLogin).UserId
            };

            db.Events.Add(newEvent);
            db.SaveChanges();

            var groups = data.Groups[..^1].Split(";").Distinct().Select(gr => gr.Trim());

            foreach (string group in groups)
            {
                AssociateEventAndGroup(newEvent, group);
            }

            db.SaveChanges();

            return newEvent.Id;
        }
        public EventData GetEventById(int id)
        {
            DataLayer.Tables.Event eventInDB = db.Events.FirstOrDefault(event_ => event_.Id == id);

            if (eventInDB == null)
                return null;

            IEnumerable<Group> temp = GetGroupsOnEvent(eventInDB.Id);

            string joinedGroups = string.Join(";", temp.Select(x => x.Name)) + ";";

            EventData queriedEvent = new EventData
            {
                Title = eventInDB.Title,
                Date = eventInDB.EventDate,
                ResultsLink = eventInDB.ResultsLink,
                InfoLink = eventInDB.InfoLink,
                OrganizerLogin = db.Logins.First(organizer => organizer.UserId == eventInDB.OrganizerId).Login,
                RegionId = eventInDB.RegionId,
                Location = eventInDB.Location,
                Groups = joinedGroups
            };

            return queriedEvent;
        }
        public HomeIndexModel GetEventsInfo(HomeIndexData data)
        {
            IEnumerable<DataLayer.Tables.Event> events = db.Events;
            if (data.RegionId != null)
            {
                events = events?.Where(e => e.RegionId == data.RegionId);
            }
            if (data.Month != null)
            {
                events = events?.Where(e => e.EventDate.Month == (int)data.Month);
            }
            if (data.Year != null)
            {
                events = events?.Where(e => e.EventDate.Year == data.Year);
            }

            var res = new HomeIndexModel();
            if (events == null)
            {
                res.Events = null;
                res.CurrentPage = 1;
                res.CountPages = 1;
            }
            else
            {
                IEnumerable<Event> homeEvents = from e in events
                                                join u in db.Users on e.OrganizerId equals u.Id
                                                join r in db.Regions on e.RegionId equals r.Id
                                                orderby e.EventDate ascending
                                                select new Event
                                                {
                                                    Id = e.Id,
                                                    EventDate = e.EventDate,
                                                    InfoLink = e.InfoLink,
                                                    ResultsLink = e.ResultsLink,
                                                    Location = e.Location,
                                                    Title = e.Title,
                                                    Organizer = u.Name + " " + u.Surname,
                                                    Region = r.Name
                                                };
                int amountOfPages = (homeEvents.Count() - 1) / numberOfEventsOnPage + 1;
                int startIndex = (data.Page - 1) * numberOfEventsOnPage;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                if (startIndex > homeEvents.Count())
                {
                    startIndex = (amountOfPages - 1) * numberOfEventsOnPage;
                }
                int finishIndex = Math.Min(startIndex + numberOfEventsOnPage, homeEvents.Count());
                res.Events = homeEvents.ToArray()[startIndex..finishIndex];
                res.CurrentPage = startIndex / numberOfEventsOnPage + 1;
                res.CountPages = amountOfPages;
            }
            return res;
        }
        public string UpdateEvent(int id, EventData data)
        {
            DataLayer.Tables.Event eventInDB = db.Events.FirstOrDefault(event_ => event_.Id == id);

            if (eventInDB == null)
                return null;

            IEnumerable<Group> eventGroups = GetGroupsOnEvent(eventInDB.Id);

            string eventInDBGroups = string.Join(";", eventGroups.Select(x => x.Name)) + ";";

            if (data.Title != eventInDB.Title)
                eventInDB.Title = data.Title;

            if (data.Date != eventInDB.EventDate)
                eventInDB.EventDate = data.Date;

            if (data.ResultsLink != eventInDB.ResultsLink)
                eventInDB.ResultsLink = data.ResultsLink;

            if (data.InfoLink != eventInDB.InfoLink)
                eventInDB.InfoLink = data.ResultsLink;

            if (data.RegionId != eventInDB.RegionId)
                eventInDB.RegionId = data.RegionId;

            if (data.Location != eventInDB.Location)
                eventInDB.Location = data.Location;

            string remainingGroups = "";

            if (data.Groups != eventInDBGroups)
            {
                var groupsInDB = eventInDBGroups[..^1].Split(";");
                var editedGroups = data.Groups[..^1].Split(";").Distinct().Select(g => g.Trim());


                foreach (string group in groupsInDB)
                {
                    if (!editedGroups.Contains(group))
                    {
                        int groupId = db.Groups.FirstOrDefault(group_ => group_.Name == group).Id;
                        DataLayer.Tables.EventGroup eventGroupToDelete = db.EventGroups.FirstOrDefault(eventGroup => eventGroup.EventId == eventInDB.Id && eventGroup.GroupId == groupId);

                        DataLayer.Tables.Application existingApplication = db.Applications.FirstOrDefault(application => application.EventGroupId == eventGroupToDelete.Id);

                        if (existingApplication == null)
                        {
                            db.EventGroups.Remove(eventGroupToDelete);
                        }
                        else
                        {
                            DataLayer.Tables.Group remainingGroup = db.Groups.FirstOrDefault(group => group.Id == eventGroupToDelete.GroupId);
                            remainingGroups += (remainingGroup.Name + ";");
                        }
                    }
                }

                foreach (string group in editedGroups)
                {
                    if (!groupsInDB.Contains(group))
                    {
                        AssociateEventAndGroup(eventInDB, group);
                    }
                }
            }

            db.SaveChanges();

            return remainingGroups;
        }
        public IEnumerable<Group> GetGroupsOnEvent(int eventId)
        {
            IEnumerable<Group> groups = from events in db.Events
                                        where events.Id == eventId
                                        join eventGroups in db.EventGroups on events.Id equals eventGroups.EventId
                                        join groups_ in db.Groups on eventGroups.GroupId equals groups_.Id
                                        orderby groups_.Name ascending
                                        select new Group { Id = groups_.Id, Name = groups_.Name };
            return groups;
        }
        public void DeleteEvent(int id)
        {
            DataLayer.Tables.Event eventInDB = db.Events.FirstOrDefault(event_ => event_.Id == id);

            if (eventInDB == null)
                return;

            var applications = from events in db.Events
                               where events.Id == eventInDB.Id
                               join eventGroups in db.EventGroups on events.Id equals eventGroups.EventId
                               join applications_ in db.Applications on eventGroups.Id equals applications_.EventGroupId
                               select applications_;

            if (applications != null)
                db.Applications.RemoveRange(applications);

            var eventGroupChains = db.EventGroups.Where(chain => chain.EventId == eventInDB.Id);

            if (eventGroupChains != null)
                db.EventGroups.RemoveRange(eventGroupChains);

            db.Events.Remove(eventInDB);

            db.SaveChanges();
        }
        #endregion

        #region Заявки

        public EventApplicationsModel GetApplicationsById(int id)
        {
            EventApplicationsModel model = new EventApplicationsModel();

            DataLayer.Tables.Event eventInDB = db.Events.FirstOrDefault(@event => @event.Id == id);

            if (eventInDB == null)
                return null;

            DataLayer.Tables.User organizer = db.Users.FirstOrDefault(user => user.Id == eventInDB.OrganizerId);
            DataLayer.Tables.LoginData organizerLoginData = db.Logins.FirstOrDefault(user => user.UserId == organizer.Id);
            DataLayer.Tables.Region eventRegion = db.Regions.FirstOrDefault(region => region.Id == eventInDB.RegionId);

            model.Title = eventInDB.Title;
            model.Date = eventInDB.EventDate;
            model.ResultsLink = eventInDB.ResultsLink;
            model.InfoLink = eventInDB.InfoLink;
            model.Id = eventInDB.Id;
            model.Organizer = organizer.Name + " " + organizer.Surname;
            model.OrganizerLogin = organizerLoginData.Login;
            model.Region = eventRegion.Name;
            model.Location = eventInDB.Location;

            IEnumerable<Group> groups = GetGroupsOnEvent(eventInDB.Id);

            model.Applications = new Dictionary<string, List<EventApplication>>();

            var eventApplications = from events in db.Events
                                    where events.Id == eventInDB.Id
                                    join eventGroups in db.EventGroups on events.Id equals eventGroups.EventId
                                    join applications in db.Applications on eventGroups.Id equals applications.EventGroupId
                                    select new
                                    {
                                        eventGroups.GroupId,
                                        applications.ChipId,
                                        applications.UserId
                                    };

            foreach (Group group in groups)
            {
                List<EventApplication> applicationsPerGroup = new List<EventApplication>();
                foreach (var application in eventApplications.Where(x => x.GroupId == group.Id))
                {
                    DataLayer.Tables.User user = db.Users.FirstOrDefault(user => user.Id == application.UserId);
                    EventApplication userApplication = new EventApplication
                    {
                        Login = db.Logins.FirstOrDefault(user_ => user_.UserId == user.Id).Login,
                        Name = user.Name + " " + user.Surname,
                        Birthday = user.BirthDate,
                        Club = db.Clubs.FirstOrDefault(club => club.Id == user.ClubId)?.Name,
                        Region = db.Regions.FirstOrDefault(region => region.Id == user.RegionId).Name,
                        Chip = application.ChipId,
                        GroupId = group.Id
                    };
                    applicationsPerGroup.Add(userApplication);
                }
                model.Applications.Add(group.Name, applicationsPerGroup);
            }

            model.AmountOfPeople = eventApplications.Count();
            model.AmountOfRentChips = eventApplications.Count(x => x.ChipId == null);

            return model;
        }
        private void AssociateEventAndGroup(DataLayer.Tables.Event curEvent, string group)
        {
            int groupId;

            DataLayer.Tables.Group groupInDB = db.Groups.FirstOrDefault(group_ => group_.Name == group);

            if (groupInDB == null)
            {
                DataLayer.Tables.Group newGroup = new DataLayer.Tables.Group { Name = group };
                db.Groups.Add(newGroup);
                db.SaveChanges();
                groupId = newGroup.Id;
            }
            else
            {
                groupId = groupInDB.Id;
            }

            db.EventGroups.Add(
                new DataLayer.Tables.EventGroup
                {
                    GroupId = groupId,
                    EventId = curEvent.Id
                });
        }
        public bool IsApplied(int eventId, string login)
        {
            var userApplications = from logins in db.Logins
                                   where logins.Login == login
                                   join applications in db.Applications on logins.UserId equals applications.UserId
                                   join eventGroups in db.EventGroups on applications.EventGroupId equals eventGroups.Id
                                   select new
                                   {
                                       eventGroups.EventId
                                   };
            if (userApplications != null)
                return userApplications.Any(x => x.EventId == eventId);
            return false;
        }
        public void AddNewApplication(int eventId, string login, int groupId, int? chip)
        {
            DataLayer.Tables.LoginData userLoginData = db.Logins.FirstOrDefault(user => user.Login == login);

            if (userLoginData == null)
                return;

            DataLayer.Tables.User user = db.Users.FirstOrDefault(user => user.Id == userLoginData.UserId);

            if (user == null)
                return;

            var userApplicationOnEvent = from eventGroups in db.EventGroups
                                         where eventGroups.EventId == eventId && eventGroups.GroupId == groupId
                                         join applications in db.Applications on eventGroups.Id equals applications.EventGroupId
                                         select new { applications.UserId };

            if (userApplicationOnEvent != null && userApplicationOnEvent.Any(x => x.UserId == user.Id))
                return;

            DataLayer.Tables.EventGroup eventGroupChain = db.EventGroups.FirstOrDefault(eventGroup => eventGroup.EventId == eventId && eventGroup.GroupId == groupId);

            int eventGroupId = eventGroupChain.Id;

            if (eventGroupChain == null)
            {
                DataLayer.Tables.EventGroup newEventGroupChain = new DataLayer.Tables.EventGroup
                {
                    GroupId = groupId,
                    EventId = eventId
                };

                db.EventGroups.Add(newEventGroupChain);
                db.SaveChanges();

                eventGroupId = newEventGroupChain.Id;
            }

            db.Applications.Add(
                new DataLayer.Tables.Application
                {
                    EventGroupId = eventGroupId,
                    ChipId = chip,
                    UserId = user.Id
                });

            db.SaveChanges();
        }
        public ApplicationData GetApplication(int eventId, string login)
        {
            DataLayer.Tables.LoginData userLoginData = db.Logins.FirstOrDefault(user => user.Login == login);

            if (userLoginData == null)
                return null;

            DataLayer.Tables.User user = db.Users.FirstOrDefault(user => user.Id == userLoginData.UserId);

            if (user == null)
                return null;

            DataLayer.Tables.Application userApplication = GetApplicationByUserId(eventId, user.Id);

            if (userApplication == null)
                return null;

            ApplicationData application = new ApplicationData
            {
                CurrentEvent = GetEventById(eventId),
                Chip = userApplication.ChipId,
                GroupId = (db.EventGroups.FirstOrDefault(eventGroup => eventGroup.Id == userApplication.EventGroupId)).GroupId
            };

            return application;
        }
        public void DeleteApplication(int eventId, string login)
        {
            DataLayer.Tables.LoginData userLoginData = db.Logins.FirstOrDefault(user => user.Login == login);

            if (userLoginData == null)
                return;

            DataLayer.Tables.User user = db.Users.FirstOrDefault(user => user.Id == userLoginData.UserId);

            if (user == null)
                return;

            DataLayer.Tables.Application userApplication = GetApplicationByUserId(eventId, user.Id);

            if (userApplication == null)
                return;

            db.Applications.Remove(userApplication);
            db.SaveChanges();
        }
        public DataLayer.Tables.Application GetApplicationByUserId(int eventId, int userId)
        {
            DataLayer.Tables.Application userApplication = (from eventGroups in db.EventGroups
                                                            where eventGroups.EventId == eventId
                                                            join applications in db.Applications on eventGroups.Id equals applications.EventGroupId
                                                            select applications).FirstOrDefault(application => application.UserId == userId);
            return userApplication;
        }
        public void UpdateApplication(int eventId, string login, int groupId, int? chip)
        {
            DataLayer.Tables.LoginData userLoginData = db.Logins.FirstOrDefault(user => user.Login == login);

            if (userLoginData == null)
                return;

            DataLayer.Tables.User user = db.Users.FirstOrDefault(user => user.Id == userLoginData.UserId);

            if (user == null)
                return;

            DataLayer.Tables.Application userApplication = GetApplicationByUserId(eventId, user.Id);

            if (userApplication == null)
                return;

            DataLayer.Tables.EventGroup eventGroupChain = db.EventGroups.FirstOrDefault(eventGroup => eventGroup.EventId == eventId && eventGroup.GroupId == groupId);

            if (userApplication.EventGroupId != eventGroupChain.Id)
                userApplication.EventGroupId = eventGroupChain.Id;

            if (userApplication.ChipId != chip)
                userApplication.ChipId = chip;

            db.SaveChanges();
        }

        #endregion

        #region Администрирование 
        public IEnumerable<Role> GetAllRoles()
        {
            IEnumerable<Role> allRoles = from roles in db.Roles 
                                         select new Role
                                         {
                                             Id = roles.Id,
                                             Name = roles.Name
                                         };
            return allRoles;
        }
        public void DeleteUser(string login)
        {
            DataLayer.Tables.LoginData userLoginData = db.Logins.FirstOrDefault(user_ => user_.Login == login);

            if (userLoginData == null)
                return;

            DataLayer.Tables.User userInDB = db.Users.FirstOrDefault(user_ => user_.Id == userLoginData.UserId);

            if (userInDB == null)
                return;

            var userApplications = db.Applications.Where(application => application.UserId == userInDB.Id);

            if (userApplications != null)
                db.Applications.RemoveRange(userApplications);

            db.Logins.Remove(userLoginData);
            db.Users.Remove(userInDB);
            db.SaveChanges();
        }
        public ManageUsersModel GetAllUsers()
        {
            int adminRoleId = db.Roles.FirstOrDefault(role => role.Name == "admin").Id;
            int moderatorRoleId = db.Roles.FirstOrDefault(role => role.Name == "moderator").Id;
            int organizerRoleId = db.Roles.FirstOrDefault(role => role.Name == "organizer").Id;
            int sportsmanRoleId = db.Roles.FirstOrDefault(role => role.Name == "sportsman").Id;

            int totalUsers = db.Users.Count();
            int totalAdmins = db.Users.Where(user => user.RoleId == adminRoleId).Count();
            int totalModerators = db.Users.Where(user => user.RoleId == moderatorRoleId).Count();
            int totalOrganizers = db.Users.Where(user => user.RoleId == organizerRoleId).Count();
            int totalSportsmen = db.Users.Where(user => user.RoleId == sportsmanRoleId).Count();

            ManageUsersModel model = new ManageUsersModel
            {
                AmountOfUsers = totalUsers,
                AmountOfAdmins = totalAdmins,
                AmountOfModerators = totalModerators,
                AmountOfOrganizers = totalOrganizers,
                AmountOfSportsmen = totalSportsmen
            };

            model.Users = from users in db.Users
                          join logins in db.Logins on users.Id equals logins.UserId
                          join roles in db.Roles on users.RoleId equals roles.Id
                          join regions in db.Regions on users.RegionId equals regions.Id
                          join clubs in db.Clubs on users.ClubId equals clubs.Id
                          select new AccountUserModel
                          {
                              Login = logins.Login,
                              Role = roles.Name,
                              RoleId = roles.Id,
                              Name = users.Name,
                              Surname = users.Surname,
                              Birthday = users.BirthDate,
                              RegionId = regions.Id,
                              Region = regions.Name,
                              ClubId = clubs.Id,
                              Club = clubs.Name
                          };

            return model;
        }
        public void UpdateUserRole(ManageEditData data)
        {
            DataLayer.Tables.LoginData userLoginData = db.Logins.FirstOrDefault(user => user.Login == data.Login);

            if (userLoginData == null)
                return;

            DataLayer.Tables.User userInDB = db.Users.FirstOrDefault(user => user.Id == userLoginData.UserId);

            if (userInDB == null)
                return;

            userInDB.RoleId = data.RoleId;

            db.SaveChanges();
        }

        #endregion

        #region Общие

        public IEnumerable<Club> GetAllClubs()
        {
            var clubs = db.Clubs.Select(club => new Club { Id = club.Id, Name = club.Name }).OrderBy(club => club.Name);
            return clubs;
        }
        public IEnumerable<Region> GetAllRegions()
        {
            var regions = db.Regions.Select(region => new Region { Id = region.Id, Name = region.Name }).OrderBy(region => region.Name);
            Console.WriteLine(regions.GetType());
            return regions;
        }

        #endregion

    }
}
