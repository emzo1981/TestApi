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
        public async Task<ApiResponse> GetLatest(string symbols,string from)
        {
            var uri = _apiClient.CreateRequestUri("latest", $"access_key={FixerKey}&symbols={symbols}&base={from}");
            return await _apiClient.GetAsync<ApiResponse>(uri);
        }     

        public async Task<ApiResponse> GetHistoricalValues(string from, string to, DateTime date )
        {
            var uri = _apiClient.CreateRequestUri($"{date.ToShortDateString()}", $"access_key={FixerKey}&symbols={to}&base={from}");
            return await _apiClient.GetAsync<ApiResponse>(uri);           
        }
    }
}
