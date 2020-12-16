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
        private EFDataManager manager;
        public TempDataManager(EFDataManager manager)
        {
            this.manager = manager;
        }
        public EventApplicationsModel GetApplicationsById(int id) => manager.GetApplicationsById(id);
        public async Task AddNewUserAsync(AccountRegisterData data) => await manager.AddNewUserAsync(data);
        public async Task<AccountUserModel> GetUserAsync(string login) => await manager.GetUserAsync(login);
        public async Task<AccountUserModel> GetUserAsync(string login, string password) => await manager.GetUserAsync(login, password);
        public IEnumerable<Region> GetAllRegions() => manager.GetAllRegions();
        public IEnumerable<Club> GetAllClubs() => manager.GetAllClubs();
        public async Task<AccountUserModel> UpdateUser(string login, AccountUserModel data) => await manager.UpdateUser(login, data);
        public int AddNewEvent(EventData data) => manager.AddNewEvent(data);
        public EventData GetEventById(int id) => manager.GetEventById(id);
        public void UpdateEvent(int id, EventData data) => manager.UpdateEvent(id, data);
        public HomeIndexModel GetEventsInfo(HomeIndexData data) => manager.GetEventsInfo(data);

        public IEnumerable<Group> GetGroupsOnEvent(int id)
        {
            return new List<Group>()
            {
                new Group() {Id = 1, Name = "Ж12"},
                new Group() {Id = 2, Name = "Ж14"},
                new Group() {Id = 10, Name = "М12"},
                new Group() {Id = 11, Name = "М14"},
            };
        }


        public bool IsApplied(int EventId, string login)
        {
            return login == "sportsman";
        }

        public void AddNewApplication(int id, string login, int groupId, int? chip)
        {
            
        }

        public void DeleteApplication(int id, string login)
        {
            
        }

        public ApplicationData GetApplication(int id, string login)
        {
            if (login == "moderator")
            {
                return null;
            }
            var res = new ApplicationData()
            {
                CurrentEvent = GetEventById(id),
                Chip = 111222333,
                GroupId = 2
            };
            return res;
        }

        public void UpdateApplication(int id, string login, int groupId, int? chip)
        {
            
        }
    }
}
