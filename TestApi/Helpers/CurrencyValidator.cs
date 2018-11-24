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
    
        public CurrencyValidator(IConfiguration configuration)
        {
            AllowedCurrencies = configuration.GetValue<string>("AllowedCurrencies");
        }
        public bool Validate(string symbol)
        {            
           
            var _allowedCurrencies = AllowedCurrencies.Split(",").AsEnumerable<string>();
            if (_allowedCurrencies.Contains(symbol.ToUpper()))
                return true;
            else
                return false;        
         
        }
      
      
    }
}
