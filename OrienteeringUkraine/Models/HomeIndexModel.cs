﻿using System.Collections.Generic;

namespace OrienteeringUkraine.Models
{
    public class HomeIndexModel
    {
        public int CountPages { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<HomeEvent> Events { get; set; }
    }
}
