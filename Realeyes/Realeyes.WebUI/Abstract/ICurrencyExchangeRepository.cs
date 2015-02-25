namespace Realeyes.WebUI.Abstract
{
    using System;

    public interface ICurrencyExchangeRepository
    {
        double GetLastExchangeRate(string firstCurrency, string secondCurrency);

        string[] GetAllPossibleCurrencies();

        double[] GetCurrenciesExchangeHistory(
            string firstCurrency,
            string secondCurrency,
            DateTime beginningDate,
            DateTime endDate);
    }
}
