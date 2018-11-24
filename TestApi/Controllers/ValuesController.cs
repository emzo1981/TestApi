using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TestApi.Helpers;
using TestApi.Services;

namespace TestApi.Controllers
{  
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;
        private readonly ICurrencyValidator _currencyValidator;  
        private readonly string AllowedCurrencies;
        public ValuesController(IDataRepository dataRepository,ICurrencyValidator currencyValidator,IConfiguration configuraton)
        {
            _dataRepository = dataRepository;
            _currencyValidator = currencyValidator;           
            AllowedCurrencies = configuraton.GetValue<string>("AllowedCurrencies");
        }
        // GET api/latest
        [HttpGet]
        [Route("api/convert")]
        public async Task<IActionResult> Convert(string from,string to, string amount)
        {
            if(string.IsNullOrEmpty(from))
                return BadRequest(@"parameter From is missing");
            if (string.IsNullOrEmpty(to))
                return BadRequest(@"parameter To is missing");
            if (string.IsNullOrEmpty(amount))
                return BadRequest(@"parameter Amount is missing");
            
            if (!_currencyValidator.Validate(to))
            {
                return BadRequest(@"This symbol is not allowed : {to}");
            }
            if (!_currencyValidator.Validate(from))
            {
                return BadRequest(@"This symbol is not allowed : {from}");
            }

            
                var Currency = await _dataRepository.GetCurrentCurrencies(from,to,amount);
                return Ok(Currency);
           
            
        }

       
    }
}
