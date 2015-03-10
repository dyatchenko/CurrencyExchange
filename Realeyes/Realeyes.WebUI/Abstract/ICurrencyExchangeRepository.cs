namespace Realeyes.WebUI.Abstract
{
    using System;
    using System.Threading.Tasks;

    public interface ICurrencyExchangeRepository
    {
        Task<double> GetLastExchangeRate(string firstCurrency, string secondCurrency);

        Task<string[]> GetAllPossibleCurrencies();

        Task<double[]> GetCurrenciesExchangeHistory(
            string firstCurrency,
            string secondCurrency,
            DateTime beginningDate,
            DateTime endDate);
    }
}
