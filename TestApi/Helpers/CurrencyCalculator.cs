using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Helpers
{
    public class CurrencyCalculator : ICurrencyCalculator
    {      

        public decimal CalculateAverages(List<Currency> currencies)
        {
            var rates = new List<decimal>();
            foreach (Currency currency in currencies)
            {
                rates.Add(currency.Rates.FirstOrDefault().RateValue);
            }
            return rates.Average();
        }

        public decimal CalculateMaximum(List<Currency> currencies)
        {
            var rates = new List<decimal>();
            foreach (Currency currency in currencies)
            {
                rates.Add(currency.Rates.FirstOrDefault().RateValue);
            }
            return rates.Max();
        }

        public decimal CalculateMinimum(List<Currency> currencies)
        {
            var rates = new List<decimal>();
            foreach (Currency currency in currencies)
            {
                rates.Add(currency.Rates.FirstOrDefault().RateValue);
            }
            return rates.Min();
        }

        public IEnumerable<CurrencyRateExchangeResponse> CalculateAmountValues(Currency currency, decimal amount)
        {
            var CurrenciesList = new List<CurrencyRateExchangeResponse>();

            foreach (Rate entry in currency.Rates)
            {
                var NewCurrency = new CurrencyRateExchangeResponse();
                NewCurrency.To = entry.Name;
                NewCurrency.From = currency.Name;
                NewCurrency.RateValue = entry.RateValue;
                NewCurrency.AmountValue = amount * entry.RateValue;
                CurrenciesList.Add(NewCurrency);
            }

            return CurrenciesList;
        
        }
    }
}
