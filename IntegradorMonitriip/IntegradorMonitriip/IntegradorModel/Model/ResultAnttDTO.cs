using antt.gov.br.monitriip.v1._0;
using System;


namespace IntegradorMonitriip.Model
{
    public class ResultAnttDTO
    {
        public resultadoOperacao result { get; set; }

        public string rowKey { get; set; }
        public DateTime dataEnvioAntt { get; set; }
    }
}
