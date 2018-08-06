using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorRepository.LocalDatabase.Repository.Entity
{ 
   public partial class ServicosRelacionados
    {
        public long ID { get; set; }

        public int idGradeOperacao { get; set; }

        public string linha { get; set; }

        public string numServico { get; set; }

        public string origem { get; set; }

        public string destino { get; set; }

        public string codDestino { get; set; }

        public string codOrigem { get; set; }

        public string prefixoLinha { get; set; }

        public string piso { get; set; }

        public string assentos { get; set; }

        public virtual Ope_GradeOperacao GradeOperacao { get; set; }
    }
}
