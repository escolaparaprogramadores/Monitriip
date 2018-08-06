using antt.gov.br.monitriip.v1._0;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorModel.Model
{
    public class ViagemModel : TableEntity
    {
        public static string GetPartitionKey(DateTime dataViagem, int idCliente, string Chave)
        {
            string partitionPattern = "{0}C{1}K{2}";

            var ret = String.Format(partitionPattern
                                    , dataViagem.ToString("yyyyMMddHHmmss")
                                    , idCliente.ToString().PadLeft(10, '0')
                                   , Chave
                                     );

            return ret;
        }

        public static string GetRowKey(DateTime dataEvento, String PlacaVeiculo, int CodigoTipoLogID)
        {
            string rowPattern = "{0}V{1}T{2}";
            var ret = String.Format(rowPattern
                    , DateTime.MaxValue.Subtract(dataEvento).Ticks.ToString().PadLeft(20, '0')
                    , PlacaVeiculo.ToString().PadLeft(10, '0')
                    , CodigoTipoLogID.ToString().PadLeft(2, '0')

                    );
            return ret;
        }

        public ViagemModel(DateTime dataEvento, int idCliente, string PlacaVeiculo, DateTime dataViagem, string IdentificacaoLinha, int CodigoSentidoLinha, string AutorizacaoViagem, int CodigoTipoLogID)
        {
            this.dataHoraViagem = dataViagem;
            this.dataHoraEvento = dataEvento;
            this.IDCliente = idCliente;
            this.placaVeiculo = PlacaVeiculo;
            this.codigoTipoLogID = CodigoTipoLogID;
            this.autorizacaoViagem = AutorizacaoViagem;
            this.identificacaoLinha = IdentificacaoLinha;
            this.codigoSentidoLinha = CodigoSentidoLinha;
            this.Chave = (string.IsNullOrEmpty(AutorizacaoViagem) == true ? string.Format("L{0}S{1}", IdentificacaoLinha, CodigoSentidoLinha) : AutorizacaoViagem);
            //this.Chave = (string.IsNullOrEmpty(IdentificacaoLinha) ==true ? AutorizacaoViagem : string.Format("L{0}S{1}", IdentificacaoLinha, CodigoSentidoLinha));

            this.PartitionKey = GetPartitionKey(dataViagem, idCliente, Chave);
            this.RowKey = GetRowKey(dataEvento, PlacaVeiculo, CodigoTipoLogID);
        }

        public ViagemModel()
        {

        }

        public int Id;
        public string Chave { get; set; }
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
        public string comentario { get; set; }
        public string tipoComentario { get; set; }
        #region resultadoOperacao
        public Guid? idTransacao { get; set; }
        public bool? isErro { get; set; }
        public DateTime? dataEnvioAntt { get; set; }
        public string Erros { get; set; }
        public int? codigoRetorno { get; set; }
        public string mensagem { get; set; }
        #endregion resultadoOperacao

        #region jornada
        public string cpfMotorista { get; set; }

        public int? codigoTipoRegistroEvento { get; set; } //jornada
        #endregion jornada

        #region parada
        public int? codigoMotivoParada { get; set; } //parada
        #endregion parada

        #region inicioFimViagemRegular
        public string identificacaoLinha { get; set; }

        [NotMapped]
        public string dataProgramada { get; set; }

        [NotMapped]
        public string horaProgramada { get; set; }

        public int? codigoTipoRegistroViagem { get; set; }
        public int? codigoSentidoLinha { get; set; }
        #endregion inicioFimViagemRegular

        #region leitorBilheteEmbarque

        public string bilhetes { get; set; }
        public string jsonEnviado { get; set; }

        #endregion leitorBilheteEmbarque

        #region leitorCartaoEmbarque
        public string cartoes { get; set; }
        #endregion leitorCartaoEmbarque

        #region velocidadeTempoLocalizacao
        public int? velocidadeAtual { get; set; }
        public int? distanciaPercorrida { get; set; }
        public int? codigoSituacaoIgnicaoMotor { get; set; }
        public int? codigoSituacaoPortaVeiculo { get; set; }
        #endregion velocidadeTempoLocalizacao

        #region inicioFimViagemFretado
        public string autorizacaoViagem { get; set; }
        #endregion inicioFimViagemFretado

        #region VendaDePassagem

        public string CodigoMotivoDesconto { get; set; }
        public string CodigoCategoriaTransporte { get; set; }
        public int? IdPontoDestinoViagem { get; set; }
        public int? IdPontoOrigemViagem { get; set; }
        public string InformacoesPassageiro { get; set; }
        public List<bilhete> NumeroBilheteEmbarque { get; set; }
        public string NumeroPoltrona { get; set; }
        public string NumeroSerieEquipamentoFiscal { get; set; }
        public string PercentualDesconto { get; set; }
        public string PlataformaEmbarque { get; set; }
        public string ValorPedagio { get; set; }
        public string ValorTarifa { get; set; }
        public string ValorTaxaEmbarque { get; set; }
        public string ValorTotal { get; set; }
        public string AliquotaICMS { get; set; }

        #endregion

        #region cartaoEmitidoRecargaEfetuada
        public string DataVenda { get; set; }
        public string HoraVenda { get; set; }
        public string BonusRecarga { get; set; }
        public string CodigoTipoCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string SaldoTotalCartao { get; set; }
        public string ValorTotalRecarga { get; set; }
        #endregion

        #region avaliacao viagem
        public string tempoViagem { get; set; }
        public string tempoDescanso { get; set; }
        public string KmPercorrido { get; set; }
        public string TotalParada { get; set; }
        public string TotalJustificativas { get; set; }

        //[NotMapped]
        //public string PartitionKey { get; set; }
        #endregion

        
        public int id_gradeoperacao { get; set; }

        [NotMapped]
        public bool isTransbordo { get; set; }

        public int? IdMotorista { get; set; }


        [NotMapped]
        public string tipoViagem { get; set; }

    }
}

