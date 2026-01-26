using Marketstack.Tests;

class Program
    {
    static void Main(string[] args)
    {
        // See https://aka.ms/new-console-template for more information
        //Console.WriteLine("Hello, World!");

        var test = new MarketstackServiceTests();
        //test.GetExchanges_ReturnsExchanges().GetAwaiter().GetResult();
        //test.GetExchangeStocks_ReturnsStocks().GetAwaiter().GetResult();
        //var Symbol = "AAPL";
        //var fromYear = 2021;
        //var fromMonth = 1;
        //var fromDay = 1;


        String Symbol = "AAPL";
        if (args.Length >= 1)
            Symbol = args[0];
        int fromYear = DateTime.Now.Year - 1;

        int fromMonth = DateTime.Now.Month;
        int fromDay = DateTime.Now.Day;
        if (args.Length == 4)
        { 
            fromYear = Int32.Parse(args[0]);
            fromMonth = Int32.Parse(args[1]);
            fromDay = Int32.Parse(args[3]);
        }

        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string fileName = Symbol + "_" + fromYear.ToString() + "_" + fromMonth.ToString() + "_" + fromDay.ToString() +  ".csv";
        test.GetStockEodBars_ReturnsBars(Symbol, fromYear, fromMonth, fromDay, docPath, fileName).GetAwaiter().GetResult();
        //test.GetStockEodBars_Parallel_ReturnsBars().GetAwaiter().GetResult();
    }
}


