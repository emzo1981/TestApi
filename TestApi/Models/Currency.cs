﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models
{
    public class Currency
    {
        public string Name { get; set; }
        public List<Rate> Rates { get; set; }
        public DateTimeOffset Date { get; set; }

    }
}
