using NewsGPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using IntegradorModel.Model;

namespace IntegradorRepositoryAzure
{
    public class ViagensRepository : AzureTablesRepository<ViagemModel>
    {
        IQueryable<ViagemModel> qry;
        public ViagensRepository()
            : base("AnttLog")
        {
            qry = this.GetQuery(this.GetTableQuery());
        }

        public void UpdateLogViagem(ViagemModel viagem)
        {
            try
            {
                ViagemModel model = qry.Where(x => x.PartitionKey == viagem.PartitionKey && x.RowKey == viagem.RowKey).FirstOrDefault();

                model.idTransacao = viagem.idTransacao;

                if (viagem.isErro != null)
                    model.isErro = viagem.isErro;

                if (viagem.dataEnvioAntt != null)
                    model.dataEnvioAntt = viagem.dataEnvioAntt;

                if (viagem.Erros != null)
                    model.Erros = viagem.Erros;

                this.Update(model);
            }
            catch (Exception ex)
            {

            }
        }

        public List<ViagemModel> getLogs(string partitionKey)
        {
            return qry.Where(x => x.PartitionKey == partitionKey).ToList();
        }

        public ViagemModel getLogInicioViagem(string partitionKey)
        {
            return qry.Where(x => x.PartitionKey == partitionKey && x.codigoTipoLogID == 7 && x.codigoTipoRegistroViagem == 1 && x.idTransacao != null).FirstOrDefault();
        }

        public ViagemModel getUltimoLogFimViagem(string partitionKey)
        {

            try
            {
                var query = qry.Where(x => x.PartitionKey == partitionKey && x.codigoTipoLogID == 7 && x.codigoTipoRegistroViagem == 0).ToList();

                if (query.Count > 0)
                    return query.OrderByDescending(r => r.dataHoraEvento).FirstOrDefault();
                else
                    return query[0];
            }
            catch (Exception ex)
            {
                return new ViagemModel();
            }

        }

        public ViagemModel getPrimeiroLogFimViagem(string partitionKey)
        {

            try
            {
                var query = qry.Where(x => x.PartitionKey == partitionKey && x.codigoTipoLogID == 7 && x.codigoTipoRegistroViagem == 0).ToList();

                if (query.Count > 0)
                    return query.OrderBy(r => r.dataHoraEvento).FirstOrDefault();
                else
                    return query[0];
            }
            catch (Exception ex)
            {
                return new ViagemModel();
            }

        }

    }
}

