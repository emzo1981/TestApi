using System;
using System.Collections.Generic;
using System.Linq;
using TestApi.Controllers;
using TestApi.Helpers;
using TestApi.Models;
using Xunit;

namespace XUnitTestProject
{
    public class CurrencyCalculatorUnitTest
    {
        private List<Currency> ListCurrency = new List<Currency>();

        public CurrencyCalculatorUnitTest()
        {
            Currency currency = new Currency()
            {
                Name = "USD",
                Rates = new List<Rate>(){
                    new Rate(){ Name = "CHF", RateValue = 2M }                   
                    }
            };
            ListCurrency.Add(currency);

            currency = new Currency()
            {
                Name = "USD",
                Rates = new List<Rate>(){
                    new Rate(){ Name = "CHF", RateValue = 1M }

                    }
            };
            ListCurrency.Add(currency);
            
           
        
        }
        [Fact]
        public void CalculateAverages_Test()
        {
            CurrencyCalculator currencyCalculator = new CurrencyCalculator();
            decimal avg = currencyCalculator.CalculateAverages(ListCurrency);
          
            Assert.Equal(GetRatesList().Average(), avg);
        }
        [Fact]
        public void CalculateMinimum_Test()
        {
            CurrencyCalculator currencyCalculator = new CurrencyCalculator();
            decimal Minimum = currencyCalculator.CalculateMinimum(ListCurrency);
            Assert.Equal(GetRatesList().Min(), Minimum);
        }
        [Fact]
        public void CalculateMaximum_Test()
        {
            CurrencyCalculator currencyCalculator = new CurrencyCalculator();
            decimal Maximum = currencyCalculator.CalculateMaximum(ListCurrency);
            Assert.Equal(GetRatesList().Max(), Maximum);
        }

        private List<decimal> GetRatesList()
        {
            var rates = new List<decimal>();
            foreach (Currency currency in ListCurrency)
            {
                rates.Add(currency.Rates.FirstOrDefault().RateValue);
            }
            return rates;

        }
    }
}
