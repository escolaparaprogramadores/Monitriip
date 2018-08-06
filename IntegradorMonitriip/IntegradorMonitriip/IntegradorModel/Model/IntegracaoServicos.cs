using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorModel.Model
{
    public class IntegracaoServicos : TableEntity
    {
        public static string GetPartitionKey(DateTime dataViagem, int idCliente)
        {
            string partitionPattern = "{0}IS{1}";

            var ret = String.Format(partitionPattern,
                                    dataViagem.ToString("yyyyMMdd")
                                    , idCliente.ToString().PadLeft(10, '0')
                                     );

            return ret;
        }

        public static string GetRowKey(DateTime dataViagem, string SRVP, int idCliente)
        {

            string param1 = dataViagem.ToString("yyyyMMdd");
            string param2 = SRVP;
            string param3 = idCliente.ToString().PadLeft(8, '0');
            string rowPattern = "IS{0}D{1}S{2}";
            var ret = String.Format(rowPattern
                    , param1
                    , param2
                    , param3
                    );
            return ret;
        }


        public IntegracaoServicos(int idCliente, string SRVP,
            DateTime dataViagem)
        {
            this.PartitionKey = IntegracaoServicos.GetPartitionKey(dataViagem, idCliente);
            this.RowKey = IntegracaoServicos.GetRowKey(dataViagem, SRVP, idCliente);
        }


        public IntegracaoServicos()
        {
        }


        public int IDCliente { get; set; }
        public string Data { get; set; }

        public string DestinoRJ { get; set; }

        public string OrigemRJ { get; set; }
        public string HoraSaida { get; set; }

        public string LinhaRJ { get; set; }

        public string NumServico { get; set; }

        public string Veiculo { get; set; }
        public string Motorista { get; set; }

        public bool Status { get; set; }

        public string StatusErro { get; set; }

        public string DestinoQuadri { get; set; }

        public string OrigemQuadri { get; set; }

    }
}

