using Marketstack.Tests;

class Program
    {
    static void Main(string[] args)
    {
        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Hello, World!");

        var test = new MarketstackServiceTests();
        //test.GetExchanges_ReturnsExchanges().GetAwaiter().GetResult();
        //test.GetExchangeStocks_ReturnsStocks().GetAwaiter().GetResult();
        var appleSymbol = "AAPL";
        var fromYear = 2021;
        var fromMonth = 1;
        var fromDay = 1;

        test.GetStockEodBars_ReturnsBars(appleSymbol, fromYear, fromMonth, fromDay).GetAwaiter().GetResult();
        //test.GetStockEodBars_Parallel_ReturnsBars().GetAwaiter().GetResult();
    }
}


