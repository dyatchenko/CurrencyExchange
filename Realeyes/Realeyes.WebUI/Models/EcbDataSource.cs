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
            const string FileName = "ecb-xml.tmp";

            XElement result = null;

            try
            {
                using (WebClient myWebClient = new WebClient())
                    myWebClient.DownloadFile(ECB_URL, FileName);

                result = XElement.Parse(File.ReadAllText(FileName));
            }
            finally { File.Delete(FileName); }

            return result;
        }
    }
}