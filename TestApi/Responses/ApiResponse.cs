using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Responses
{
    public class ApiResponse
    {
        public string Base { get; set; }
        public DateTimeOffset Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
        public bool Success { get; set; }
        public ApiResponseError Error { get; set; }


    }
}
