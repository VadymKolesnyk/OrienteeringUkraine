using OrienteeringUkraine.Models;
using System.ComponentModel.DataAnnotations;

namespace OrienteeringUkraine.Data
{
    public class ApplicationData
    {
        public EventData CurrentEvent { get; set; }

        [Range(0, 999999999, ErrorMessage = "Недопустимое значение для чипа")]
        public int? Chip { get; set; }
        [Required(ErrorMessage = "Не указана группа")]
        public int GroupId { get; set; }
    }
}
