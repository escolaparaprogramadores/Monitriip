using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    public partial class Logs_ServicosRelacionados
    {
        public Int64 ID { get; set; }
        public long ID_ServicoRelacionado { get; set; }        
        public int IDCliente { get; set; }
        public string IMEI { get; set; }
        public int codigoTipoLogID { get; set; }
        public string placaVeiculo { get; set; }
        public string cnpjEmpresa { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public decimal pdop { get; set; }
        public DateTime dataHoraEvento { get; set; }
        public DateTime dataHoraViagem { get; set; }
              
        #region resultadoOperacao
        public Guid? idTransacao { get; set; }
        public bool? isErro { get; set; }
        public DateTime? dataEnvioAntt { get; set; }
        public string Erros { get; set; }
        public int? codigoRetorno { get; set; }
        public string mensagem { get; set; }
        #endregion resultadoOperacao

        #region inicioFimViagemRegular
        public string identificacaoLinha { get; set; }
        public string dataProgramada { get; set; }
        public string horaProgramada { get; set; }
        public int? codigoTipoRegistroViagem { get; set; }
        public int? codigoSentidoLinha { get; set; }
        #endregion inicioFimViagemRegular       

        #region inicioFimViagemFretado
        public string autorizacaoViagem { get; set; }
        #endregion inicioFimViagemFretado

        public string tipoViagem { get; set; }

        public virtual ServicosRelacionados ServicosRelacionados { get; set; } 

    }
}
