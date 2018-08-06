using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NewsGPS.Contracts.DTO.RJ
{
    public class ServicoDetalhadoXML
    {
        [System.Xml.Serialization.XmlElement("data")]
        public string data { get; set; }

        [System.Xml.Serialization.XmlElement("destino")]
        public string destino { get; set; }

        [System.Xml.Serialization.XmlElement("horarioSaida")]
        public string horarioSaida { get; set; }

        [System.Xml.Serialization.XmlElement("codOrigem")]
        public string CodOrigem { get; set; }

        [System.Xml.Serialization.XmlElement("codDestino")]
        public string CodDestino { get; set; }

        [System.Xml.Serialization.XmlElement("linha")]
        public string linha { get; set; }

        [System.Xml.Serialization.XmlElement("prefixoLinha")]
        public string prefixoLinha { get; set; }

        [System.Xml.Serialization.XmlElement("plataforma")]
        public string plataforma { get; set; }

        [System.Xml.Serialization.XmlElement("tipoViagem")]
        public string tipoViagem { get; set; }

        [System.Xml.Serialization.XmlElement("assentos")]
        public string assentos { get; set; }

        [System.Xml.Serialization.XmlElement("piso")]
        public string piso { get; set; }

        [System.Xml.Serialization.XmlElement("numServico")]
        public string numServico { get; set; }

        [System.Xml.Serialization.XmlElement("origem")]
        public string origem { get; set; }

        [System.Xml.Serialization.XmlElement("status")]
        public string status { get; set; }

        [System.Xml.Serialization.XmlElement("motorista")]
        public string motorista { get; set; }

        [System.Xml.Serialization.XmlElement("veiculo")]
        public string veiculo { get; set; }

        [XmlArray("servicosRelacionado")]
        [XmlArrayItem("servicoRelacionado", typeof(ServicosRelacionado))]
        public ServicosRelacionado[] ServicosRelacionado { get; set; }

        [XmlArray("seccoes")]
        [XmlArrayItem("seccao", typeof(seccaoXML))]
        public seccaoXML[] Seccao { get; set; }
    }

    public class seccaoXML
    {
        [System.Xml.Serialization.XmlElement("ordem")]
        public int ordem { get; set; }

        [System.Xml.Serialization.XmlElement("hora")]
        public string hora { get; set; }

        [System.Xml.Serialization.XmlElement("localidade")]
        public string localidade { get; set; }

        [System.Xml.Serialization.XmlElement("prefixo")]
        public string prefixo { get; set; }
    }

  
    public class ServicosRelacionado
    {
        [System.Xml.Serialization.XmlElement("linha")]
        public string linha { get; set; }

        [System.Xml.Serialization.XmlElement("numServico")]
        public string numServico { get; set; }

        [System.Xml.Serialization.XmlElement("origem")]
        public string origem { get; set; }

        [System.Xml.Serialization.XmlElement("destino")]
        public string destino { get; set; }

        [System.Xml.Serialization.XmlElement("codDestino")]
        public string codDestino { get; set; }

        [System.Xml.Serialization.XmlElement("codOrigem")]
        public string codOrigem { get; set; }

        [System.Xml.Serialization.XmlElement("prefixoLinha")]
        public string prefixoLinha { get; set; }

        [System.Xml.Serialization.XmlElement("piso")]
        public string piso { get; set; }

        [System.Xml.Serialization.XmlElement("assentos")]
        public string assentos { get; set; }
    }

  
    [System.Xml.Serialization.XmlRoot("servicosDetalhado2")]
    public class servicosDetalhado2
    {
        //[XmlArray("servicos")]
        //[XmlArrayItem("Servico", typeof(ServicoDetalhadoXML))]
        //public ServicoXML[] Servico { get; set; }
        [XmlElement("servicoDetalhado2")]
        public List<ServicoDetalhadoXML> servicos = new List<ServicoDetalhadoXML>();
    }

}
