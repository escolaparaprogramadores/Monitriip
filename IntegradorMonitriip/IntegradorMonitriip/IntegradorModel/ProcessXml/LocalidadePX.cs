using IntegradorModel.Model.XmlModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace IntegradorModel.ProcessXml
{
    public class LocalidadePX
    {
        public localidades TratarRetorno(XmlDocument Passagens)
        {
            try
            {
                var xmlReader = new XmlNodeReader(Passagens);

                localidades passagemRJ = null;

                XmlSerializer serializer = new XmlSerializer(typeof(localidades));
                passagemRJ = (localidades)serializer.Deserialize(xmlReader);

                return passagemRJ;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
