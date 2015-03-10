namespace Realeyes.WebUI.Models
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Realeyes.WebUI.Abstract;

    public class EcbDataSource : IEcbDataSource
    {
        private const string ECB_URL = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";

        public async Task<XElement> GetEcbExchangeRatesXml()
        {
            using (WebClient myWebClient = new WebClient())
                return XElement.Parse(await myWebClient.DownloadStringTaskAsync(ECB_URL).ConfigureAwait(false));
        }
    }
}