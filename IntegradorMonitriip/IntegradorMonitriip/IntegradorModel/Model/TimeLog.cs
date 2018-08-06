using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsGPS.Domain
{
    public class ErrosGrades : TableEntity
    {
        public int IDCliente { get; set; }
        public string descricao { get; set; }

        public string stacktrace { get; set; }

        public DateTime DataHoraEvento { get; set; }

        public ErrosGrades(DateTime dataEvento, string Metodo, long tempoDecorrido, int idCliente, string cnpj)
        {
            this.DataHoraEvento = dataEvento;
            this.IDCliente = idCliente;

            this.PartitionKey = GetPartitionKey(dataEvento, idCliente);
            this.RowKey = GetRowKey(dataEvento);
        }

        public ErrosGrades()
        {

        }

        public static string GetPartitionKey(DateTime dataEvento, int idCliente)
        {
            string partitionPattern = "D{0}C{1}";

            var ret = String.Format(partitionPattern
                                    , dataEvento.ToString("yyyyMMddHHmm")
                                    , idCliente.ToString()
                                     );

            return ret;
        }

        public static string GetRowKey(DateTime dataEvento)
        {
            string rowPattern = "D{0}";
            var ret = String.Format(rowPattern
                    , DateTime.MaxValue.Subtract(dataEvento).Ticks.ToString().PadLeft(20, '0')
                    );
            return ret;
        }

    }
}
