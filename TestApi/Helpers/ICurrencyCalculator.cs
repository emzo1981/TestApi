using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Helpers
{
    public interface ICurrencyCalculator
    {
        IEnumerable<CurrencyRateExchangeResponse> CalculateAmountValues(Currency currency, decimal amount);

        decimal CalculateAverages(List<Currency> currencies);
        decimal CalculateMinimum(List<Currency> currencies);
        decimal CalculateMaximum(List<Currency> currencies);



    }
}
