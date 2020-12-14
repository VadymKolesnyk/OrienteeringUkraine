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
        public EFDataManager(EFContext db)
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

        public void AppNewApplication(int id, string login, int groupId, int? chip)
        {
            throw new NotImplementedException();
        }

        public void DeleteApplication(int id, string login)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Club> GetAllClubs()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Region> GetAllRegions()
        {
            throw new NotImplementedException();
        }

        public ApplicationData GetApplication(int id, string login)
        {
            throw new NotImplementedException();
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

        public IEnumerable<Group> GetGroupsOnEvent(int id)
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

        public bool IsApplied(int EventId, string login)
        {
            throw new NotImplementedException();
        }

        public void UpdateApplication(int id, string login, int groupId, int? chip)
        {
            throw new NotImplementedException();
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
