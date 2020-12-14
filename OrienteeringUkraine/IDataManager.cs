using OrienteeringUkraine.Data;
using OrienteeringUkraine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine
{
    public interface IDataManager
    {
        public HomeIndexModel GetEventsInfo(HomeIndexData data);
        public Task<AccountUserModel> GetUserAsync(string login);
        public Task<AccountUserModel> GetUserAsync(string login, string password);
        public Task<AccountUserModel> UpdateUser(string login, AccountUserModel user);
        public Task AddNewUserAsync(AccountRegisterData data);
        public IEnumerable<Region> GetAllRegions();
        public IEnumerable<Club> GetAllClubs();
        public EventApplicationsModel GetApplicationsById(int id);
        public bool IsExistsEvent(int id);
        public EventData GetEventById(int id);
        public void UpdateEvent(int id, EventData data);

        public int AddNewEvent(EventData data);
    }
}
