﻿using System;
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
                BirthDate = data.Birthday,
                RoleId = defaultRoleId,
                RegionId = data.RegionId,
                ClubId = data.ClubId
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

        public async Task<AccountUserModel> GetUserAsync(string login)
        {
            DataLayer.Tables.LoginData userLoginData = db.Logins.FirstOrDefault(@user => @user.Login == login);
            DataLayer.Tables.User userInDB = db.Users.FirstOrDefault(@user => @user.Id == userLoginData.UserId);

            if (userLoginData == null || userInDB == null)
                return null;

            AccountUserModel user = new AccountUserModel
            {
                Login = userLoginData.Login,
                Role = db.Roles.FirstOrDefault(role => role.Id == userInDB.RoleId).Name,
                Name = userInDB.Name,
                Surname = userInDB.Surname,
                Birthday = userInDB.BirthDate,
                RegionId = userInDB.RegionId,
                Region = db.Regions.FirstOrDefault(region => region.Id == userInDB.RegionId).Name,
                ClubId = userInDB.ClubId,
                Club = db.Clubs.FirstOrDefault(club => club.Id == userInDB.ClubId).Name
            };

            return user;
        }

        public async Task<AccountUserModel> GetUserAsync(string login, string password)
        {
            DataLayer.Tables.LoginData userLoginData = db.Logins.FirstOrDefault(@user => @user.Login == login);

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
            throw new NotImplementedException();
        }

        public async Task<AccountUserModel> UpdateUser(string login, AccountUserModel user)
        {
            DataLayer.Tables.LoginData userLoginData = db.Logins.FirstOrDefault(@user => @user.Login == login);
            DataLayer.Tables.User userInDB = db.Users.FirstOrDefault(@user => @user.Id == userLoginData.UserId);

            if (userLoginData == null || userInDB == null)
                return null;

            user.Role = db.Roles.FirstOrDefault(role => role.Id == userInDB.RoleId).Name;
            user.Club = db.Clubs.FirstOrDefault(club => club.Id == user.ClubId).Name;
            user.Region = db.Regions.FirstOrDefault(region => region.Id == user.RegionId).Name;

            userInDB.Name = user.Name;
            userInDB.Surname = user.Surname;
            userInDB.BirthDate = user.Birthday;
            userInDB.RegionId = user.RegionId;
            userInDB.ClubId = user.ClubId;

            db.Logins.Remove(userLoginData);

            db.Logins.Add(
                new DataLayer.Tables.LoginData 
                { 
                    Login = user.Login, 
                    UserId = userInDB.Id, 
                    HashedPassword = userLoginData.HashedPassword 
                });

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
