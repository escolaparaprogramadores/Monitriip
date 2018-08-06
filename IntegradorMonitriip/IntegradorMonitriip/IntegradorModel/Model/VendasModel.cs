using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorModel.Model
{
    public class VendasModel : TableEntity
    {
        public static string GetPartitionKey(DateTime dataViagem, string SRVP, int idCliente)
        {
            string partitionPattern = "{0}C{1}K{2}";

            var ret = String.Format(partitionPattern
                                    , dataViagem.ToString("yyyyMMdd")
                                    , SRVP
                                    , idCliente.ToString().PadLeft(10, '0')
                                     );

            return ret;
        }

        public static string GetRowKey(string numBilheteSistema, string numSerie,
            DateTime dataViagem, string SRVP, int idCliente)
        {
                      
           // string param1 = DateTime.MaxValue.Subtract(DateTime.UtcNow).Ticks.ToString().PadLeft(20, '0');
            string param2 = numSerie.PadLeft(6, '0');
            string param3 = dataViagem.ToString("yyyyMMdd");
            string param4 = SRVP;
            string param5 = idCliente.ToString().PadLeft(8, '0');
            string param6 = string.IsNullOrEmpty(numBilheteSistema) ? DateTime.MaxValue.Subtract(DateTime.UtcNow).Ticks.ToString().PadLeft(20, '0') : numBilheteSistema;
            //string rowPattern = "T{0}NS{1}D{2}S{3}C{4}B{5}";
            string rowPattern = "NS{0}D{1}S{2}C{3}B{4}";
            var ret = String.Format(rowPattern
                    //, param1/*.Substring(param1.Length - 6)*/
                    , param2/*.Substring(param2.Length - 6)*/
                    , param3/*.Substring(param2.Length - 6)*/
                    , param4/*.Substring(param2.Length - 6)*/
                    , param5/*.Substring(param2.Length - 6)*/
                    , param6
                    );
            return ret;
        }
        public static string GetRowKey(string numBilheteSistema, string numSerie)
        {
            //numBilheteEmbarque, numSerie
            string param1 = numBilheteSistema.PadLeft(6, '0');
            string param2 = numSerie.PadLeft(6, '0');
            string rowPattern = "B{0}S{1}";
            var ret = String.Format(rowPattern
                    , param1.Substring(param1.Length - 6)
                    , param2.Substring(param2.Length - 6)
                    );
            return ret;
        }

        public VendasModel(int idCliente, string numBilheteEmbarque, string numSerie, string linha,
            DateTime dataViagem )
        {
            this.PartitionKey = VendasModel.GetPartitionKey(dataViagem, linha, idCliente);
            this.RowKey = VendasModel.GetRowKey(numBilheteEmbarque, numSerie);
        }

        public VendasModel(string numBilheteSistema, string numSerie, string SRVP,
            DateTime dataViagem, int idCliente)
        {
            this.dataHoraViagem = dataViagem;
            this.idCliente = idCliente;

            this.PartitionKey = VendasModel.GetPartitionKey(dataViagem, SRVP, idCliente);
            this.RowKey = VendasModel.GetRowKey(numBilheteSistema, numSerie, dataViagem, SRVP, idCliente);
        }

        public VendasModel()
        {
        }

        public DateTime dataHoraViagem { get; set; }
        public int idCliente { get; set; }
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
        public string locDestino { get; set; }
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

        public string numBilheteEstado { get; set; }
        public string numBilheteEmbarque { get; set; }
        public string numBilheteSistema { get; set; }
        public string numBilheteImpresso { get; set; }

        public string numSerie { get; set; }

        public string origem { get; set; }
        public string locOrigem { get; set; }
        public string perDesconto { get; set; }

        public string poltrona { get; set; }

        public string tarifa { get; set; }
        public string plataformaEmbarque { get; set; }
        public string taxaEmbarque { get; set; }

        public string tipoServico { get; set; }

        public string tipoViagem { get; set; }

        public string valorPedagio { get; set; }

        public string valorTotal { get; set; }
        public string retornoANTT { get; set; }

        public bool isVendas { get; set; }
        public DateTime? dataEnvioAntt { get; set; }
        public DateTime? checkin { get; set; }

        public string numeroNovoBilheteEmbarque { get; set; }
    }
}
