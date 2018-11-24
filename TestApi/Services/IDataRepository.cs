using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Services
{
    public interface IDataRepository
    {
        Task<Currency> GetConvertedCurrencies(string from, string to, string amount);
        IEnumerable<Currency> GetHistoricalCurrencies(string symbols);



    }
}
