namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Com_Empresa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Com_Empresa()
        {
            Ope_GradeOperacao = new HashSet<Ope_GradeOperacao>();
            GPS_Linha = new HashSet<GPS_Linha>();
            GPS_Linha1 = new HashSet<GPS_Linha>();
            GPS_Linha2 = new HashSet<GPS_Linha>();
            GPS_PontoReferencia = new HashSet<GPS_PontoReferencia>();
            GPS_PontoReferencia1 = new HashSet<GPS_PontoReferencia>();
            GPS_PontoReferencia2 = new HashSet<GPS_PontoReferencia>();
            GPS_Rota = new HashSet<GPS_Rota>();
            GPS_Rota1 = new HashSet<GPS_Rota>();
            GPS_Rota2 = new HashSet<GPS_Rota>();
            Ope_GradeOperacao1 = new HashSet<Ope_GradeOperacao>();
            Ope_GradeOperacao2 = new HashSet<Ope_GradeOperacao>();
            Ope_GradeOperacao3 = new HashSet<Ope_GradeOperacao>();
            Ope_GradeOperacaoSeccao = new HashSet<Ope_GradeOperacaoSeccao>();
            Ope_GradeOperacaoSeccao1 = new HashSet<Ope_GradeOperacaoSeccao>();
            Ope_GradeOperacaoSeccao2 = new HashSet<Ope_GradeOperacaoSeccao>();
            Tbl_Veiculo = new HashSet<Tbl_Veiculo>();
            Tbl_Veiculo1 = new HashSet<Tbl_Veiculo>();
            Tbl_Veiculo2 = new HashSet<Tbl_Veiculo>();
            Tbl_Veiculo3 = new HashSet<Tbl_Veiculo>();
            Com_Empresa1 = new HashSet<Com_Empresa>();
            Com_Empresa11 = new HashSet<Com_Empresa>();
        }

        public int ID { get; set; }

        public Guid IDAplicacao { get; set; }

        public int IDContratante { get; set; }

        public bool Ativo { get; set; }

        [Required]
        [StringLength(300)]
        public string Nome { get; set; }

        [StringLength(300)]
        public string NomeLower { get; set; }

        [StringLength(100)]
        public string Apelido { get; set; }

        [StringLength(100)]
        public string ApelidoLower { get; set; }

        [StringLength(20)]
        public string CnpjCpf { get; set; }

        [StringLength(20)]
        public string IM_RG { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Nascimento { get; set; }

        [StringLength(300)]
        public string Site { get; set; }

        [Column(TypeName = "ntext")]
        public string Observacao { get; set; }

        public bool PJ { get; set; }

        [StringLength(500)]
        public string Logo { get; set; }

        public int? IDPortalLogistico { get; set; }

        public int? IDImportacao { get; set; }

        public Guid? IDUser { get; set; }

        public int? IDRioCard { get; set; }

        public int IDUsuarioInclusao { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public int? IDUsuarioAlteracao { get; set; }

        [StringLength(254)]
        public string ChaveMaps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacao> Ope_GradeOperacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Linha> GPS_Linha { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Linha> GPS_Linha1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Linha> GPS_Linha2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_PontoReferencia> GPS_PontoReferencia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_PontoReferencia> GPS_PontoReferencia1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_PontoReferencia> GPS_PontoReferencia2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Rota> GPS_Rota { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Rota> GPS_Rota1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Rota> GPS_Rota2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacao> Ope_GradeOperacao1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacao> Ope_GradeOperacao2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacao> Ope_GradeOperacao3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacaoSeccao> Ope_GradeOperacaoSeccao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacaoSeccao> Ope_GradeOperacaoSeccao1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacaoSeccao> Ope_GradeOperacaoSeccao2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Veiculo> Tbl_Veiculo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Veiculo> Tbl_Veiculo1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Veiculo> Tbl_Veiculo2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Veiculo> Tbl_Veiculo3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Com_Empresa> Com_Empresa1 { get; set; }

        public virtual Com_Empresa Com_Empresa2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Com_Empresa> Com_Empresa11 { get; set; }

        public virtual Com_Empresa Com_Empresa3 { get; set; }
    }
}
