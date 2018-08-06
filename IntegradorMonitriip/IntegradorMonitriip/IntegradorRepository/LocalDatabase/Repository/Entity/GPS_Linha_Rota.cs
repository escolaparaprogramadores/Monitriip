namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GPS_Linha_Rota
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GPS_Linha_Rota()
        {
            GPS_Linha_Ponto = new HashSet<GPS_Linha_Ponto>();
            Ope_GradeOperacaoOnibus = new HashSet<Ope_GradeOperacaoOnibus>();
        }

        public int ID { get; set; }

        public short IDLinha { get; set; }

        public int IDRota { get; set; }

        public int IDTipoRota { get; set; }

        public bool isServico { get; set; }

        public int? TempoViagem { get; set; }

        public int? Preponderancia { get; set; }

        public bool Ativo { get; set; }

        public virtual GPS_Linha GPS_Linha { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Linha_Ponto> GPS_Linha_Ponto { get; set; }

        public virtual GPS_Rota GPS_Rota { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacaoOnibus> Ope_GradeOperacaoOnibus { get; set; }
    }
}
