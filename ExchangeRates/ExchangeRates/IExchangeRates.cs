using static ExchangeRates.ExchangeRatesBase;

namespace ExchangeRates
{
    public interface IExchangeRates
    {
        string Currency { get; }

        public abstract event ExchangeRateAddedDelegate ExchangeRateAdded;

        public abstract event ExchangeRateBelow1dDelegate ExchangeRateBelow1;

        void AddValue(float value);

        void AddValue(int value);

        void AddValue(string value);

        Statistics GetStatistics();

        void ShowStatistics();
    }
}