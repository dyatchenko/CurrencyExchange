namespace Realeyes.WebUI.Models
{
    using System;
    using System.IO;
    using System.Net;
    using System.Web;
    using System.Xml.Linq;
    using Realeyes.WebUI.Abstract;

    public class EcbDataSource : IEcbDataSource
    {
        private const string ECB_URL = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";

        public XElement GetEcbExchangeRatesXml()
        {
            XElement result = null;

            using (WebClient myWebClient = new WebClient())
                result = XElement.Parse(myWebClient.DownloadString(ECB_URL));

            return result;
        }
    }
}