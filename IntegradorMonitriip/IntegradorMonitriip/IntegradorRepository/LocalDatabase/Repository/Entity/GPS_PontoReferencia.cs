namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GPS_PontoReferencia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GPS_PontoReferencia()
        {
            GPS_Linha = new HashSet<GPS_Linha>();
            GPS_Linha1 = new HashSet<GPS_Linha>();
            GPS_Linha_Ponto = new HashSet<GPS_Linha_Ponto>();
            GPS_PontoReferenciaIntegracao = new HashSet<GPS_PontoReferenciaIntegracao>();
            GPS_Rota = new HashSet<GPS_Rota>();
            GPS_Rota1 = new HashSet<GPS_Rota>();
            Ope_GradeOperacaoSeccao = new HashSet<Ope_GradeOperacaoSeccao>();
            Ope_GradeOperacaoSeccao1 = new HashSet<Ope_GradeOperacaoSeccao>();
        }

        public int ID { get; set; }

        public int? IDEmpresa { get; set; }

        public double Raio { get; set; }

        public bool? isPontoPainel { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string Descricao { get; set; }

        [StringLength(500)]
        public string Endereco { get; set; }

        [Required]
        public DbGeography GEO { get; set; }

        public bool? isParada { get; set; }

        public int IDUsuarioInclusao { get; set; }

        public DateTime DataInclusao { get; set; }

        public int? IDTipoPonto { get; set; }

        public bool? isPublico { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public int? IDUsuarioAlteracao { get; set; }

        public bool Ativo { get; set; }

        public int IDCor { get; set; }

        public int IDTipoGeo { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DbGeography PontoCentral { get; set; }

        public bool? isVelocidade { get; set; }

        public bool? isAlertaSaida { get; set; }

        public bool? isGaragem { get; set; }

        public bool? isAlertaEntrada { get; set; }

        public double? VelocidadeMaxima { get; set; }

        public double? VelocidadeMinima { get; set; }

        public TimeSpan? PeriodoAtivoInicio { get; set; }

        public TimeSpan? PeriodoAtivoFim { get; set; }

        public int Preponderancia { get; set; }

        public bool? isControle { get; set; }

        public bool? isAntt { get; set; }

        public virtual Com_Empresa Com_Empresa { get; set; }

        public virtual Com_Empresa Com_Empresa1 { get; set; }

        public virtual Com_Empresa Com_Empresa2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Linha> GPS_Linha { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Linha> GPS_Linha1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Linha_Ponto> GPS_Linha_Ponto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_PontoReferenciaIntegracao> GPS_PontoReferenciaIntegracao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Rota> GPS_Rota { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Rota> GPS_Rota1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacaoSeccao> Ope_GradeOperacaoSeccao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacaoSeccao> Ope_GradeOperacaoSeccao1 { get; set; }
    }
}
