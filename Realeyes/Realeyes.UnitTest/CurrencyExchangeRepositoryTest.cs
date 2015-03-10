
namespace Realeyes.UnitTests
{
    using System.Xml.Linq;
    using System;
    using System.Threading.Tasks;

    using Moq;

    using NUnit.Framework;

    using Realeyes.WebUI.Abstract;
    using Realeyes.WebUI.Models;

    [TestFixture]
    public class CurrencyExchangeRepositoryTest
    {
        [Test]
        public void GetLastExchangeRateTest()
        {
            Mock<IEcbDataSource> mock = new Mock<IEcbDataSource>();
            mock.Setup(p => p.GetEcbExchangeRatesXml())
                .Returns(Task.FromResult(XElement.Parse(Properties.Resources.xml_EcbSample)));

            var repo = new CurrencyExchangeRepository(mock.Object);

            double result = repo.GetLastExchangeRate("EUR", "USD").Result;
            Assert.AreEqual(1.1317d, result);

            result = repo.GetLastExchangeRate("USD", "EUR").Result;
            Assert.AreEqual(1/1.1317d, result);
        }

        [Test]
        public void GetAllPossibleCurrenciesTest()
        {
            Mock<IEcbDataSource> mock = new Mock<IEcbDataSource>();
            mock.Setup(p => p.GetEcbExchangeRatesXml())
                .Returns(Task.FromResult(XElement.Parse(Properties.Resources.xml_EcbSample)));

            var repo = new CurrencyExchangeRepository(mock.Object);

            var all = repo.GetAllPossibleCurrencies();

            Assert.AreEqual(32, all.Result.Length);
        }

        [Test]
        public void GetCurrenciesExchangeHistoryTest()
        {
            Mock<IEcbDataSource> mock = new Mock<IEcbDataSource>();
            mock.Setup(p => p.GetEcbExchangeRatesXml())
                .Returns(Task.FromResult(XElement.Parse(Properties.Resources.xml_EcbSample)));

            var repo = new CurrencyExchangeRepository(mock.Object);

            var history = repo.GetCurrenciesExchangeHistory(
                "USD",
                "EUR",
                new DateTime(2015, 02, 01),
                new DateTime(2015, 03, 01));

            Assert.AreEqual(28, history.Result.Length);
        }
    }
}
