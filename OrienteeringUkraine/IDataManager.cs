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
        public Task<AccountUser> GetUserAsync(string login);
        public Task<AccountUser> GetUserAsync(string login, string password);
        public Task AddNewUserAsync(AccountRegisterData data);
    }
}
