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
        public HomeIndexModel GetEventsInfo(HomeIndexData data)
        {
            var events = new List<HomeEvent>
            {
                new HomeEvent
                {
                    Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання",
                    EventDate = DateTime.Parse("30-10-2020"),
                    Region = "Львівська",
                    Location = "м. Львів",
                    Organizer = "Бадан Ю.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2"
                },
                new HomeEvent
                {
                    Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання",
                    EventDate = DateTime.Parse("30-10-2020"),
                    Region = "Львівська",
                    Location = "м. Львів",
                    Organizer = "Бадан Ю.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2"
                },
                new HomeEvent
                {
                    Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання",
                    EventDate = DateTime.Parse("30-10-2020"),
                    Region = "Львівська",
                    Location = "м. Львів",
                    Organizer = "Бадан Ю.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2"
                },
                new HomeEvent
                {
                    Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання",
                    EventDate = DateTime.Parse("30-10-2020"),
                    Region = "Львівська",
                    Location = "м. Львів",
                    Organizer = "Бадан Ю.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2"
                },
                new HomeEvent
                {
                    Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання",
                    EventDate = DateTime.Parse("30-10-2020"),
                    Region = "Львівська",
                    Location = "м. Львів",
                    Organizer = "Бадан Ю.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2"
                },
                new HomeEvent
                {
                    Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)",
                    EventDate = DateTime.Parse("04-12-2020"),
                    Region = "м. Київ",
                    Location = "Пуща-Водиця",
                    Organizer = "Гавриленко В.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2"
                },
                new HomeEvent
                {
                    Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)",
                    EventDate = DateTime.Parse("04-12-2020"),
                    Region = "м. Київ",
                    Location = "Пуща-Водиця",
                    Organizer = "Гавриленко В.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2"
                },
                new HomeEvent
                {
                    Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)",
                    EventDate = DateTime.Parse("04-12-2020"),
                    Region = "м. Київ",
                    Location = "Пуща-Водиця",
                    Organizer = "Гавриленко В.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2"
                },
                new HomeEvent
                {
                    Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)",
                    EventDate = DateTime.Parse("04-12-2020"),
                    Region = "м. Київ",
                    Location = "Пуща-Водиця",
                    Organizer = "Гавриленко В.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2"
                },
                new HomeEvent
                {
                    Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)",
                    EventDate = DateTime.Parse("04-12-2020"),
                    Region = "м. Київ",
                    Location = "Пуща-Водиця",
                    Organizer = "Гавриленко В.",
                    InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                    ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2"
                }
            };
            var model = new HomeIndexModel()
            {
                CountPages = 10,
                CurrentPage = data.Page,
                Events = events,
            };
            int id = 0;
            foreach (var item in events)
            {
                item.Id = id++;
            }
            if (model.CurrentPage > model.CountPages)
            {
                model.CurrentPage = model.CountPages;
            }
            return model;
        }


    }
}
