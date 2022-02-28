using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using Currency.Converter.Infrastructure.Services;
using System;
using System.Collections.Generic;

namespace Infrastructure.Test
{

    public class CurrencyConverter
    {
        readonly CurrencyConverterService currencyConverterService;

        public CurrencyConverter()
        {
            var mockLoggerCCS = new Mock<ILogger<CurrencyConverterService>>();

            currencyConverterService = new(mockLoggerCCS.Object);
        }

        [Fact]
        public void ClearConfiguration_None_EmptyDic()
        {
            var before = currencyConverterService._rateDic.Count;
            currencyConverterService.ClearConfiguration();
            var after = currencyConverterService._rateDic.Count;
            Assert.Equal(before, after);
        }

        [Fact]
        public void UpdateConfiguration_SendNoneZeroData_FillDic()
        {
            var before = currencyConverterService._rateDic.Count;

            List<Tuple<string, string, double>> testTuple = new()
            {
                new("USD", "EUR", 0.9),
                new("USD", "CAD", 1.1)
            };
            currencyConverterService.UpdateConfiguration(testTuple);
            var after = currencyConverterService._rateDic.Count;
            Assert.Equal(before+4, after);
        }

        [Fact]
        public void Convert_NoData()
        {
            var response = currencyConverterService.Convert("usd", "eur", 1000);
            Assert.Equal(-1, response);
        }

        [Fact]
        public void Convert_WithData_direct()
        {
            List<Tuple<string, string, double>> testTuple = new()
            {
                new("USD", "EUR", 0.9),
                new("USD", "CAD", 1.1)
            };
            currencyConverterService.UpdateConfiguration(testTuple);
            var response = currencyConverterService.Convert("usd", "eur", 1000);

            Assert.Equal(0.9*1000,response);
        }

        [Fact]
        public void Convert_WithData_Indirect()
        {
            List<Tuple<string, string, double>> testTuple = new()
            {
                new("USD", "EUR", 0.9),
                new("USD", "CAD", 1.1)
            };
            currencyConverterService.UpdateConfiguration(testTuple);
            var response = currencyConverterService.Convert("eur", "cad", 1000);

            Assert.Equal(1/0.9*1.1*1000, response);
        }
    }
}