namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ope_Eventos
    {
        public int ID { get; set; }

        public int? Codigo { get; set; }

        [Column("Descricao do Evento")]
        [StringLength(100)]
        public string Descricao_do_Evento { get; set; }
    }
}
