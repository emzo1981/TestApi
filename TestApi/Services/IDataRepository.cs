using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Responses;

namespace TestApi.Services
{
    public interface IDataRepository
    {
        Task<ApiResponse> GetLatest(string symbols,string from);
        Task<ApiResponse> GetHistoricalValues(string from, string to, DateTime date);



    }
}
