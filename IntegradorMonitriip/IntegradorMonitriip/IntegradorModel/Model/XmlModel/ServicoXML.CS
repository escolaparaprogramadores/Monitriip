using System.Collections.Generic;
using System.Xml.Serialization;

namespace NewsGPS.Contracts.DTO.RJ
{

    //[Serializable()]
    //[System.Xml.Serialization.XmlRoot("servicoes")]
    public class servicoes
    {     

        //[XmlArray("servicoes")]
        //[XmlArrayItem("servico", typeof(ServicoXML))]
        [XmlElement("servico")]
        public List<ServicoXML> servicos = new List<ServicoXML>();
    }

    public class ServicoXML
    {
        //[System.Xml.Serialization.XmlElement("data")]
        public string data { get; set; }

        //[System.Xml.Serialization.XmlElement("destino")]
        public string destino { get; set; }

       // [System.Xml.Serialization.XmlElement("horarioSaida")]
        public string horarioSaida { get; set; }

       // [System.Xml.Serialization.XmlElement("linha")]
        public string linha { get; set; }

       // [System.Xml.Serialization.XmlElement("numServico")]
        public string numServico { get; set; }

       // [System.Xml.Serialization.XmlElement("origem")]
        public string origem { get; set; }
        public string veiculo { get; set; }
        public string motorista { get; set; }

        public string dataChegadaPrevista { get; set; }

    }

   

}
