using System;
using System.Collections.Generic;
using System.Linq;

namespace Realeyes.WebUI.Models
{
    using System.Globalization;
    using System.Xml.Linq;

    using Realeyes.WebUI.Abstract;

    using WebGrease.Css.Extensions;

    public class CurrencyExchangeRepository : ICurrencyExchangeRepository
    {
        private readonly IEcbDataSource dataSource;

        private readonly Dictionary<DateTime, Dictionary<string, double>> exchangeRates =
            new Dictionary<DateTime, Dictionary<string, double>>();

        private const string EURO_CURRENCY_NAME = "EUR";

        private const string XML_NAMESPACE = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

        public CurrencyExchangeRepository(IEcbDataSource dataSource)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource");
            }

            this.dataSource = dataSource;
            PrepareExchangeRates();
        }

        public double GetLastExchangeRate(string firstCurrency, string secondCurrency)
        {
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

        public string[] GetAllPossibleCurrencies()
        {
            return exchangeRates.First().Value.Keys.ToArray();
        }

        public double[] GetCurrenciesExchangeHistory(
            string firstCurrency,
            string secondCurrency,
            DateTime beginningDate,
            DateTime endDate)
        {
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
            double deflt = 0d;

            for (DateTime current = beginningDate; current < endDate; current = current.AddDays(1))
            {
                if (!exchangeRates.ContainsKey(current))
                {
                    yield return deflt;
                    continue;
                }

                var rates = exchangeRates[current];

                if (!rates.ContainsKey(upperedCur1) || !rates.ContainsKey(upperedCur2))
                {
                    yield return deflt;
                    continue;
                }

                deflt = rates[upperedCur2] / rates[upperedCur1];
                yield return deflt;
            }
        }

        // <Cube time="2015-02-26">
        //    <Cube currency="USD" rate="1.1317"/>
        private void PrepareExchangeRates()
        {
            var mainXml = this.dataSource.GetEcbExchangeRatesXml();
            if (mainXml == null) return;

            var name = XName.Get("Cube", XML_NAMESPACE);
            var rates = mainXml.Element(name);
            if (rates == null) return;

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

                        var newRates = new Dictionary<string, double> { { EURO_CURRENCY_NAME, 1} };
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

        public static double GetDouble(string value, double defaultValue = 0)
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