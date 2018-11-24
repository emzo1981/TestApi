using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Helpers
{
    public class CurrencyValidator : ICurrencyValidator
    {
        private readonly string AllowedCurrencies;
        public string NotAllowedCurrencySymbols = "";
        public CurrencyValidator(IConfiguration configuration)
        {
            AllowedCurrencies = configuration.GetValue<string>("AllowedCurrencies");
        }
        public bool Validate(string symbols )
        {            
            
            NotAllowedCurrencySymbols = "";
            var _allowedCurrencies = AllowedCurrencies.Split(",").AsEnumerable<string>();
            var queryCurrencies = symbols.Split(",").AsEnumerable<string>();

            foreach (string queryCurrency in queryCurrencies)
            {
                if (!_allowedCurrencies.Contains(queryCurrency.ToUpper()))
                {
                    NotAllowedCurrencySymbols += queryCurrency + ",";                       
                }
            }
            if (!string.IsNullOrEmpty(NotAllowedCurrencySymbols))
                return false;

            return true;
        }
        public string GetNotAllowedCurrencySymbols()
        {
            return NotAllowedCurrencySymbols;
        }
      
    }
}
