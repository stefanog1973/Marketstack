using Marketstack.Tests;
using System.Text;

class Program
    {


    // https://stackoverflow.com/questions/38496889/console-chart-drawing

    // https://learn.microsoft.com/it-it/dotnet/standard/io/how-to-open-and-append-to-a-log-file




    //public static void DrawChart(Dictionary<int, int> dict)
    //{
    //    int consoleWidth = 78;
    //    int consoleHeight = 20;

    //    Console.WriteLine(dict.Max(x => x.Key).ToString());

    //    Func<int, int, bool> IsHit = (hx, hy) => dict.Any(dct => dct.Key / dict.Max(x => x.Key) == hx / dict.Max(x => x.Key) && dct.Value / dict.Max(x => x.Value) == hy / dict.Max(x => x.Value));

    //    for (int i = 0; i < consoleHeight; i++)
    //    {
    //        Console.Write(i == 0 ? '┌' : '│');
    //        for (int j = 0; j < consoleWidth; j++)
    //        {
    //            int actualheight = i * 2;

    //            if (IsHit(j, actualheight) && IsHit(j, actualheight + 1))
    //            {
    //                Console.ForegroundColor = ConsoleColor.Red;
    //                Console.BackgroundColor = ConsoleColor.Black;
    //                Console.Write('█');
    //            }
    //            else if (IsHit(j, actualheight))
    //            {
    //                Console.ForegroundColor = ConsoleColor.Red;
    //                Console.BackgroundColor = ConsoleColor.Black;
    //                Console.Write('▀');
    //            }
    //            else if (IsHit(j, actualheight + 1))
    //            {
    //                Console.ForegroundColor = ConsoleColor.Black;
    //                Console.BackgroundColor = ConsoleColor.Red;
    //                Console.Write('▀');
    //            }
    //        }
    //        Console.ResetColor();
    //        Console.WriteLine();
    //    }
    //    Console.WriteLine('└' + new string('─', (consoleWidth / 2) - 1) + '┴' + new string('─', (consoleWidth / 2) - 1) + '┘');
    //    Console.Write((dict.Min(x => x.Key) + "/" + dict.Min(x => x.Value)).PadRight(consoleWidth / 3));
    //    Console.Write((dict.Max(x => x.Value) / 2).ToString().PadLeft(consoleWidth / 3 / 2).PadRight(consoleWidth / 3));
    //    Console.WriteLine(dict.Max(x => x.Value).ToString().PadLeft(consoleWidth / 3));
    //}



    static void Main(string[] args)
    {
        // See https://aka.ms/new-console-template for more information
        MarketstackServiceTests.sb = new StringBuilder();

        var test = new MarketstackServiceTests();

        List<string> Symbols = new List<string>();

        //String Symbol = "AAPL";
        // NON FUNZIONA SU TER.MI !!!!!!!! 
        //Symbols.Append<string>("NYSE:G");

        if (args.Length >= 1)
            Symbols.Append<string>(args[0]);
        int fromYear = DateTime.Now.Year - 1;
        int fromMonth = DateTime.Now.Month;
        int fromDay = DateTime.Now.Day;
        // path di settings e di output
        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string settingsFile = Path.Combine(docPath, "settings.txt");
        if ( !File.Exists(settingsFile))
        {
            string message = "Settings file settings.txt not found in folder " + docPath + "\n";
            MarketstackServiceTests.sb.Append(message);
            Console.WriteLine("Error : " + message);
        }

        // https://stackoverflow.com/questions/6573069/initializing-ienumerablestring-in-c-sharp
        IEnumerable<String> mySettings = File.ReadLines(settingsFile);
        //try
        //{
        //    mySettings = File.ReadLines(settingsFile);
        //}
        //catch(Exception ex)
        //{
        //    string message = "Error reading settings file settings.txt from folder" + ex.Message + "\n";
        //    MarketstackServiceTests.sb.Append(message);
        //    Console.WriteLine("Error : " + message);
        //}
        string myPeriod = "10y";
        foreach (string line in mySettings)
        {
            if (line.StartsWith("period="))
            {
                myPeriod = line.Substring("period=".Length);
            }
            if (line.StartsWith("apiKeyVariable="))
            {
                MarketstackServiceTests.apiKeyVariable = line.Substring("apiKeyVariable=".Length);
            }
            if (line.StartsWith("fromYear="))
            {
                fromYear = Int32.Parse(line.Substring("fromYear=".Length));
            }
            if ((line.StartsWith("fromMonth=")) && (line.Length > "fromMonth=".Length))
            {
                fromMonth = Int32.Parse(line.Substring("fromMonth=".Length));
            }
            if ((line.StartsWith("fromDay=")) && (line.Length > "fromDay=".Length))
            {
                fromDay = Int32.Parse(line.Substring("fromDay=".Length));
            }
            if (line.StartsWith("Symbol="))
            {
                String miaStringa = line.Substring("Symbol=".Length);
                int inizio = 0;
                string jobId = "";
                int endIndex = 0;
                while (miaStringa != "")
                {
                    endIndex = miaStringa.IndexOf(" ", inizio);
                    if (endIndex == -1)
                    { 
                        Symbols.Add(miaStringa);
                        break;
                    }
                    jobId = miaStringa.Substring(inizio, endIndex);
                    miaStringa = miaStringa.Replace(jobId, "");
                    miaStringa = miaStringa.Remove(0, 1);
                    Symbols.Add(jobId);
                }
                if (Symbols.Count() == 0)
                    Symbols.Add(line.Substring("Symbol=".Length));
            }

        }
        if (myPeriod == "10y")
        {
            fromYear = DateTime.Now.Year - 10;
        }


        if (args.Length == 4)
        {
            fromYear = Int32.Parse(args[1]);
            fromMonth = Int32.Parse(args[2]);
            fromDay = Int32.Parse(args[3]);
        }
        if (Symbols.Count() == 0)
            Symbols.Add("AAPL");
        foreach (string Symbol in Symbols)
        {
            string fileName = Symbol + "_" + fromYear.ToString() + "_" + fromMonth.ToString() + "_" + fromDay.ToString() + ".csv";
            try
            {
                test.GetStockEodBars_ReturnsBars(Symbol, fromYear, fromMonth, fromDay, docPath, fileName).GetAwaiter().GetResult();
                Console.WriteLine("Getting data for " + Symbol + " from " + fromYear.ToString() + "-" + fromMonth.ToString() + "-" + fromDay.ToString());
            }
            catch (Exception ex)
            {
                
                string message = "Error getting data for " + Symbol + " from " + fromYear.ToString() + "-" + fromMonth.ToString() + "-" + fromDay.ToString() + " : " + ex.Message + "\n";
                MarketstackServiceTests.sb.Append(message);
                Console.WriteLine("Error : " + message);


            }
            
        }
        String data = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
        File.AppendAllText(docPath + "\\log_" + data + ".txt", MarketstackServiceTests.sb.ToString());
        MarketstackServiceTests.sb.Clear();
    }
}


