﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataViewer.Models
{
    public class SearchModel
    {
        public string Key { get; set; }
        public string Obj { get; set; }
        public string Tag { get; set; }
        public int? Take { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Sensors { get; set; }
    }
}
