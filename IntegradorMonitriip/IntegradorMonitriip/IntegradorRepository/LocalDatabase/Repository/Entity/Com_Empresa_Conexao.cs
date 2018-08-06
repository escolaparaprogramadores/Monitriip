namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Com_Empresa_Conexao
    {
        public int ID { get; set; }

        public int IDEmpresa { get; set; }

        public int Grupo { get; set; }

        public int IDMarcaEquipamento { get; set; }

        public int PortaOrigem { get; set; }

        public bool isConfig { get; set; }

        public bool isUDP { get; set; }

        [StringLength(15)]
        public string IPFila { get; set; }

        [Required]
        [StringLength(50)]
        public string NomeFila { get; set; }

        public bool Trace { get; set; }

        public bool is10seg { get; set; }
    }
}
