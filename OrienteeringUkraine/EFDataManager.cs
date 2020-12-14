using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
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

        public Task AddNewUserAsync(AccountRegisterData data)
        {
            throw new NotImplementedException();
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
    }
}
