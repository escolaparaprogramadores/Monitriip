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
    public class VendasPX
    {
        public vendas TratarRetorno(XmlDocument Passagens)
        {
            try
            {

                //XmlRootAttribute xRoot = new XmlRootAttribute();
                //xRoot.ElementName = "vendas";                
                //xRoot.IsNullable = true;

                var xmlReader = new XmlNodeReader(Passagens);

                vendas passagemRJ = null;
                
                XmlSerializer serializer = new XmlSerializer(typeof(vendas));
                passagemRJ = (vendas)serializer.Deserialize(xmlReader);

                return passagemRJ;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public vendas TratarRetornoGM(XmlDocument Passagens)
        {
            try
            {

                //XmlRootAttribute xRoot = new XmlRootAttribute();
                //xRoot.ElementName = "vendas";                
                //xRoot.IsNullable = true;

                var xmlReader = new XmlNodeReader(Passagens);
                Passagens.InnerXml = Passagens.InnerXml.Replace("<bilhetes>", "<vendas>").Replace("</bilhetes>", "</vendas>").
                                                        Replace("<bilhete>", "<venda>").Replace("</bilhete>", "</venda>").
                                                        Replace("<bilhetes />", "<vendas />");



                vendas passagemRJ = null;

                XmlSerializer serializer = new XmlSerializer(typeof(vendas));
                passagemRJ = (vendas)serializer.Deserialize(xmlReader);

                return passagemRJ;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public vendas TratarRetornoItamarati(XmlDocument Passagens)
        {
            try
            {

                //XmlRootAttribute xRoot = new XmlRootAttribute();
                //xRoot.ElementName = "vendas";                
                //xRoot.IsNullable = true;

                var xmlReader = new XmlNodeReader(Passagens);
                Passagens.InnerXml = Passagens.InnerXml.Replace("<numeroPoltrona>", "<poltrona>")
                                  .Replace("</numeroPoltrona>", "</poltrona>")
                                  .Replace("<codigoTipoViagem>", "<tipoViagem>")
                                  .Replace("</codigoTipoViagem>", "</tipoViagem>")
                                  .Replace("<codigoMotivoDesconto>", "<motivoDesconto>")
                                  .Replace("</codigoMotivoDesconto>", "</motivoDesconto>");

                vendas passagemRJ = null;

                XmlSerializer serializer = new XmlSerializer(typeof(vendas));
                passagemRJ = (vendas)serializer.Deserialize(xmlReader);

                return passagemRJ;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
