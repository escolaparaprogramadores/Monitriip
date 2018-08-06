namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Com_Empresa_PJ
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? IDMatriz { get; set; }

        [StringLength(20)]
        public string IE { get; set; }

        [StringLength(250)]
        public string RamoAtividade { get; set; }

        [StringLength(250)]
        public string Roteiro { get; set; }

        [StringLength(250)]
        public string Suframa { get; set; }
    }
}
