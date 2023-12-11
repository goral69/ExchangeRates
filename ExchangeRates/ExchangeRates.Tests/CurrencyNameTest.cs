namespace ExchangeRates.Tests
{
    public class CurrencyNameTest
    {
        [Test]
        public void WhenAddCurrencyNameInMemoryShouldReturnCorrectName()
        {
            // arrange
            var currencyInMemory = new ExchangeRatesInMemory("EUR");

            // act
            var currency = currencyInMemory.Currency;

            // assert
             Assert.That("EUR", Is.EqualTo(currencyInMemory.Currency));
        }
    }
}