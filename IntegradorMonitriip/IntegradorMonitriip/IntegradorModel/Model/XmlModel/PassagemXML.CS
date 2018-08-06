using System.Collections.Generic;
using System.Xml.Serialization;

namespace IntegradorModel.Model.XmlModel
{

    public class vendas
    {
        [XmlElement("venda")]
        public List<PassagemXML> passagens = new List<PassagemXML>();
    }

    public class bilhetes
    {
        [XmlElement("bilhete")]
        public List<PassagemXML> passagens = new List<PassagemXML>();
    }

    public class PassagemXML
    {
        public string aliquotaICMS { get; set; }
        public string categoria { get; set; }
        public string celularPassageiro { get; set; }
        public string cnpj { get; set; }
        public string status { get; set; }
        public string cpfPassageiro { get; set; }
        public string dataEmissao { get; set; }
        public string dataVenda { get; set; }
        public string dataViagem { get; set; }
        public string destino { get; set; }
        //public string locDestino { get; set; }
        public string docPassageiro { get; set; }
        public string horaEmissao { get; set; }
        public string horaVenda { get; set; }
        public string horaViagem { get; set; }
        public string idLog { get; set; }
        public string identificadorBilhete { get; set; }
        public string linha { get; set; }
        public string motivoDesconto { get; set; }
        public string nomePassageiro { get; set; }
        public string numServico { get; set; }
        public string numBilheteEmbarque { get; set; }
        //public string numBilheteSistema { get; set; }
        public string numBilheteEstado { get; set; }
        public string numBilheteImpresso { get; set; }

        public string numSerie { get; set; }

        public string origem { get; set; }
        //public string locOrigem { get; set; }
        public string perDesconto { get; set; }

        public string plataformaEmbarque { get; set; }
        public string poltrona { get; set; }

        public string tarifa { get; set; }

        public string taxaEmbarque { get; set; }

        public string tipoServico { get; set; }

        public string tipoViagem { get; set; }

        public string valorPedagio { get; set; }

        public string valorTotal { get; set; }

        public string numeroNovoBilheteEmbarque { get; set; }

    }



}
