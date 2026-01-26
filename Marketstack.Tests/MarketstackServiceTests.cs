using Marketstack.Entities.Enums;
using Marketstack.Entities.Exchanges;
using Marketstack.Interfaces;
using Marketstack.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace Marketstack.Tests
{
    public class MarketstackServiceTests
    {
        private readonly IMarketstackService _marketstackService;
        //private const string apiKeyVariable = "ASPNETCORE_MarketstackApiToken";
        //private const string apiKeyVariable = "845448f081c0c3e47687637b4f15effc"; //stefanog1973@gmail.com


        //private const string apiKeyVariable = "e2edce97a88b0205f422a3885671a40c"; //stefano.grippa@reti.it
        private const string apiKeyVariable = "5ac48b9ed2e90521967b6ebb0990d0b5"; //e.grippa@ecfop.net
        
        public MarketstackServiceTests()
        {
            //var apiKey = Environment.GetEnvironmentVariable(apiKeyVariable, EnvironmentVariableTarget.Machine);
            //var options = Options.Create(new MarketstackOptions() { ApiToken = apiKey, MaxRequestsPerSecond = 3, Https = true });
            var options = Options.Create(new MarketstackOptions() { ApiToken = apiKeyVariable, MaxRequestsPerSecond = 3, Https = true });
            _marketstackService = new MarketstackService(options, NullLogger<MarketstackService>.Instance);
        }

        [Fact]
        public async Task GetExchanges_ReturnsExchanges()
        {
            var exchanges = await _marketstackService.GetExchanges();
            Console.WriteLine("number of exchanges = " + exchanges.Count());
            Assert.NotEmpty(exchanges);
            foreach (Marketstack.Entities.Exchanges.Exchange myExchange in exchanges)
            {
                Console.WriteLine(myExchange.Name);
            }
        }

        [Fact]
        public async Task GetExchangeStocks_ReturnsStocks()
        {
            var nasdaqMic = "XNAS";
            var stocks = await _marketstackService.GetExchangeStocks(nasdaqMic);
            Console.WriteLine("number of stocks = " + stocks.Count());
            Assert.True(stocks.Count > 1000);
            foreach (Marketstack.Entities.Stocks.Stock mystock in stocks)
            {
                Console.WriteLine(mystock.Name + " ticker " + mystock.Symbol);
            }
        }

        
        public async Task GetStockEodBars_ReturnsBars(String ticker, int fromYear, int fromMonth, int fromDay, string docPath, string fileName)
        {
            var fromDate = new DateTime(fromYear, fromMonth, fromDay);
            var toDate = DateTime.Now;
            var bars = await _marketstackService.GetStockEodBars(ticker, fromDate, toDate);                
            Assert.NotEmpty(bars);
            var distinctDates = bars.Select(b => b.Date).Distinct().ToList();
            var csv = new StringBuilder();

            Console.WriteLine($"AAPL Bars count: {bars.Count}, Distinct dates count: {distinctDates.Count}");

            Assert.Equal(distinctDates.Count, bars.Count);
            //Assert.True(bars.Count > 100, "Not enough bars");


            //https://learn.microsoft.com/it-it/dotnet/standard/io/how-to-write-text-to-a-file
            

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, fileName)))

                // https://stackoverflow.com/questions/18757097/writing-data-into-csv-file-in-c-sharp
                foreach (Marketstack.Entities.Stocks.StockBar mystockBar in bars)
                {
                    // attenzione nel file csv i decimali sono con il punto e non con la virgola
                    //Suggestion made by KyleMit


                    // https://www.google.com/search?q=c%23+scrivere+numeri+float+in+file+csv&rlz=1C1UEAD_enIT1160IT1160&oq=c%23+scrivere+numeri+float+in+file+csv&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIGCAEQRRg60gEIOTU4MGowajeoAgCwAgA&sourceid=chrome&ie=UTF-8
                    var newLine = string.Format("{0},{1},{2},{3},{4}", mystockBar.Close.ToString().Replace(",", "."),
                                                                       mystockBar.High.ToString().Replace(",", "."), 
                                                                       mystockBar.Low.ToString().Replace(",", "."), 
                                                                       mystockBar.Open.ToString().Replace(",", "."), 
                                                                       mystockBar.Volume.ToString());
                    outputFile.WriteLine(newLine);



                    //Console.WriteLine($"close: {mystockBar.Close}, open: {mystockBar.Open}");
                }
                   
        }
        
        public async Task GetStockEodBars_Parallel_ReturnsBars()
        {
            // 10 stocks
            List<string> symbols = new List<string>() { "AAPL", "MSFT", "GOOG", "VOD", "NVDA", "NFLX", "PEP", "NOW", "VEEV", "MOH" };
            var fromDate = new DateTime(2026, 1, 23);
            var toDate = DateTime.Now;
            var tasks = symbols.Select(async (symbol) => await _marketstackService.GetStockEodBars(symbol, fromDate, toDate));
            var stocksBars = await Task.WhenAll(tasks);

            Assert.True(symbols.Count == stocksBars.Length);

            foreach (Marketstack.Entities.Stocks.StockBar mystockBar in stocksBars.First())
            {
                Console.WriteLine($"close: {mystockBar.Close}, open: {mystockBar.Open}");
            }
        }

        [Fact]
        public async Task GetStockIntraydayBars_ReturnsBars()
        {
            var appleSymbol = "AAPL";
            var fromDate = DateTime.Parse("2026-01-22");
            var toDate = DateTime.Parse("2025-01-23");
            var bars = await _marketstackService.GetStockIntraDayBars(appleSymbol, fromDate, toDate);                
            Assert.NotEmpty(bars);
            Assert.Equal(7, bars.Count);
        }

        [Theory]
        [InlineData(Interval._15min, 23)]
        [InlineData(Interval._30min, 11)]
        [InlineData(Interval._3hour, 2)]
        public async Task GetStockIntraydayBars_WithInterval_ReturnsBars(Interval interval, int expected)
        {
            var appleSymbol = "AAPL";
            var fromDate = DateTime.Parse("2026-01-22");
            var toDate = DateTime.Parse("2026-01-23");
            var bars = await _marketstackService.GetStockIntraDayBars(appleSymbol, fromDate, toDate, interval);                
            Assert.NotEmpty(bars);
            Assert.Equal(expected, bars.Count);
        }
    }
}
