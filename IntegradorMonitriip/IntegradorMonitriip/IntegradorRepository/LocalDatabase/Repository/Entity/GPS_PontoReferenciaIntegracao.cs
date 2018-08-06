namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GPS_PontoReferenciaIntegracao
    {
        public int ID { get; set; }

        public int? IDCliente { get; set; }

        public int? IDPontoReferencia { get; set; }

        [StringLength(20)]
        public string CodIntegracao { get; set; }

        [StringLength(10)]
        public string CodOrgaoIntegracao { get; set; }

        public bool Ativo { get; set; }

        [StringLength(5)]
        public string Sigla { get; set; }

        public virtual GPS_PontoReferencia GPS_PontoReferencia { get; set; }
    }
}
