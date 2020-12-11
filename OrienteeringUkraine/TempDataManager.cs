using OrienteeringUkraine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrienteeringUkraine
{
    public class TempDataManager : IDataManager // Потом удалить (тестовый DataManager)
    {
        public EventApplicationsModel GetApplicationsById(int id)
        {
            if (id == 0 || id > 10)
            {
                id = 10;
            }
            var model = new EventApplicationsModel()
            {
                AmountOfPeople = 100,
                AmountOfRentChips = 52,
                Id = id,
            };
            if (id >= 5)
            {
                model.Title = "Відкритий кубок Львова зі спортивного орієнтуванн Lion Cup 2020. Всеукраїнські змагання";
                model.InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1";
                model.ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2";
                model.Date = DateTime.Parse("2020-10-30");
            }
            else
            {
                model.Title = "Чемпіонат України серед дорослих, юніорів, юнаків та ветеранів зі спортивного орієнтування (бігом)";
                model.InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1";
                model.ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2";
                model.Date = DateTime.Parse("2020-12-04");
            }
            var groups = new[] { "Ж12", "Ж14", "М12", "М14" };
            Random random = new Random();
            model.Applications = new Dictionary<string, List<EventApplication>>();
            foreach (var item in groups)
            {
                var a = random.Next(5, 15);
                var applications = new List<EventApplication>();
                for (int i = 0; i < a; i++)
                {
                    applications.Add(new EventApplication()
                    {
                        Birthday = DateTime.Parse("2001-03-24"),
                        Chip = i + 10 * i + 100 * i + 1000000 * a,
                        Club = "}{мельницькі пацыки",
                        Name = "Олександр Дзюбчик " + i,
                        Region = "Хмельницька"
                    });
                }
                model.Applications.Add(item, applications);
            }
            return model;
        }

        public bool IsExistsEvent(int id)
        {
            return (id >= 1 && id <= 10);
        }
    }
}
