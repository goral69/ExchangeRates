using System.Xml;

namespace ExchangeRates
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WriteLineColor(ConsoleColor.DarkGreen, "==========================================================================");
            WriteLineColor(ConsoleColor.DarkGreen, "==========================================================================");
            WriteLineColor(ConsoleColor.DarkGreen, "=====                                                                =====");
            WriteLineColor(ConsoleColor.DarkGreen, "=====               Exchange Rates console application               =====");
            WriteLineColor(ConsoleColor.DarkGreen, "=====                                                                =====");
            WriteLineColor(ConsoleColor.DarkGreen, "==========================================================================");
            WriteLineColor(ConsoleColor.DarkGreen, "==========================================================================");

            bool closeApp = false;

            while (!closeApp)
            {
                Console.WriteLine();
                WriteLineColor(ConsoleColor.Yellow,
                    "1 - Add a currency exchange rate to the program memory and show statistics\n" +
                    "2 - Add a currency exchange rate to the .txt file and show statistics\n" +
                    "Q - Close application\n");
                WriteLineColor(ConsoleColor.DarkYellow, "Please choose");

                var userSelect = Console.ReadKey().Key;

                switch (userSelect)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        AddExchangeRatesToMemory();
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        AddExchangeRatesToTxtFile();
                        break;

                    case ConsoleKey.Q:
                        closeApp = true;
                        break;

                    default:
                        WriteLineColor(ConsoleColor.Red, "\n Invalid operation.\n");
                        continue;
                }
            }
            WriteLineColor(ConsoleColor.Cyan, $"\n\n{"See you. Press any key to exit.",52}");
            Console.ReadKey();
        }

        private static void AddExchangeRatesToMemory()
        {
            string currencyName;
            currencyName = AddCurrencyName();

            if (currencyName == "XXX")
            {
                return;
            }

            var exchangeRatesInMemory = new ExchangeRatesInMemory(currencyName);

            exchangeRatesInMemory.ExchangeRateAdded += ExchangeRateMainAdded;
            exchangeRatesInMemory.ExchangeRateBelow1 += ExchangeRateMainBelow1;
            AddExchangeRate(exchangeRatesInMemory);
            exchangeRatesInMemory.ShowStatistics();
        }

        private static void AddExchangeRatesToTxtFile()
        {
            string currencyName;
            currencyName = AddCurrencyName();

            if (currencyName == "XXX")
            {
                return;
            }

            string fileName = $"{currencyName}-ExRates.txt";
            if (File.Exists(fileName))
            {
                bool closeSwitch = false;
                while (!closeSwitch)
                {
                    WriteLineColor(ConsoleColor.Cyan, $"Source data file for the currency {currencyName} already exists:\n" +
                        "1 - add data\n2 - delete data");

                    var userSelect = Console.ReadKey().Key;

                    switch (userSelect)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            closeSwitch = true;
                            break;

                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            File.Delete(fileName);
                            closeSwitch = true;
                            break;

                        default:
                            WriteLineColor(ConsoleColor.Red, "\n Invalid operation.\n");
                            continue;
                    }
                }

            }

            var exchangeRatesInFiles = new ExchangeRatesInFiles(currencyName);

            exchangeRatesInFiles.ExchangeRateAdded += ExchangeRateMainAdded;
            exchangeRatesInFiles.ExchangeRateBelow1 += ExchangeRateMainBelow1;
            AddExchangeRate(exchangeRatesInFiles);
            exchangeRatesInFiles.ShowStatistics();
        }

        private static void AddExchangeRate(IExchangeRates currency)
        {
            WriteLineColor(ConsoleColor.Yellow, "\nEnter all exchange rates to calculate statistics.\n" +
               "To finish entering data, enter the letter Q.\n");
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            while (true)
            {
                WriteLineColor(ConsoleColor.DarkCyan, $"Enter the exchange rate for the currency {currency.Currency}:");
                var input = Console.ReadLine();
                if (input == "q" || input == "Q")
                {
                    break;
                }
                try
                {
                    currency.AddValue(input);
                }
                catch (Exception e)
                {
                    WriteLineColor(ConsoleColor.Red, $"Exception catch: {e.Message}");
                    WriteLineColor(ConsoleColor.Yellow, "\nTo finish entering data, enter the letter Q.");
                }
            }
        }

        private static string AddCurrencyName()
        {
            string currencyName = "FAULT";
            while (currencyName == "FAULT")
            {
                WriteLineColor(ConsoleColor.Yellow, "\n\nEnter the name of the currency\n" +
                    "Expected ISO 4217 designation - three-letter code\n" +
                    "Capitalization does not matter\n" +
                    "If you want to stop, type xxx");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                currencyName = Console.ReadLine().ToUpper();
                if (currencyName.Length == 3 && currencyName != "XXX")
                {
                    bool isValidCurrencyCode = IsValidCurrencyCode(currencyName);
                    if (!isValidCurrencyCode)
                    {
                        WriteLineColor(ConsoleColor.Red, $"{currencyName} does not comply with the ISO 4217 standard.");
                        currencyName = "FAULT";
                    }
                }
                else
                {
                    if (currencyName == "XXX")      // XXX - special code in ISO 4217 - The codes assigned for transactions where no currency is involved
                    {
                        WriteLineColor(ConsoleColor.Cyan, "Back to main menu.");
                    }
                    else
                    {
                        WriteLineColor(ConsoleColor.Red, $"'{currencyName}' is not a three-letter designation.");
                        currencyName = "FAULT";
                    }
                }
            }
            return currencyName;
        }

        private static bool IsValidCurrencyCode(string currencyName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("list-one.xml");
            // when a new file appears on the website https://www.six-group.com/en/products-services/financial-information/data-standards.html you can always upload it and the program will still work properly
            XmlNodeList currencyNodes = doc.SelectNodes("/ISO_4217/CcyTbl/CcyNtry[Ccy[string-length(normalize-space(.)) > 0]]");

            foreach (XmlNode currencyNode in currencyNodes)
            {
                string currencyCode = currencyNode.SelectSingleNode("Ccy").InnerText;
                if (currencyCode == currencyName)
                {
                    return true;
                }
            }
            return false;
        }

        private static void WriteLineColor(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static void ExchangeRateMainAdded(object sender, EventArgs args)
        {
            WriteLineColor(ConsoleColor.Green, "Added exchange rate\n");
            WriteLineColor(ConsoleColor.Yellow, "To finish entering data, enter the letter Q.");
            Console.Beep();
        }

        private static void ExchangeRateMainBelow1(object sender, EventArgs args)
        {
            WriteLineColor(ConsoleColor.Red, "Exchange rate below 1,0000 !");
        }

    }
}