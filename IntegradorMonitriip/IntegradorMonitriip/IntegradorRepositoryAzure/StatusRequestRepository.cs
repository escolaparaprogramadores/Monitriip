using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsGPS.Domain;
using IntegradorMonitriip.Model;

namespace IntegradorRepositoryAzure
{
    public class StatusRequestRepository : IntegradorRepositoryAzure.AzureTablesRepository<StatusLog>
    {
        #region Construtores

        public StatusRequestRepository()
            : base("StatusRequest")
        {

        }
        //"AnttLogErros"

        public List<StatusLog> CheckStatusIntegracao()
        {
            var lista = new List<StatusLog>();
            try
            {
                var qry = this.GetQuery();

                var res = qry.ToList();

                var groups = res.GroupBy(x => x.IDCliente).ToList();

                foreach (var group in groups)
                {
                    var tpErroGroup = group.GroupBy(g => g.idTpErro).ToList();

                    foreach (var tp in tpErroGroup)
                    {
                        var lstErros = tp.OrderByDescending(x => x.DataHoraEvento).ToList();
                        var ultInt = lstErros.Where(x => x.erro == false).FirstOrDefault();
                        if (ultInt == null)
                            continue;

                        lista.Add(ultInt);

                        var ultErro = lstErros.Where(x => x.erro == true).FirstOrDefault();
                        if (ultErro == null)
                            continue;

                        lista.Add(ultErro);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lista;
        }

        public StatusRequestRepository(string table)
            : base(table)
        {

        }

        private static readonly object locker = new object();
        private static readonly object locker2 = new object();
        private DateTime dtRef;
        private DateTime dtRef1;

        public void saveLog(StatusLog log)
        {
            lock (locker)
            {
                try
                {
                    var db = this.GetQueryByPartitionKey(log.PartitionKey)
                                .Where(x => x.idTpErro == log.idTpErro
                                && x.erro == log.erro)
                                .FirstOrDefault();

                    if (db == null)
                    {
                        this.Add(log);
                    }
                    else
                    {
                        db.IDCliente = log.IDCliente;
                        db.Metodo = log.Metodo;
                        db.Modulo = log.Modulo;
                        db.idTpErro = log.idTpErro;
                        db.descricao = log.descricao;
                        db.stacktrace = log.stacktrace;
                        db.DataHoraEvento = DateTime.UtcNow;
                        db.InnerException = log.InnerException;
                        db.erro = false;
                        db.url = log.url;
                        this.Update(db);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void Add(ErrosIntegracaoLog log, bool status, string url)
        {
            var entity = new StatusLog(DateTime.UtcNow, log.idTpErro, log.IDCliente);
            entity.IDCliente = log.IDCliente;
            entity.Metodo = log.Metodo;
            entity.Modulo = log.Modulo;
            entity.idTpErro = log.idTpErro;
            entity.descricao = log.descricao;
            entity.stacktrace = log.stacktrace;
            entity.DataHoraEvento = DateTime.UtcNow;
            entity.InnerException = log.InnerException;
            entity.erro = status;
            entity.url = url;

            lock (locker2)
            {
                try
                {
                    var db = this.GetQueryByPartitionKey(log.PartitionKey)
                                .Where(x => x.idTpErro == log.idTpErro
                                && x.erro == status)
                                .FirstOrDefault();

                    if (db == null)
                    {
                        this.Add(entity);
                    }
                    else
                    {
                        db.IDCliente = log.IDCliente;
                        db.Metodo = log.Metodo;
                        db.Modulo = log.Modulo;
                        db.idTpErro = log.idTpErro;
                        db.descricao = log.descricao;
                        db.stacktrace = log.stacktrace;
                        db.DataHoraEvento = DateTime.UtcNow;
                        db.InnerException = log.InnerException;
                        db.erro = status;
                        db.url = url;
                        this.Update(db);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public StatusLog GetErroByIDCliente(int idCliente)
        {
            var existe = this.GetQuery().Where(x => x.IDCliente == idCliente && x.erro == true).FirstOrDefault();

            return existe;
        }

        #endregion Construtores
    }
}
