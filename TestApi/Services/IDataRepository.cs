using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Services
{
    public interface IDataRepository
    {
        Task<Currency> GetLatest(string symbols,string from);
        Task<List<Currency>> GetHistoricalValues(string symbols, string from);



    }
}
