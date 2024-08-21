using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToXmlConverter.Services
{
    public interface IJsonToXml
    {
        void Convert(Stream input,Stream output);
    }
}
