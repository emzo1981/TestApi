using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TestApi.Helpers;
using TestApi.Models;
using TestApi.Responses;
using TestApi.Services;

namespace TestApi.Controllers
{  
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;
        private readonly ICurrencyValidator _currencyValidator;  
        private readonly ICurrencyCalculator _currencyCalculator;
        private readonly string AllowedCurrencies;

        public IndexController(IDataRepository dataRepository,ICurrencyValidator currencyValidator,IConfiguration configuraton, ICurrencyCalculator currencyCalculator)
        {
            _dataRepository = dataRepository;
            _currencyValidator = currencyValidator;           
            AllowedCurrencies = configuraton.GetValue<string>("AllowedCurrencies");
            _currencyCalculator = currencyCalculator;
        }
        /// <summary>
        /// convert any amount from one currency to another currencies 
        /// </summary>
        /// <param name="symbols">symbols of currencies separated with , </param>    
        /// <param name="amount">amount to convert</param>    
        /// <param name="from">base currency</param>    
        [HttpGet]
        [Route("api/latest")]
  
        public async Task<IActionResult> GetLatest(string symbols,string amount,string from)
        {
            if (string.IsNullOrEmpty(from))
            {
                return BadRequest($"from parameter can't be empty");
            }
            if (string.IsNullOrEmpty(amount))
            {
                return BadRequest($"amount parameter can't be empty");
            }
            decimal amountValue = 0;
            if (!Decimal.TryParse(amount.Replace(".",","),out amountValue))
            {
                return BadRequest($"amount parameter must be a number");
            }
            if (string.IsNullOrEmpty(symbols))
            {
                symbols = AllowedCurrencies;
            }
            if (!AllowedCurrencies.Contains(from.ToUpper()))
            {
                return BadRequest($"{from} symbol is not allowed");
            }
            if (!_currencyValidator.Validate(symbols))
            {
                return BadRequest($"Those symbols are not allowed : {_currencyValidator.GetNotAllowedCurrencySymbols()}");
            }
            
            
            var apiResponse = await _dataRepository.GetLatest(symbols, from);
            if (!apiResponse.Success)
            {
                return BadRequest(apiResponse.Error.Type);
            }
            var currency = Mapper.Map<Currency>(apiResponse);         
            var calculatedCurrencies =  _currencyCalculator.CalculateAmountValues(currency, amountValue);
            
            return Ok(calculatedCurrencies);
            

        }
        /// <summary>
        /// convert any amount from one currency to another
        /// </summary>
        /// <param name="to">currency to convert </param>    
        /// <param name="amount">amount to convert</param>    
        /// <param name="from">base currency</param>    
        [HttpGet]
        [Route("api/convert")]
        public async Task<IActionResult> GetConvert(string from, string to, string amount)
        {
            if (string.IsNullOrEmpty(to))
            {
                return BadRequest($"to parameter can't be empty");
            }
            if (string.IsNullOrEmpty(amount))
            {
                return BadRequest($"amount parameter can't be empty");
            }
            if (string.IsNullOrEmpty(from))
            {
                return BadRequest($"from parameter can't be empty");
            }
            decimal amountValue = 0;
            if (!Decimal.TryParse(amount.Replace(".", ","), out amountValue))
            {
                return BadRequest($"amount parameter must be a number");
            }
            if (!AllowedCurrencies.Contains(from.ToUpper()))
            {
                return BadRequest($"{from} symbol is not allowed");
            }
            if (!AllowedCurrencies.Contains(to.ToUpper()))
            {
                return BadRequest($"{to} symbol is not allowed");
            }
            var apiResponse = await _dataRepository.GetLatest(to, from);
            if (!apiResponse.Success)
            {
                return BadRequest(apiResponse.Error.Type);
            }

            var currency = Mapper.Map<Currency>(apiResponse);
            var calculatedCurrencies = _currencyCalculator.CalculateAmountValues(currency, amountValue);           

            return Ok(calculatedCurrencies);
        }
        /// <summary>
        /// Gets average rate from last 7 days
        /// </summary>
        /// <param name="to">currency to compare </param>            
        /// <param name="from">base currency</param>    
        [HttpGet]
        [Route("api/average")]
        public async Task<IActionResult> GetAverage(string from, string to)
        {
            if (string.IsNullOrEmpty(from))
            {
                return BadRequest($"from parameter can't be empty");
            }
            if (string.IsNullOrEmpty(to))
            {
                return BadRequest($"to parameter can't be empty");
            }
           
            if (!AllowedCurrencies.Contains(from.ToUpper()))
            {
                return BadRequest($"{from} symbol is not allowed");
            }
            if (!AllowedCurrencies.Contains(to.ToUpper()))
            {
                return BadRequest($"{to} symbol is not allowed");
            }

            var _date = DateTime.Now;
            var CurrencyList = new List<Currency>();
            for (int i = 1; i <= 7; i++)
            {
                var apiResponse = await _dataRepository.GetHistoricalValues(from, to, _date);
                if (!apiResponse.Success)
                {
                    return BadRequest(apiResponse.Error.Type);
                }
                var currency = Mapper.Map<Currency>(apiResponse);
                CurrencyList.Add(currency);
                _date = _date.AddDays(-i);
            }

            var average = _currencyCalculator.CalculateAverages(CurrencyList);
            var response = new CurrencyAverageResponse(){ Average = average };
            return Ok(response);


        }
        /// <summary>
        /// Gets maximum rate from last 7 days
        /// </summary>
        /// <param name="to">currency to compare </param>            
        /// <param name="from">base currency</param>    
        [HttpGet]
        [Route("api/maximum")]
        public async Task<IActionResult> GetMaximum(string from, string to)
        {
            if (string.IsNullOrEmpty(from))
            {
                return BadRequest($"from parameter can't be empty");
            }
            if (string.IsNullOrEmpty(to))
            {
                return BadRequest($"to parameter can't be empty");
            }

            if (!AllowedCurrencies.Contains(from.ToUpper()))
            {
                return BadRequest($"{from} symbol is not allowed");
            }
            if (!AllowedCurrencies.Contains(to.ToUpper()))
            {
                return BadRequest($"{to} symbol is not allowed");
            }

            var _date = DateTime.Now;
            var CurrencyList = new List<Currency>();
            for (int i = 1; i <= 7; i++)
            {
                var apiResponse = await _dataRepository.GetHistoricalValues(from, to, _date);
                if (!apiResponse.Success)
                {
                    return BadRequest(apiResponse.Error.Type);
                }
                var currency = Mapper.Map<Currency>(apiResponse);
                CurrencyList.Add(currency);
                _date = _date.AddDays(-i);
            }


            var maximum = _currencyCalculator.CalculateMaximum(CurrencyList);
            var response = new CurrencyMaximumResponse() { Maximum = maximum };
            return Ok(response);


        }
        /// <summary>
        /// Gets minimum rate from last 7 days
        /// </summary>
        /// <param name="to">currency to compare </param>            
        /// <param name="from">base currency</param>    
        [HttpGet]
        [Route("api/minimum")]
        public async Task<IActionResult> GetMinimum(string from, string to)
        {

            if (string.IsNullOrEmpty(from))
            {
                return BadRequest($"from parameter can't be empty");
            }
            if (string.IsNullOrEmpty(to))
            {
                return BadRequest($"to parameter can't be empty");
            }

            if (!AllowedCurrencies.Contains(from.ToUpper()))
            {
                return BadRequest($"{from} symbol is not allowed");
            }
            if (!AllowedCurrencies.Contains(to.ToUpper()))
            {
                return BadRequest($"{to} symbol is not allowed");
            }

            var _date = DateTime.Now;
            var CurrencyList = new List<Currency>();
            for (int i = 1; i <= 7; i++)
            {
                var apiResponse = await _dataRepository.GetHistoricalValues(from, to, _date);
                if (!apiResponse.Success)
                {
                    return BadRequest(apiResponse.Error.Type);
                }
                var currency = Mapper.Map<Currency>(apiResponse);
                CurrencyList.Add(currency);
                _date = _date.AddDays(-i);
            }


            var minimum = _currencyCalculator.CalculateMinimum(CurrencyList);
            var response = new CurrencyMinimumResponse() { Minimum = minimum };

            return Ok(response);


        }



    }
}
