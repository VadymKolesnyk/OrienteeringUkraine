using OrienteeringUkraine.Data;
using OrienteeringUkraine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrienteeringUkraine
{
    /// <summary>
    /// Интерфейс работы с базой данных
    /// </summary>
    public interface IDataManager
    {
        #region Пользователи

        /// <summary>
        /// Получить информацию по пользователю
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Модель - данные пользователя</returns>
        public Task<AccountUserModel> GetUserAsync(string login);
        /// <summary>
        /// Получить информацию по пользователю
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Модель - данные пользователя</returns>
        public Task<AccountUserModel> GetUserAsync(string login, string password);
        /// <summary>
        /// Добавить нового пользователя в базу данных
        /// </summary>
        /// <param name="data">Данные пользователя</param>
        /// <returns></returns>
        public Task AddNewUserAsync(AccountRegisterData data);
        /// <summary>
        /// Обновить данные о пользователе в базе данных
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="user">Изменения</param>
        /// <returns>Модель - данные пользователя</returns>
        public Task<AccountUserModel> UpdateUser(string login, AccountUserModel user);

        #endregion

        #region Соревнования

        /// <summary>
        /// Получить информацию для главной страницы по соревнованиям
        /// </summary>
        /// <param name="data">Фильтры для поиска</param>
        /// <returns>Модель для главной странице</returns>
        public HomeIndexModel GetEventsInfo(HomeIndexData data);
        /// <summary>
        /// Получить список групп на соревнование
        /// </summary>
        /// <param name="id">Индетификатор соревнования</param>
        /// <returns>Список групп на соревнование</returns>
        public IEnumerable<Group> GetGroupsOnEvent(int id);
        /// <summary>
        /// Получить информацию по соревнованию
        /// </summary>
        /// <param name="id">Индетификатор соревнования</param>
        /// <returns>Данные соревнования</returns>
        public EventData GetEventById(int id);
        /// <summary>
        /// Обновить данные соревнования
        /// </summary>
        /// <param name="id">Индетификатор соревнования</param>
        /// <param name="data">Данные для изменения</param>
        /// <returns>Группы, которые удалить не удалось. Пример "Ж12;Ж14;М12;М14;"</returns>
        public string UpdateEvent(int id, EventData data);
        /// <summary>
        /// Добавить новое соревнование 
        /// </summary>
        /// <param name="data">Данные по соревнованиям</param>
        /// <returns>Индетификатор соревнования</returns>
        public int AddNewEvent(EventData data);
        /// <summary>
        /// Удалить соревнование
        /// </summary>
        /// <param name="id">Индетификатор соревнования</param>
        public void DeleteEvent(int id);

        #endregion

        #region Заявки

        /// <summary>
        /// Добавить новую заявку
        /// </summary>
        /// <param name="id">Индетификатор соревнования</param>
        /// <param name="login">Логин пользователя</param>
        /// <param name="groupId">Индетификатор группы</param>
        /// <param name="chip">Чип</param>
        public void AddNewApplication(int id, string login, int groupId, int? chip);
        /// <summary>
        /// Обновить заявку в базе данных
        /// </summary>
        /// <param name="id">Индетификатор соревнования</param>
        /// <param name="login">Логин пользователя</param>
        /// <param name="groupId">Индетификатор группы</param>
        /// <param name="chip">Чип</param>
        public void UpdateApplication(int id, string login, int groupId, int? chip);
        /// <summary>
        /// Удалить заявку 
        /// </summary>
        /// <param name="id">Индетификатор соревнования</param>
        /// <param name="login">Логин пользователя</param>
        public void DeleteApplication(int id, string login);
        /// <summary>
        /// Получить информацию по завке
        /// </summary>
        /// <param name="id">Индетификатор соревнования</param>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Данные по заявке</returns>
        ApplicationData GetApplication(int id, string login);
        /// <summary>
        /// Проверка заявлен ли пользователь на соревнование
        /// </summary>
        /// <param name="EventId">Индетификатор соревнования</param>
        /// <param name="login">Логин пользователя</param>
        /// <returns></returns>
        public bool IsApplied(int EventId, string login);
        /// <summary>
        /// Получить информация по соревнованиям вместе с заявками
        /// </summary>
        /// <param name="id">Индетификатор соревнования</param>
        /// <returns>Модель - данные о соревновании и заявках на него</returns>
        public EventApplicationsModel GetApplicationsById(int id);


        #endregion

        #region Администрирование 

        /// <summary>
        /// Получить список всех ролей
        /// </summary>
        /// <returns>Список всех ролей</returns>
        public IEnumerable<Role> GetAllRoles();
        /// <summary>
        /// Удалить пользователя из базы данных
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="adminLogin">Логин того, кто удалил</param>
        public void DeleteUser(string login, string adminLogin);
        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns>Модель - данные о всех пользователях</returns>
        public ManageUsersModel GetAllUsers();
        /// <summary>
        /// Изменить права доступа пользователю
        /// </summary>
        /// <param name="data">Данные пользователя(логин и новая роль)</param>
        public void UpdateUserRole(ManageEditData data);

        #endregion

        #region Общие

        /// <summary>
        /// Получить список всех регионов
        /// </summary>
        /// <returns>Список всех регионов</returns>
        public IEnumerable<Region> GetAllRegions();
        /// <summary>
        /// Получить список всех клубов
        /// </summary>
        /// <returns>Список всех клубов</returns>
        public IEnumerable<Club> GetAllClubs();

        #endregion
    }
}
