namespace Realeyes.UnitTests
{
    using NUnit.Framework;

    using Realeyes.WebUI.Models;

    [TestFixture]
    public class EcbDataSourceTest
    {
        [Test]
        public void GetEcbExchangeRatesXmlTest()
        {
            var dataSource = new EcbDataSource();

            var data = dataSource.GetEcbExchangeRatesXml();

            Assert.AreNotEqual(null, data);
        }
    }
}
