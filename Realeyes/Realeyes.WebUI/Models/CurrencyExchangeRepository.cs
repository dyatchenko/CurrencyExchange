namespace Realeyes.WebUI.Models
{
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Realeyes.WebUI.Abstract;

    using WebGrease.Css.Extensions;

    public class CurrencyExchangeRepository : ICurrencyExchangeRepository
    {
        private const string EURO_CURRENCY_NAME = "EUR";

        private const string XML_NAMESPACE = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

        private readonly Dictionary<DateTime, Dictionary<string, double>> exchangeRates =
            new Dictionary<DateTime, Dictionary<string, double>>();

        private readonly TimeSpan updateInterval = new TimeSpan(1, 0, 0, 0);

        private readonly IEcbDataSource dataSource;

        private DateTime lastTimeStamp = DateTime.MinValue;

        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public CurrencyExchangeRepository(IEcbDataSource dataSource)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource");
            }

            this.dataSource = dataSource;
        }

        public async Task<double> GetLastExchangeRate(string firstCurrency, string secondCurrency)
        {
            await PrepareExchangeRates();

            if (string.IsNullOrWhiteSpace(firstCurrency) || string.IsNullOrWhiteSpace(secondCurrency))
                return 0;

            var lastDate = exchangeRates.Keys.OrderBy(p => p).Last();

            var lastRates = exchangeRates[lastDate];

            var upperedCur1 = firstCurrency.ToUpper();
            var upperedCur2 = secondCurrency.ToUpper();

            if (!lastRates.ContainsKey(upperedCur1) || !lastRates.ContainsKey(upperedCur2)) return 0;

            double rate1 = lastRates[upperedCur1];
            double rate2 = lastRates[upperedCur2];

            return rate2 / rate1;
        }

        public async Task<string[]> GetAllPossibleCurrencies()
        {
            await PrepareExchangeRates();

            return exchangeRates.First().Value.Keys.ToArray();
        }

        public async Task<double[]> GetCurrenciesExchangeHistory(
            string firstCurrency,
            string secondCurrency,
            DateTime beginningDate,
            DateTime endDate)
        {
            await PrepareExchangeRates();
            return GetCurrenciesExchangeHistoryAsEnumerable(
                firstCurrency,
                secondCurrency,
                beginningDate,
                endDate).ToArray();
        }

        private IEnumerable<double> GetCurrenciesExchangeHistoryAsEnumerable(
            string firstCurrency,
            string secondCurrency,
            DateTime beginningDate,
            DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(firstCurrency) || string.IsNullOrWhiteSpace(secondCurrency))
                yield break;

            var upperedCur1 = firstCurrency.ToUpper();
            var upperedCur2 = secondCurrency.ToUpper();
            double deflt = double.NaN;
            int firstZeroesCount = 0;

            for (DateTime current = beginningDate; current < endDate; current = current.AddDays(1))
            {
                if (!exchangeRates.ContainsKey(current))
                {
                    if (double.IsNaN(deflt)) firstZeroesCount++;
                    else yield return deflt;
                    continue;
                }

                var rates = exchangeRates[current];

                if (!rates.ContainsKey(upperedCur1) || !rates.ContainsKey(upperedCur2))
                {
                    if (double.IsNaN(deflt)) firstZeroesCount++;
                    else yield return deflt;
                    continue;
                }

                deflt = rates[upperedCur2] / rates[upperedCur1];

                if (firstZeroesCount != 0)
                {
                    for (int i = 0; i < firstZeroesCount; i++) yield return deflt;
                    firstZeroesCount = 0;
                }

                yield return deflt;
            }
        }

        private async Task PrepareExchangeRates()
        {
            if ((DateTime.Now - lastTimeStamp) < updateInterval) return;

            await semaphore.WaitAsync();

            if ((DateTime.Now - lastTimeStamp) < updateInterval)
            {
                semaphore.Release();
                return;
            }

            var mainXml = await dataSource.GetEcbExchangeRatesXml();
            if (mainXml == null)
            {
                semaphore.Release();
                return;
            }

            try
            {
                ParseXml(mainXml);
                lastTimeStamp = DateTime.Now;
            }
            finally
            {
                semaphore.Release();
            }
        }

        // <Cube time="2015-02-26">
        //    <Cube currency="USD" rate="1.1317"/>
        private void ParseXml(XElement mainXml)
        {
            var name = XName.Get("Cube", XML_NAMESPACE);
            var rates = mainXml.Element(name);
            if (rates == null) return;

            exchangeRates.Clear();
            rates.Elements(name).ForEach(
                p1 =>
                {
                    XAttribute date = p1.Attribute("time");
                    DateTime dateTime;
                    if (date == null
                        || !DateTime.TryParseExact(
                            date.Value,
                            "yyyy-MM-dd",
                            null,
                            DateTimeStyles.None,
                            out dateTime)) return;

                    var newRates = new Dictionary<string, double> { { EURO_CURRENCY_NAME, 1 } };
                    p1.Elements(name).ForEach(
                        p2 =>
                        {
                            XAttribute cur = p2.Attribute("currency");
                            XAttribute rate = p2.Attribute("rate");

                            if (cur == null || rate == null) return;

                            var curValue = cur.Value.ToUpper();
                            if (!newRates.ContainsKey(curValue)) newRates.Add(curValue, GetDouble(rate.Value));
                        });

                    if (!exchangeRates.ContainsKey(dateTime)) exchangeRates.Add(dateTime, newRates);
                });
        }

        private static double GetDouble(string value, double defaultValue = 0)
        {
            double result;

            //Try parsing in the current culture
            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                //Then try in US english
                !double.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                //Then in neutral language
                !double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                result = defaultValue;
            }

            return result;
        }
    }
}