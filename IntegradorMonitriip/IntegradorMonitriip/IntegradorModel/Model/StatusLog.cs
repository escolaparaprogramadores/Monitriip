using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorMonitriip.Model
{
    public class StatusLog : TableEntity
    {
        public string url { get; set; }

        public bool erro { get; set; }

        public int idTpErro { get; set; }

        public int IDCliente { get; set; }
        public string descricao { get; set; }

        public string stacktrace { get; set; }
        public string Metodo { get; set; }

        public DateTime DataHoraEvento { get; set; }
        public string InnerException { get; set; }
        public string Modulo { get; set; }
        public int idGrade { get; set; }
        public string servico { get; set; }

        public StatusLog(DateTime dataEvento, int idTpErro, int idCliente)
        {
            this.DataHoraEvento = dataEvento;
            this.IDCliente = idCliente;

            this.PartitionKey = GetPartitionKey(dataEvento, idCliente);
            this.RowKey = GetRowKey(dataEvento);
        }

        public StatusLog()
        {

        }

        public static string GetPartitionKey(DateTime dataEvento, int idCliente)
        {
            string partitionPattern = "D{0}C{1}";

            var ret = String.Format(partitionPattern
                                    //, dataEvento.ToString("yyyyMMddHHmm")
                                    , dataEvento.ToString("yyyy")
                                    , idCliente.ToString()
                                    //, idTpErro.ToString()
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

        public object Where(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
}
