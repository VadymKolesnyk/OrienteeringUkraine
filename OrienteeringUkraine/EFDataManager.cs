using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DataLayer;
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
            throw new NotImplementedException();
        }

        public async Task AddNewUserAsync(AccountRegisterData data)
        {
            DataLayer.Tables.Role defaulftRole = db.Roles.FirstOrDefault(role => role.Name == "sportsman");
            int defaultRoleId = defaulftRole.Id;

            DataLayer.Tables.User newUser = new DataLayer.Tables.User
            {
                Name = data.Name,
                Surname = data.Surname,
                BirthDate = data.Birthday ?? null,
                RoleId = defaultRoleId,
                RegionId = data.RegionId,
                ClubId = data.ClubId ?? null
            };

            EntityEntry<DataLayer.Tables.User> newUserEntry = await db.Users.AddAsync(newUser);
            int userId = newUserEntry.Entity.Id;

            DataLayer.Tables.LoginData newLogin = new DataLayer.Tables.LoginData
            {
                Login = data.Login,
                UserId = userId,
                HashedPassword = HashPassword(data.Password)
            };

            await db.Logins.AddAsync(newLogin);
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
            throw new NotImplementedException();
        }

        public EventData GetEventById(int id)
        {
            throw new NotImplementedException();
        }

        public HomeIndexModel GetEventsInfo(HomeIndexData data)
        {
            throw new NotImplementedException();
        }

        public Task<AccountUserModel> GetUserAsync(string login)
        {
            throw new NotImplementedException();
        }

        public Task<AccountUserModel> GetUserAsync(string login, string password)
        {
            throw new NotImplementedException();
        }

        public bool IsExistsEvent(int id)
        {
            return db.Events.Any(event_ => event_.Id == id);
        }

        public void UpdateEvent(int id, EventData data)
        {
            throw new NotImplementedException();
        }

        public Task<AccountUserModel> UpdateUser(string login, AccountUserModel user)
        {
            throw new NotImplementedException();
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
