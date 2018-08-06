using IntegradorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorRepositoryAzure
{
    public class IntegracaoServicosRepository : AzureTablesRepository<IntegracaoServicos>
    {
        IQueryable<IntegracaoServicos> qry;
        public IntegracaoServicosRepository()
            : base("IntrgracaoServicos")
        {
            qry = this.GetQuery(this.GetTableQuery());
        }

        //public void SalvarServicosIntegrados(List<VendasModel> bilhetes)
        //{
        //    try
        //    {

        //        var groups = bilhetes.GroupBy(x => x.PartitionKey).ToList();

        //        foreach (var group in groups)
        //        {
        //            this.MultiplesAdd(group.ToList());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        public void SalvarServicosIntegrados(List<IntegracaoServicos> servicos)
        {
            try
            {

                var lista = new List<IntegracaoServicos>();
                IntegracaoServicosRepository rep = new IntegracaoServicosRepository();
                rep.MergeIntegracaoServico(servicos, ref lista);

                if (lista.Count > 0)
                {
                    this.MultiplesAddServices(lista);
                    lista = new List<IntegracaoServicos>();
                }

            }
            catch (Exception ex)
            {
            }
        }

        public bool MergeIntegracaoServico(List<IntegracaoServicos> entities, ref List<IntegracaoServicos> servicos)
        {
            try
            {
                foreach (var item in entities)
                {
                    if (!string.IsNullOrEmpty(item.PartitionKey) && !string.IsNullOrEmpty(item.RowKey))
                    {
                        IntegracaoServicos modelExiste = this.GetQuery().Where(x => x.PartitionKey == item.PartitionKey && x.RowKey == item.RowKey).FirstOrDefault();

                        if (modelExiste != null)
                        {
                            try
                            {
                                modelExiste.Status = item.Status;
                                modelExiste.StatusErro = item.StatusErro;
                                modelExiste.LinhaRJ = item.LinhaRJ;
                                modelExiste.OrigemRJ = item.OrigemRJ;
                                modelExiste.DestinoRJ = item.DestinoRJ;
                                modelExiste.Motorista = item.Motorista;
                                modelExiste.Veiculo = item.Veiculo;
                                modelExiste.HoraSaida = item.HoraSaida;

                                this.Update(modelExiste);

                            }
                            catch (Exception ex)
                            {

                            }

                        }
                        else
                            servicos.Add(item);
                    }
                }

                return true;
            }
            catch (System.Exception ex)
            {
                return true;
            }
        }

    }
}
