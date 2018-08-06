namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GPS_Rota
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GPS_Rota()
        {
            GPS_Linha_Rota = new HashSet<GPS_Linha_Rota>();
        }

        public int ID { get; set; }

        public int IDCliente { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(300)]
        public string Descricao { get; set; }

        [StringLength(3000)]
        public string Painel { get; set; }

        public int IDCor { get; set; }

        [StringLength(50)]
        public string Via { get; set; }

        public int IDPontoOrigem { get; set; }

        public int IDPontoDestino { get; set; }

        public int? IDImportacao { get; set; }

        public bool Ativo { get; set; }

        public int IDUsuarioInclusao { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public int? IDUsuarioAlteracao { get; set; }

        public virtual Com_Empresa Com_Empresa { get; set; }

        public virtual Com_Empresa Com_Empresa1 { get; set; }

        public virtual Com_Empresa Com_Empresa2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Linha_Rota> GPS_Linha_Rota { get; set; }

        public virtual GPS_PontoReferencia GPS_PontoReferencia { get; set; }

        public virtual GPS_PontoReferencia GPS_PontoReferencia1 { get; set; }
    }
}
