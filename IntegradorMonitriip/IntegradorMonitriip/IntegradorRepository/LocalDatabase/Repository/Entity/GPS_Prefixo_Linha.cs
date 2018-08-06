namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GPS_Prefixo_Linha
    {
        public int ID { get; set; }

        public short IDLinha { get; set; }

        public int? IDCliente { get; set; }

        [StringLength(50)]
        public string Prefixo { get; set; }

        public virtual GPS_Linha GPS_Linha { get; set; }
    }
}
