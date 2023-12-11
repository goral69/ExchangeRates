namespace ExchangeRates.Tests
{
    public class ExchangeRatesTests
    {
        [Test]
        public void WhenAddExchangeRatesShouldReturnCorrectCount()
        {
            // arrange
            var exchangeRates = new ExchangeRatesInMemory("GBP");

            // act
            AddExchangeRates(exchangeRates);
            var result = exchangeRates.GetStatistics();

            // assert
            Assert.That(result.Count, Is.EqualTo(4));
        }

        [Test]
        public void WhenAddExchangeRatesShouldReturnCorrectAverage()
        {
            // arrange
            var exchangeRates = new ExchangeRatesInMemory("GBP");

            // act
            AddExchangeRates(exchangeRates);
            var result = exchangeRates.GetStatistics();

            // assert
            Assert.That(Math.Round(result.Average, 4), Is.EqualTo(Math.Round(2.9752f, 4)));
        }

        [Test]
        public void WhenAddExchangeRatesShouldReturnCorrectMin()
        {
            // arrange
            var exchangeRates = new ExchangeRatesInMemory("GBP");

            // act
            AddExchangeRates(exchangeRates);
            var result = exchangeRates.GetStatistics();

            // assert
            Assert.That(Math.Round(result.Min, 4), Is.EqualTo(Math.Round(0.15f, 4)));
        }

        [Test]
        public void WhenAddExchangeRatesShouldReturnCorrectMax()
        {
            // arrange
            var exchangeRates = new ExchangeRatesInMemory("GBP");

            // act
            AddExchangeRates(exchangeRates);
            var result = exchangeRates.GetStatistics();

            // assert
            Assert.That(Math.Round(result.Max, 4), Is.EqualTo(5));
        }

        private static void AddExchangeRates(ExchangeRatesInMemory exchangeRates)
        {
            exchangeRates.AddValue(5);
            exchangeRates.AddValue(0.1500f);
            exchangeRates.AddValue("2,5");
            exchangeRates.AddValue("4,2508");
        }
    }
}