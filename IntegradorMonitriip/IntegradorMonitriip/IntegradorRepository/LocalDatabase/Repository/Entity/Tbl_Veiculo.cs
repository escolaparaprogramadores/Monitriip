namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Veiculo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Veiculo()
        {
            Ope_GradeOperacao = new HashSet<Ope_GradeOperacao>();
            Ope_GradeOperacaoSeccao = new HashSet<Ope_GradeOperacaoSeccao>();
        }

        public int ID { get; set; }

        public int IDContratante { get; set; }

        [Required]
        [StringLength(10)]
        public string Identificacao { get; set; }

        [Required]
        [StringLength(50)]
        public string Descricao { get; set; }

        public int IDCliente { get; set; }

        [StringLength(7)]
        public string Placa { get; set; }

        public int? IDTipoVeiculo { get; set; }

        public int? IDModeloVeiculo { get; set; }

        public int? AnoFabr { get; set; }

        public int? AnoModelo { get; set; }

        [StringLength(20)]
        public string Chassi { get; set; }

        public int? IDCombustivel { get; set; }

        public bool Blindado { get; set; }

        [StringLength(2)]
        public string Bateria { get; set; }

        public int IDStatus { get; set; }

        public int? IDPortalLogistico { get; set; }

        public bool Ativo { get; set; }

        public int IDTipoOperacao { get; set; }

        public int? IDContrato { get; set; }

        public int? PortaUDP { get; set; }

        [StringLength(15)]
        public string IPServer { get; set; }

        public int? IDChamadoGPSInst { get; set; }

        public int? IDChamadoGPSProx { get; set; }

        public int? IDChamadoGPSUlt { get; set; }

        public DateTime? DataProxManut { get; set; }

        public int? IDManutProx { get; set; }

        public int? IDManutUlt { get; set; }

        public bool isRestrito { get; set; }

        public int? IDLinhaAutomatica { get; set; }

        public decimal? RPMFATOR { get; set; }

        [StringLength(10)]
        public string IDRioCard { get; set; }

        public int? VelocidadeMax { get; set; }

        public int? QtdLugaresSentados { get; set; }

        public int? QtdLugaresEmPe { get; set; }

        public int? IdCarroceria { get; set; }

        public int IDUsuarioInclusao { get; set; }

        public DateTime DataInclusao { get; set; }

        public int? IDUsuarioAlteracao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public bool Trace { get; set; }

        public int QtdLugaresLeito { get; set; }

        public int QtdLugaresSemiLeito { get; set; }

        public bool TemCobrador { get; set; }

        public bool TemRoleta { get; set; }

        public long? ContadorRoleta { get; set; }

        public bool TemValidador { get; set; }

        public int? QtdEixos { get; set; }

        public decimal? TensaoLigado { get; set; }

        [StringLength(20)]
        public string Rfid { get; set; }

        public virtual Com_Empresa Com_Empresa { get; set; }

        public virtual Com_Empresa Com_Empresa1 { get; set; }

        public virtual Com_Empresa Com_Empresa2 { get; set; }

        public virtual Com_Empresa Com_Empresa3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacao> Ope_GradeOperacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_GradeOperacaoSeccao> Ope_GradeOperacaoSeccao { get; set; }
    }
}
