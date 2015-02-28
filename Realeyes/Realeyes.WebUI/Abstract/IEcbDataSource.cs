namespace Realeyes.WebUI.Abstract
{
    using System.Xml.Linq;
    using System.Threading.Tasks;

    public interface IEcbDataSource
    {
        Task<XElement> GetEcbExchangeRatesXml();
    }
}
