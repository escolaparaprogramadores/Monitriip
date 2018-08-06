namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Com_Empresa_Func
    {
        public int IDPJ { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public decimal? Comissao { get; set; }

        public int? IDGerente { get; set; }

        public decimal? Salario { get; set; }

        public DateTime? DataAdmissao { get; set; }

        public DateTime? DataDemissao { get; set; }

        [StringLength(50)]
        public string Matricula { get; set; }

        public int? IDSetor { get; set; }

        public int? IDPlanta { get; set; }

        public int? IdArquivoAssinatura { get; set; }

        public int? IDChaveiro { get; set; }

        public int? IdCargo { get; set; }

        public virtual Com_Empresa_PF Com_Empresa_PF { get; set; }

        public virtual Com_Empresa_PF Com_Empresa_PF1 { get; set; }
    }
}
