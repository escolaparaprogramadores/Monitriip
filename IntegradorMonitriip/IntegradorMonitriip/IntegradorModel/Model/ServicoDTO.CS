using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NewsGPS.Contracts.DTO.RJ
{
    [DataContract]
    public class ServicoDTO : EntityDTOBase
    {

        [DataMember]
        public string Data { get; set; }

        [DataMember]
        public string Destino { get; set; }

        [DataMember]
        public string Origem { get; set; }

        [DataMember]
        public string HoraSaida { get; set; }

        [DataMember]
        public string Linha { get; set; }

        [DataMember]
        public string NumServico { get; set; }

        [DataMember]
        public string CodOrigem { get; set; }

        [DataMember]
        public string CodDestino { get; set; }

        [DataMember]
        public string Veiculo { get; set; }

        [DataMember]
        public string Motorista { get; set; }

        [DataMember]
        public string DataChegadaPrevista { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string StatusErro { get; set; }

        [DataMember]
        public string prefixoLinha { get; set; }

        [DataMember]
        public string plataforma { get; set; }

        [DataMember]
        public string tipoViagem { get; set; }

        [DataMember]
        public string assentos { get; set; }

        [DataMember]
        public string piso { get; set; }

        [DataMember]
        public List<seccaoXML> seccoes { get; set; }

        [DataMember]
        public List<ServicosRelacionado> ServicosRelacionado { get; set; }
    }

    [DataContract]
    public class ServicoDetalhadoDTO : EntityDTOBase
    {

        [DataMember]
        public string Data { get; set; }

        [DataMember]
        public string Destino { get; set; }

        [DataMember]
        public string Origem { get; set; }

        [DataMember]
        public string HoraSaida { get; set; }

        [DataMember]
        public string Linha { get; set; }

        [DataMember]
        public string NumServico { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public List<SeccaoDTO> Seccoes { get; set; }

    }

    [DataContract]
    public class SeccaoDTO : EntityDTOBase
    {

        [DataMember]
        public int Ordem { get; set; }

        [DataMember]
        public string Hora { get; set; }

        [DataMember]
        public string Localidade { get; set; }

        [DataMember]
        public string Prefixo { get; set; }


    }

    [DataContract]
    public class ServicosRelacionadoDTO: EntityDTOBase
    {
        [DataMember]
        public int linha { get; set; }

        [DataMember]
        public string numServico { get; set; }

        [DataMember]
        public string origem { get; set; }

        [DataMember]
        public string destino { get; set; }

        [DataMember]
        public string codDestino { get; set; }

        [DataMember]
        public string codOrigem { get; set; }

        [DataMember]
        public string prefixoLinha { get; set; }

        [DataMember]
        public string piso { get; set; }

        [DataMember]
        public string assentos { get; set; }
    }
}
