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
        private readonly EFContext db;
        public EFDataManager(EFContext db) : base()
        {
            this.db = db;
        }

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

            var groups = data.Groups[..^1].Split(";").Distinct();

            foreach (string group in groups)
            {
                AssociateEventAndGroup(newEvent, group);
            }

            db.SaveChanges();

            return newEvent.Id;
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

        public IEnumerable<Club> GetAllClubs()
        {
            var clubs = db.Clubs.Select(club => new Club { Id = club.Id, Name = club.Name }).OrderBy(club => club.Name);
            return clubs;
        }

        public IEnumerable<Region> GetAllRegions()
        {
            var regions = db.Regions.Select(region => new Region { Id = region.Id, Name = region.Name }).OrderBy(region => region.Name);
            return regions;
        }

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

            var groups = from events in db.Events
                         where events.Id == eventInDB.Id
                         join eventGroups in db.EventGroups on events.Id equals eventGroups.EventId
                         join groups_ in db.Groups on eventGroups.GroupId equals groups_.Id
                         orderby groups_.Name ascending
                         select new { Id = groups_.Id, Name = groups_.Name };

            model.Applications = new Dictionary<string, List<EventApplication>>();

            var eventApplications = from events in db.Events
                                    where events.Id == eventInDB.Id
                                    join eventGroups in db.EventGroups on events.Id equals eventGroups.EventId
                                    join applications in db.Applications on eventGroups.Id equals applications.EventGroupId
                                    select new
                                    {
                                        GroupId = eventGroups.GroupId,
                                        ChipId = applications.ChipId,
                                        UserId = applications.UserId
                                    };

            foreach (var group in groups)
            {
                List<EventApplication> applicationsPerGroup = new List<EventApplication>();
                foreach (var application in eventApplications.Where(x => x.GroupId == group.Id))
                {
                    DataLayer.Tables.User user = db.Users.FirstOrDefault(user => user.Id == application.UserId);
                    EventApplication userApplication = new EventApplication
                    {
                        Login = db.Logins.FirstOrDefault(user_ => user_.UserId == user.Id).Login,
                        Name = user.Name + " " + user.Surname,
                        Birthday = (DateTime)(user.BirthDate ?? null),
                        Club = db.Clubs.FirstOrDefault(club => club.Id == user.ClubId).Name,
                        Region  = db.Regions.FirstOrDefault(region => region.Id == user.RegionId).Name,
                        GroupId = group.Id
                    };
                    applicationsPerGroup.Add(userApplication);
                }
                model.Applications.Add(group.Name, applicationsPerGroup);
            }

            return model;
        }

        public EventData GetEventById(int id)
        {
            DataLayer.Tables.Event eventInDB = db.Events.FirstOrDefault(event_ => event_.Id == id);

            if (eventInDB == null)
                return null;

            string joinedGroups = GetEventGroups(eventInDB);

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

        private string GetEventGroups(DataLayer.Tables.Event eventInDB)
        {
            var groups = from events in db.Events
                         where events.Id == eventInDB.Id
                         join eventGroups in db.EventGroups on events.Id equals eventGroups.EventId
                         join groups_ in db.Groups on eventGroups.GroupId equals groups_.Id
                         orderby groups_.Name ascending
                         select new { groups_.Name };

            string joinedGroups = string.Join(";", groups.Select(x => x.Name)) + ";";
            return joinedGroups;
        }

        public HomeIndexModel GetEventsInfo(HomeIndexData data)
        {
            throw new NotImplementedException();
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

        public bool IsExistsEvent(int id)
        {
            return db.Events.Any(@event => @event.Id == id);
        }

        public void UpdateEvent(int id, EventData data)
        {
            DataLayer.Tables.Event eventInDB = db.Events.FirstOrDefault(event_ => event_.Id == id);

            if (eventInDB == null)
                return;

            string eventInDBGroups = GetEventGroups(eventInDB);

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

            if (data.Groups != eventInDBGroups)
            {
                var groupsInDB = eventInDBGroups[..^1].Split(";");
                var editedGroups = data.Groups[..^1].Split(";").Distinct();

                foreach (string group in groupsInDB)
                {
                    if (!editedGroups.Contains(group))
                    {
                        int groupId = db.Groups.FirstOrDefault(group_ => group_.Name == group).Id;
                        DataLayer.Tables.EventGroup eventGroupToDelete = db.EventGroups.FirstOrDefault(eventGroup => eventGroup.EventId == eventInDB.Id && eventGroup.GroupId == groupId);
                        db.EventGroups.Remove(eventGroupToDelete);
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

    }
}
