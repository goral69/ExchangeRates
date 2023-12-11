namespace ExchangeRates
{
    internal class ExchangeRatesInFiles : ExchangeRatesBase
    {
        public override event ExchangeRateAddedDelegate ExchangeRateAdded;

        private const string fileNameSuffix = "-ExRates.txt";

        public ExchangeRatesInFiles(string currency) : base(currency)
        {
        }

        public override void AddValue(float value)
        {
            string fileName = $"{Currency}{fileNameSuffix}";
            using (var writer = File.AppendText(fileName))
            {
                writer.WriteLine(value);
            }

            ExchangeRateAdded?.Invoke(this, new EventArgs());
        }

        private List<float> ValuesFromFile()
        {
            var values = new List<float>();
            string fileName = $"{Currency}{fileNameSuffix}";
            if (File.Exists(fileName))
            {
                using (var reader = File.OpenText(fileName))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        var number = float.Parse(line);
                        values.Add(number);
                        line = reader.ReadLine();
                    }
                }
            }
            return values;
        }

        public override Statistics GetStatistics()
        {
            var valuesFromFile = this.ValuesFromFile();
            var stats = new Statistics();
            foreach (var valueCurrency in valuesFromFile)
            {
                stats.AddValue(valueCurrency);
            }
            return stats;
        }
    }
}