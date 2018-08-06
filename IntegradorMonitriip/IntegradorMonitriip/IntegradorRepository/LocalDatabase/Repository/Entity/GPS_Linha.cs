namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GPS_Linha
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GPS_Linha()
        {
            GPS_Linha_Rota = new HashSet<GPS_Linha_Rota>();
            GPS_Prefixo_Linha = new HashSet<GPS_Prefixo_Linha>();
        }

        public short ID { get; set; }

        [StringLength(10)]
        public string Numero { get; set; }

        [Required]
        [StringLength(150)]
        public string Nome { get; set; }

        [StringLength(150)]
        public string NomePainel { get; set; }

        [StringLength(350)]
        public string Descricao { get; set; }

        public int? IDCliente { get; set; }

        public short Intervalo { get; set; }

        public byte NumTurnos { get; set; }

        public bool Ativo { get; set; }

        public int IDIcone { get; set; }

        public short ToleranciaAnterior { get; set; }

        public short ToleranciaPosterior { get; set; }

        public int IDPontoOrigem { get; set; }

        public int IDPontoDestino { get; set; }

        public int? IDImportacao { get; set; }

        public int IDUsuarioInclusao { get; set; }

        public DateTime DataInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public int? IDUsuarioAlteracao { get; set; }

        [StringLength(10)]
        public string AutorizacaoANTT { get; set; }

        [StringLength(50)]
        public string CodigoANTT { get; set; }

        public virtual Com_Empresa Com_Empresa { get; set; }

        public virtual Com_Empresa Com_Empresa1 { get; set; }

        public virtual Com_Empresa Com_Empresa2 { get; set; }

        public virtual GPS_PontoReferencia GPS_PontoReferencia { get; set; }

        public virtual GPS_PontoReferencia GPS_PontoReferencia1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Linha_Rota> GPS_Linha_Rota { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GPS_Prefixo_Linha> GPS_Prefixo_Linha { get; set; }
    }
}
