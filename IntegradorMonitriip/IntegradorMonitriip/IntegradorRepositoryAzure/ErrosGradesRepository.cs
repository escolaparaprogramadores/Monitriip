using NewsGPS.Domain;

namespace IntegradorRepositoryAzure
{
    public class ErrosGradesRepository : IntegradorRepositoryAzure.AzureTablesRepository<ErrosGrades>
    {
        #region Construtores

        public ErrosGradesRepository()
            : base("ErrosGradesLog")
        {

        }
        //"AnttLogErros"
        public ErrosGradesRepository(string table)
            : base(table)
        {

        }

        public void saveLog(ErrosGrades entity)
        {
            try
            {
                this.Add(entity);
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion Construtores
    }
}
