using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TestApi.Helpers;
using TestApi.Models;
using TestApi.Responses;
using TestApi.Services;

namespace TestApi.Controllers
{  
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;
        private readonly ICurrencyValidator _currencyValidator;  
        private readonly ICurrencyCalculator _currencyCalculator;
        private readonly string AllowedCurrencies;

        public ValuesController(IDataRepository dataRepository,ICurrencyValidator currencyValidator,IConfiguration configuraton, ICurrencyCalculator currencyCalculator)
        {
            _dataRepository = dataRepository;
            _currencyValidator = currencyValidator;           
            AllowedCurrencies = configuraton.GetValue<string>("AllowedCurrencies");
            _currencyCalculator = currencyCalculator;
        }
        // GET api/latest
        [HttpGet]
        [Route("api/latest")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
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
            if (!Decimal.TryParse(amount,out amountValue))
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
            
            
            var Currency = await _dataRepository.GetLatest(symbols, from);
            var calculatedCurrencies =  _currencyCalculator.CalculateRates(Currency, amountValue);
            
            return Ok(calculatedCurrencies);
            

        }
        // GET api/convert

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
            if (!Decimal.TryParse(amount, out amountValue))
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
            Currency currency = await _dataRepository.GetLatest(to, from);
           
            var calculatedCurrencies = _currencyCalculator.CalculateRates(currency, amountValue);

            return Ok(calculatedCurrencies);
        }
        // GET api/average

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
            

            var Currency = await _dataRepository.GetHistoricalValues(from,to);
            var average = _currencyCalculator.CalculateAverages(Currency);
            var response = new CurrencyAverageResponse(){ Average = average };
            return Ok(response);


        }
        // GET api/maximum

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

            var Currency = await _dataRepository.GetHistoricalValues(from, to);

            var maximum = _currencyCalculator.CalculateMaximum(Currency);
            var response = new CurrencyMaximumResponse() { Maximum = maximum };
            return Ok(response);


        }
        // GET api/minimum

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

            var Currency = await _dataRepository.GetHistoricalValues(from, to);

            var minimum = _currencyCalculator.CalculateMinimum(Currency);
            var response = new CurrencyMinimumResponse() { Minimum = minimum };

            return Ok(response);


        }



    }
}
