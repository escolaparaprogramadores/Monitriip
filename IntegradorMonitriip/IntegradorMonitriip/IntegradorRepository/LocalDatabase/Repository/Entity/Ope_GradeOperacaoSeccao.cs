namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ope_GradeOperacaoSeccao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ope_GradeOperacaoSeccao()
        {
            Ope_GradeOperacaoOnibus = new HashSet<Ope_GradeOperacaoOnibus>();
        }

        public int ID { get; set; }

        public int IDGradeOperacao { get; set; }

        public int IDPontoOrigem { get; set; }

        public int IDPontoDestino { get; set; }

        public int? IDVeiculo { get; set; }

        public DateTimeOffset? DataPartidaPrevista { get; set; }

        public DateTimeOffset? DataPartidaReal { get; set; }

        public DateTimeOffset? DataChegadaPrevista { get; set; }

        public DateTimeOffset? DataChegadaReal { get; set; }

        public int? IDMotorista { get; set; }

        public int? IDCobrador { get; set; }

        public bool isCancelado { get; set; }

        public int? IDMotivoCancelado { get; set; }

        public DateTimeOffset? DataCriacao { get; set; }

        public int IDUsuarioCriacao { get; set; }

        public DbGeography GEO { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? DuracaoTrechoPrevisto { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? DuracaoTrechoReal { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? AtrasoPartida { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? AtrasoChegada { get; set; }

        public virtual Com_Empresa Com_Empresa { get; set; }

        public virtual Com_Empresa Com_Empresa1 { get; set; }

        public virtual Com_Empresa Com_Empresa2 { get; set; }

        public virtual GPS_PontoReferencia GPS_PontoReferencia { get; set; }

        public virtual GPS_PontoReferencia GPS_PontoReferencia1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacaoOnibus> Ope_GradeOperacaoOnibus { get; set; }

        public virtual Ope_GradeOperacaoOnibus Ope_GradeOperacaoOnibus1 { get; set; }

        public virtual Ope_GradeOperacaoOnibus Ope_GradeOperacaoOnibus2 { get; set; }
       
        public virtual Tbl_Veiculo Tbl_Veiculo { get; set; }

        [NotMapped]
        public string partitionKey { get; set; }
    }
}
