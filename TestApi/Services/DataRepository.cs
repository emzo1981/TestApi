using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Responses;

namespace TestApi.Services
{
    public class DataRepository : IDataRepository
    {
        private readonly IApiClient _apiClient;
        private readonly string FixerKey;


        public DataRepository(IApiClient apiClient,IConfiguration configuration)
        {
            FixerKey = configuration.GetValue<string>("FixerKey");
            _apiClient = apiClient;
        }
        public async Task<Currency> GetLatest(string symbols,string from)
        {
            string uri = $"http://data.fixer.io/api/latest?access_key={FixerKey}&symbols={symbols}&base={from}";
            var result = await _apiClient.GetAsync<ApiResponse>(new Uri($"http://data.fixer.io/api/latest?access_key={FixerKey}&symbols={symbols}&base={from}"));            
            var currency = Mapper.Map<Currency>(result);
            return currency;

        }     

        public async Task<List<Currency>> GetHistoricalValues(string from, string to)
        {
            var _date = DateTime.Now.ToShortDateString();
            var CurrencyList = new List<Currency>();
            for (int i = 1; i <= 7; i++)
            {
                var result = await _apiClient.GetAsync<ApiResponse>(new Uri($"http://data.fixer1.io/api/{_date}?access_key={FixerKey}&symbols={to}&base={from}"));
                var currency = Mapper.Map<Currency>(result);
                CurrencyList.Add(currency);
                _date = DateTime.Now.AddDays(-i).ToShortDateString();
            }
           
            return CurrencyList;
        }
    }
}
