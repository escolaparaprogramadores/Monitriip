namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ope_GradeOperacaoOnibus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ope_GradeOperacaoOnibus()
        {
            Ope_GradeOperacaoSeccao1 = new HashSet<Ope_GradeOperacaoSeccao>();
            Ope_GradeOperacaoSeccao2 = new HashSet<Ope_GradeOperacaoSeccao>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int IdLinhaRota { get; set; }

        public int Ordem { get; set; }

        public int IDDiaSemana { get; set; }

        public int IDTipoJornada { get; set; }

        [StringLength(100)]
        public string CodigoSRVP { get; set; }

        public int? IDViagem { get; set; }

        public int IDTurno { get; set; }

        public int? IDSeccaoAtual { get; set; }

        [StringLength(50)]
        public string CodFretamento { get; set; }

        public virtual GPS_Linha_Rota GPS_Linha_Rota { get; set; }

        public virtual Ope_GradeOperacao Ope_GradeOperacao { get; set; }

        public virtual Ope_GradeOperacaoSeccao Ope_GradeOperacaoSeccao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacaoSeccao> Ope_GradeOperacaoSeccao1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacaoSeccao> Ope_GradeOperacaoSeccao2 { get; set; }

        [NotMapped]
        public virtual List<ServicosRelacionados> ListaServicosRelacionados { get; set; }
    }
}
