using Marketstack.Tests;

class Program
    {


    // https://stackoverflow.com/questions/38496889/console-chart-drawing
    public static void DrawChart(Dictionary<int, int> dict)
    {
        int consoleWidth = 78;
        int consoleHeight = 20;

        Console.WriteLine(dict.Max(x => x.Key).ToString());

        Func<int, int, bool> IsHit = (hx, hy) => dict.Any(dct => dct.Key / dict.Max(x => x.Key) == hx / dict.Max(x => x.Key) && dct.Value / dict.Max(x => x.Value) == hy / dict.Max(x => x.Value));

        for (int i = 0; i < consoleHeight; i++)
        {
            Console.Write(i == 0 ? '┌' : '│');
            for (int j = 0; j < consoleWidth; j++)
            {
                int actualheight = i * 2;

                if (IsHit(j, actualheight) && IsHit(j, actualheight + 1))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write('█');
                }
                else if (IsHit(j, actualheight))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write('▀');
                }
                else if (IsHit(j, actualheight + 1))
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write('▀');
                }
            }
            Console.ResetColor();
            Console.WriteLine();
        }
        Console.WriteLine('└' + new string('─', (consoleWidth / 2) - 1) + '┴' + new string('─', (consoleWidth / 2) - 1) + '┘');
        Console.Write((dict.Min(x => x.Key) + "/" + dict.Min(x => x.Value)).PadRight(consoleWidth / 3));
        Console.Write((dict.Max(x => x.Value) / 2).ToString().PadLeft(consoleWidth / 3 / 2).PadRight(consoleWidth / 3));
        Console.WriteLine(dict.Max(x => x.Value).ToString().PadLeft(consoleWidth / 3));
    }



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


        //String Symbol = "AAPL";
        String Symbol = "MSFT";

        if (args.Length >= 1)
            Symbol = args[0];
        int fromYear = DateTime.Now.Year - 1;

        int fromMonth = DateTime.Now.Month;
        int fromDay = DateTime.Now.Day;
        if (args.Length == 4)
        { 
            fromYear = Int32.Parse(args[1]);
            fromMonth = Int32.Parse(args[2]);
            fromDay = Int32.Parse(args[3]);
        }

        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string fileName = Symbol + "_" + fromYear.ToString() + "_" + fromMonth.ToString() + "_" + fromDay.ToString() +  ".csv";
        test.GetStockEodBars_ReturnsBars(Symbol, fromYear, fromMonth, fromDay, docPath, fileName).GetAwaiter().GetResult();
        //test.GetStockEodBars_Parallel_ReturnsBars().GetAwaiter().GetResult();




        Dictionary<int, int> chartList = new Dictionary<int, int>()
{
        {50,31}, // x = 50, y = 31
        {71,87},
        {25,66},
        {94,15},
        {33,94}
};
        DrawChart(chartList);
    }
}


