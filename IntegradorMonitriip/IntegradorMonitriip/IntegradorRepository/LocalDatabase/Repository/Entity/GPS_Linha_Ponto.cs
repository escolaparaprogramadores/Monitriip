namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GPS_Linha_Ponto
    {
        public int ID { get; set; }

        public int IDLinhaRota { get; set; }

        public int IDPontoReferencia { get; set; }

        public DateTime TempoParada { get; set; }

        public bool IsDescanso { get; set; }

        public virtual GPS_Linha_Rota GPS_Linha_Rota { get; set; }

        public virtual GPS_PontoReferencia GPS_PontoReferencia { get; set; }
    }
}
