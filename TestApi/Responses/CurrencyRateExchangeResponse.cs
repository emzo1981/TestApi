using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models
{
    public class CurrencyRateExchangeResponse
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal AmountValue { get; set; }
        public decimal RateValue { get; set; }

    }
}
