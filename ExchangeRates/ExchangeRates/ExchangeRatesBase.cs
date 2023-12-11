namespace ExchangeRates
{
    public abstract class ExchangeRatesBase : IExchangeRates
    {
        public string Currency { get; private set; }

        public delegate void ExchangeRateAddedDelegate(object sender, EventArgs args);
        public abstract event ExchangeRateAddedDelegate ExchangeRateAdded;

        public delegate void ExchangeRateBelow1dDelegate(object sender, EventArgs args);
        public event ExchangeRateBelow1dDelegate ExchangeRateBelow1;

        public ExchangeRatesBase(string currency)
        {
            this.Currency = currency;
        }

        public abstract void AddValue(float value);

        public void AddValue(int value)
        {
            float result = value;
            this.AddValue(result);
        }

        public void AddValue(string value)
        {
            if (float.TryParse(value, out float result))
            {
                if (result < 1)
                {
                    ExchangeRateBelow1?.Invoke(this, new EventArgs());
                }
                this.AddValue(result);
            }
            else
            {
                throw new Exception("Invalid string value.");
            }
        }

        public abstract Statistics GetStatistics();

        public void ShowStatistics()
        {
            var stat = GetStatistics();
            if (stat.Count != 0)
            {

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\n-----------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"Statistics have been calculated for the currency {Currency}:");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"Number of exchange rates: {stat.Count}");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Average: {stat.Average:N4}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Min    : {stat.Min:N4}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Max    : {stat.Max:N4}");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("-----------------------------------------------------");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Statistics for the {Currency} currency cannot be obtained because there is no data.");
                Console.ResetColor();

            }
        }
    }
}