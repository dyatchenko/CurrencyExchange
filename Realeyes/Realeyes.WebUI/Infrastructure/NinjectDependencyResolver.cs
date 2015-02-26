namespace Realeyes.WebUI.Infrastructure
{
    using System.Linq;

    namespace ServiceBrokerListener.WebUI.Infrastructure
    {
        using System.Web.Mvc;
        using System;
        using System.Collections.Generic;
        using System.Xml.Linq;

        using Moq;
        using Ninject;

        using Realeyes.WebUI.Abstract;

        public class NinjectDependencyResolver : IDependencyResolver
        {
            private readonly IKernel kernel;

            public NinjectDependencyResolver(IKernel kernelParam)
            {
                kernel = kernelParam;
                AddBindings();
            }

            public object GetService(Type serviceType)
            {
                return kernel.TryGet(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return kernel.GetAll(serviceType);
            }

            /// <summary>
            /// An sample xml: <Cube time="2015-02-16"><Cube currency="USD" rate="1.1408"/><Cube currency="JPY" rate="135.26"/><Cube currency="BGN" rate="1.9558"/><Cube currency="CZK" rate="27.638"/><Cube currency="DKK" rate="7.444"/><Cube currency="GBP" rate="0.7421"/><Cube currency="HUF" rate="306.91"/><Cube currency="PLN" rate="4.1832"/><Cube currency="RON" rate="4.4452"/><Cube currency="SEK" rate="9.5838"/><Cube currency="CHF" rate="1.0626"/><Cube currency="NOK" rate="8.6035"/><Cube currency="HRK" rate="7.714"/><Cube currency="RUB" rate="71.47"/><Cube currency="TRY" rate="2.7919"/><Cube currency="AUD" rate="1.4669"/><Cube currency="BRL" rate="3.2336"/><Cube currency="CAD" rate="1.4206"/><Cube currency="CNY" rate="7.1292"/><Cube currency="HKD" rate="8.85"/><Cube currency="IDR" rate="14524.29"/><Cube currency="ILS" rate="4.4121"/><Cube currency="INR" rate="70.9736"/><Cube currency="KRW" rate="1257.23"/><Cube currency="MXN" rate="16.9608"/><Cube currency="MYR" rate="4.0846"/><Cube currency="NZD" rate="1.5193"/><Cube currency="PHP" rate="50.393"/><Cube currency="SGD" rate="1.5481"/><Cube currency="THB" rate="37.181"/><Cube currency="ZAR" rate="13.2698"/></Cube>
            /// </summary>
            private void AddBindings()
            {
                const string Sample =
                    "<Cube time=\"2015-02-16\"><Cube currency=\"USD\" rate=\"1.1408\"/><Cube currency=\"JPY\" rate=\"135.26\"/><Cube currency=\"BGN\" rate=\"1.9558\"/><Cube currency=\"CZK\" rate=\"27.638\"/><Cube currency=\"DKK\" rate=\"7.444\"/><Cube currency=\"GBP\" rate=\"0.7421\"/><Cube currency=\"HUF\" rate=\"306.91\"/><Cube currency=\"PLN\" rate=\"4.1832\"/><Cube currency=\"RON\" rate=\"4.4452\"/><Cube currency=\"SEK\" rate=\"9.5838\"/><Cube currency=\"CHF\" rate=\"1.0626\"/><Cube currency=\"NOK\" rate=\"8.6035\"/><Cube currency=\"HRK\" rate=\"7.714\"/><Cube currency=\"RUB\" rate=\"71.47\"/><Cube currency=\"TRY\" rate=\"2.7919\"/><Cube currency=\"AUD\" rate=\"1.4669\"/><Cube currency=\"BRL\" rate=\"3.2336\"/><Cube currency=\"CAD\" rate=\"1.4206\"/><Cube currency=\"CNY\" rate=\"7.1292\"/><Cube currency=\"HKD\" rate=\"8.85\"/><Cube currency=\"IDR\" rate=\"14524.29\"/><Cube currency=\"ILS\" rate=\"4.4121\"/><Cube currency=\"INR\" rate=\"70.9736\"/><Cube currency=\"KRW\" rate=\"1257.23\"/><Cube currency=\"MXN\" rate=\"16.9608\"/><Cube currency=\"MYR\" rate=\"4.0846\"/><Cube currency=\"NZD\" rate=\"1.5193\"/><Cube currency=\"PHP\" rate=\"50.393\"/><Cube currency=\"SGD\" rate=\"1.5481\"/><Cube currency=\"THB\" rate=\"37.181\"/><Cube currency=\"ZAR\" rate=\"13.2698\"/></Cube>";
                var random = new Random();
                XElement sampleXml = XElement.Parse(Sample);
                Mock<ICurrencyExchangeRepository> mock = new Mock<ICurrencyExchangeRepository>();
                mock.Setup(m => m.GetAllPossibleCurrencies())
                    .Returns(
                        sampleXml.Elements("Cube")
                            .Select(p => p.Attribute("currency").Value)
                            .ToArray());
                mock.Setup(m => m.GetLastExchangeRate(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns<string, string>((cur1, cur2) => (double)(random.Next(10)));
                mock.Setup(
                    m =>
                    m.GetCurrenciesExchangeHistory(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                    .Returns<string, string, DateTime, DateTime>(
                        (str1, str2, date1, date2) =>
                            {
                                List<double> result = new List<double>();
                                for (int i = 0; i < (date2 - date1).TotalDays; i++) result.Add(random.Next(100));
                                return result.ToArray();
                            });
                kernel.Bind<ICurrencyExchangeRepository>().ToConstant(mock.Object);
            }
        }
    }
}