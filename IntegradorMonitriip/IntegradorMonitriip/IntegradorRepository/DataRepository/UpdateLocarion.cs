using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorModel.Model.XmlModel;
using IntegradorRepository.LocalDatabase.Repository.Entity;
using System.Data.Entity;
using System.Transactions;

namespace IntegradorRepository.DataRepository
{
    public class UpdateLocarion
    {
        private static readonly object locker = new object();
        public void update(localidades servicos, int iDCliente)
        {
            using (var context = new DatabaseContext())
            {
                var lst = servicos.lst;

                var lstCodigos = lst.Where(x => !string.IsNullOrEmpty(x.codigoAntt))
                    .Select(x => x.codigoAntt.Trim())
                    .ToList();

                //var lstLocalidades = context.RefIntegracao.Where(x => x.IDCliente == iDCliente
                //                                && lstCodigos.Any(y =>
                //                                y.Equals(x.CodIntegracao)))
                //                                .ToList();

                var lstLocalidades =
                    (from loc in context.RefIntegracao
                     where loc.IDCliente == iDCliente
                     //&& lstCodigos.Any(y => y.Equals(loc.CodIntegracao))
                     select loc).ToList();

                var size = lstLocalidades.Count();

                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        for (var i = 0; i < size; i++)
                        {
                            try
                            {
                                var codigo = lst.Where(l =>
                                                            l.codigoAntt.Equals(lstLocalidades[i].CodIntegracao.Trim()))
                                                            .FirstOrDefault();

                                lstLocalidades[i].Sigla = codigo.codigoLocalidade;
                                context.Entry(lstLocalidades[i]).State = EntityState.Modified;

                                if (i % 100 == 0)
                                    context.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        context.SaveChanges();

                        scope.Complete();
                        scope.Dispose();
                        context.Dispose();
                    }
                    catch (Exception ex)
                    {
                        scope.Dispose();
                        context.Dispose();
                    }
                }
            }
        }
    }
}
