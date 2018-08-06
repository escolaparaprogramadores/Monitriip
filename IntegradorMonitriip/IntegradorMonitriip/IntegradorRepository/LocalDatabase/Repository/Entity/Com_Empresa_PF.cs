namespace IntegradorRepository.LocalDatabase.Repository.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Com_Empresa_PF
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Com_Empresa_PF()
        {
            Com_Empresa_Func = new HashSet<Com_Empresa_Func>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(20)]
        public string OrgaoExpedidor { get; set; }

        public DateTime? IdentidadeDataEmissao { get; set; }

        public int Sexo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Com_Empresa_Func> Com_Empresa_Func { get; set; }

        public virtual Com_Empresa_Func Com_Empresa_Func1 { get; set; }
    }
}
