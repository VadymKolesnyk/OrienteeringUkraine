using System;
using System.ComponentModel.DataAnnotations;

namespace OrienteeringUkraine.Data
{
    public class EventData
    {
        [Required(ErrorMessage = "Не указано название совернования")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Не указана дата совернования")]
        public DateTime Date { get; set; }
        [Url(ErrorMessage = "Неверно задан url")]
        public string ResultsLink { get; set; }
        [Url(ErrorMessage = "Неверно задан url")]
        public string InfoLink { get; set; }
        public string OrganizerLogin { get; set; }
        [Required(ErrorMessage = "Не указан регион соревнования")]
        public int RegionId { get; set; }
        public string Region { get; set; }
        public string Location { get; set; }
        [Required(ErrorMessage = "Укажите хотя-бы одну группу")]
        [RegularExpression(@"([^;\n]+;)*", ErrorMessage = "Неправильный формат ввода")]
        public string Groups { get; set; }
    }
}
