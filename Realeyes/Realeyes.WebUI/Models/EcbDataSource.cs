namespace Realeyes.WebUI.Models
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Realeyes.WebUI.Abstract;

    public class EcbDataSource : IEcbDataSource
    {
        private const string ECB_URL = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";

        public async Task<XElement> GetEcbExchangeRatesXml()
        {
            Func<XElement> getEcbData = () =>
                {
                    using (WebClient myWebClient = new WebClient()) 
                        return XElement.Parse(myWebClient.DownloadString(ECB_URL));
                };

            return await Task.Run(getEcbData);
        }
    }
}