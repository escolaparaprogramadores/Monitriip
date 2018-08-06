using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsGPS.Domain;

namespace IntegradorRepositoryAzure
{
    public class EmbarqueRepository : IntegradorRepositoryAzure.AzureTablesRepository<Embarque>
    {
        #region Construtores

        public EmbarqueRepository()
            : base("EmbarqueLog")
        {

        }
        //"AnttLogErros"
        public EmbarqueRepository(string table)
            : base(table)
        {

        }

        #endregion Construtores
    }
}
