using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realeyes.WebUI.Abstract
{
    using System.Xml.Linq;

    public interface IEcbDataSource
    {
        XElement GetEcbExchangeRatesXml();
    }
}
