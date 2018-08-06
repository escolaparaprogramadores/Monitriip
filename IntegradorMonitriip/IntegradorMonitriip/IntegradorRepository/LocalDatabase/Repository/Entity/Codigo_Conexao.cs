namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Codigo_Conexao
    {
        public int ID { get; set; }

        public int IDCliente { get; set; }

        [Required]
        [StringLength(25)]
        public string Codigo1 { get; set; }

        [Required]
        [StringLength(25)]
        public string Codigo2 { get; set; }

        public int? Porta { get; set; }
    }
}
