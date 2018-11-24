using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TestApi.Controllers;
using TestApi.Helpers;
using TestApi.Services;
using Xunit;

namespace XUnitTestProject
{
    public class IndexControllerTest
    {
        [Fact]
        public void Test1()
        {
            var DataRepository = new Mock<IDataRepository>();
            var currencyValidator = new Mock<ICurrencyValidator>();
          
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(@"F:\Projekty Csharp\ASPNet Core\TestApi\XUnitTestProject\appsettings.json");
            IConfiguration configuration = configurationBuilder.Build();
            var currencyCalculator = new Mock<ICurrencyCalculator>();
            IndexController indexController = new IndexController(DataRepository.Object, currencyValidator.Object, configuration, currencyCalculator.Object);


        }
    }
}
