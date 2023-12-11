namespace ExchangeRates
{
    public class ExchangeRatesInMemory : ExchangeRatesBase
    {
        public override event ExchangeRateAddedDelegate ExchangeRateAdded;

        private List<float> values = new List<float>();

        public ExchangeRatesInMemory(string currency) : base(currency)
        {
        }

        public override void AddValue(float value)
        {
            this.values.Add(value);

            ExchangeRateAdded?.Invoke(this, new EventArgs());
        }

        public override Statistics GetStatistics()
        {
            var stats = new Statistics();
            foreach (var valueCurrency in this.values)
            {
                stats.AddValue(valueCurrency);
            }

            return stats;
        }
    }
}