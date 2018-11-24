using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models
{
    public class FixerResponse
    {
        public string Base { get; set; }
        public DateTimeOffset Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }


    }
}
