﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataViewer.Models
{
    public class SensorSelectViewModel
    {
        public List<SensorModel> Sensors { get; set; }
        public SearchModel SearchModel { get; set; }
    }
}
