using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;

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
        //public async Task<Currency> GetCurrentCurrencies(string symbols)
        //{
        //   var result = await _apiClient.GetAsync<FixerResponse>(new Uri($"http://data.fixer.io/api/latest?access_key={FixerKey}&symbols={symbols}"));
        //   var currency = Mapper.Map<Currency>(result);
        //   return currency;

        //}
        public async Task<Currency> GetConvertedCurrencies(string from, string to,string amount)
        {
            var result = await _apiClient.GetAsync<FixerResponse>(new Uri($"http://data.fixer.io/api/convert?access_key={FixerKey}&from={from}&to={to}&amount={amount}"));
            var currency = Mapper.Map<Currency>(result);
            return currency;

        }

        public IEnumerable<Currency> GetHistoricalCurrencies(string symbols)
        {
            return new List<Currency>();
        }
    }
}
