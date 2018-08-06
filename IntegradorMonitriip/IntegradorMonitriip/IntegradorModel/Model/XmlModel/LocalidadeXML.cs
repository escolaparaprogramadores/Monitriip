using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegradorModel.Model.XmlModel
{
    public class localidades
    {
        [XmlElement("localidade")]
        public List<LocalidadeXML> lst = new List<LocalidadeXML>();
    }
    public class LocalidadeXML
    {
        public string codigoAntt { get; set; }
        public string codigoEstado { get; set; }
        public string codigoLocalidade { get; set; }
        public string descEstado { get; set; }
        public string descLocalidade { get; set; }
        public int idLocalidade { get; set; }

    }
}
