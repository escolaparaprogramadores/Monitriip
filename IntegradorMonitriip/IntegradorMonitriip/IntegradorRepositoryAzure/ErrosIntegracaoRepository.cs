using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsGPS.Domain;
using IntegradorMonitriip.Model;

namespace IntegradorRepositoryAzure
{
    public class ErrosIntegracaoRepository : IntegradorRepositoryAzure.AzureTablesRepository<ErrosIntegracaoLog>
    {
        #region Construtores

        public ErrosIntegracaoRepository()
            : base("ErrosIntegracao")
        {

        }
        //"AnttLogErros"
        public ErrosIntegracaoRepository(string table)
            : base(table)
        {

        }

        #endregion Construtores
    }
}
