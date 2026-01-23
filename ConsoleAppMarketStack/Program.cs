using Marketstack.Tests;

class Program
    {
    static void Main(string[] args)
    {
        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Hello, World!");

        var test = new MarketstackServiceTests();
        test.GetExchanges_ReturnsExchanges().GetAwaiter().GetResult();
        test.GetExchangeStocks_ReturnsStocks().GetAwaiter().GetResult();
        test.GetStockEodBars_ReturnsBars().GetAwaiter().GetResult();
        test.GetStockEodBars_Parallel_ReturnsBars().GetAwaiter().GetResult();
    }
}


