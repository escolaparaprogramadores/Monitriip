namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ope_GradeOperacao
    {
        public int ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataReferencia { get; set; }

        public int IDTipoGrade { get; set; }

        public int IDCliente { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataOrigem { get; set; }

        public int? IDGradeOrigem { get; set; }

        public short ToleranciaAnterior { get; set; }

        public short ToleranciaPosterior { get; set; }

        public DateTimeOffset? DataPartidaPrevista { get; set; }

        public DateTimeOffset? DataPartidaReal { get; set; }

        public DateTimeOffset? DataChegadaPrevista { get; set; }

        public DateTimeOffset? DataChegadaReal { get; set; }

        public DateTimeOffset? DataCriacao { get; set; }

        public int IDUsuarioCriacao { get; set; }

        public bool isCancelado { get; set; }

        public int? IDMotivo { get; set; }

        public int? IDUsuarioCancelou { get; set; }

        [Column(TypeName = "ntext")]
        public string Observacao { get; set; }

        public DateTimeOffset? DataCancelamento { get; set; }

        public double? PercPercorrida { get; set; }

        public int? VariacaoAtual { get; set; }

        public int? IDVeiculo { get; set; }

        public int? IDContratante { get; set; }

        [StringLength(10)]
        public string AutorizacaoANTT { get; set; }
        public bool IsAssociado { get; set; }

        public string IMEI { get; set; }

        public bool IsAberto { get; set; }

        public bool IsFechado { get; set; }

        public bool IsTransbordoAberto { get; set; }

        public bool IsTransbordoFechado { get; set; }

        public string PartitionKey { get; set; }

        public string plataforma { get; set; }

        public string tipoViagem { get; set; }

        public string assentos { get; set; }

        public string piso { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? DuracaoViagemPrevista { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? DuracaoViagemReal { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? DataPartidaTolerancia { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? DataChegadaTolerancia { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? AtrasoPartida { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? AtrasoChegada { get; set; }

        //public bool IsAssociado { get; set; }

        //[StringLength(20)]
        //public string IMEI { get; set; }

        public virtual Com_Empresa Com_Empresa { get; set; }

        public virtual Com_Empresa Com_Empresa1 { get; set; }

        public virtual Com_Empresa Com_Empresa2 { get; set; }

        public virtual Com_Empresa Com_Empresa3 { get; set; }

        public virtual Tbl_Veiculo Tbl_Veiculo { get; set; }

        public virtual Ope_GradeOperacaoOnibus Ope_GradeOperacaoOnibus { get; set; }

        public virtual ICollection<ServicosRelacionados> ServicosRelacionados { get; set; }
    }
}
