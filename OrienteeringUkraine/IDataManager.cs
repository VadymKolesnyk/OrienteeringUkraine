using OrienteeringUkraine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine
{
    public interface IDataManager
    {
        public Task<bool> IsExistsLoginAsyns(string login);
        public Task<bool> IsValidAuthorizeAsyns(string login, string password);
        public Task AddNewUserAsync(AccountRegisterData data);
    }
}
