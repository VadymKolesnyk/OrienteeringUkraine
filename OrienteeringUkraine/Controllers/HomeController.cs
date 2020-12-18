using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using OrienteeringUkraine.Data;
using OrienteeringUkraine.Models;

namespace OrienteeringUkraine.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(IDataManager dataManager) : base(dataManager) { }
        public IActionResult Index(HomeIndexData data)
        {
            var regions = new SelectList(dataManager.GetAllRegions(), "Id", "Name");
            var selectedRegion = regions.FirstOrDefault(r => r.Value == data?.RegionId.ToString());
            if (selectedRegion != null)
            {
                selectedRegion.Selected = true;
            }
            ViewBag.Regions = regions;

            var years = new SelectList(Enumerable.Range(2011, DateTime.Now.Year + 5 - 2011));
            var selectedYear = years.FirstOrDefault(y => y.Value == data.Year.ToString());
            if (selectedYear != null)
            {
                selectedYear.Selected = true;
            }
            ViewBag.Years = years;

            var monthsArray = new[]
            {
            new {Month = Months.Jan, Name = "Январь" },
            new {Month = Months.Feb, Name = "Февраль" },
            new {Month = Months.Mar, Name = "Март"},
            new {Month = Months.Apr, Name = "Апрель"},
            new {Month = Months.May, Name = "Май"},
            new {Month = Months.June, Name = "Июнь"},
            new {Month = Months.July, Name = "Июль"},
            new {Month = Months.Aug, Name = "Август"},
            new {Month = Months.Sept, Name = "Сентябрь"},
            new {Month = Months.Oct, Name = "Октябрь"},
            new {Month = Months.Nov, Name = "Ноябрь"},
            new {Month = Months.Dec, Name = "Декабрь"},
            };

            var months = new SelectList(monthsArray, "Month", "Name");
            var selectedMounth = years.FirstOrDefault(y => y.Value == data.Month.ToString());
            if (selectedMounth != null)
            {
                selectedMounth.Selected = true;
            }
            ViewBag.Months = months;

            var model = dataManager.GetEventsInfo(data);
            model.RegionId = data.RegionId;
            model.Year = data.Year;
            model.Month = data.Month;

            return View(model);
        }

    }
}
