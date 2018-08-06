using NewsGPS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using IntegradorModel.Model;

namespace IntegradorRepositoryAzure
{
    public class VendasRepository : AzureTablesRepository<VendasModel>
    {
        IQueryable<VendasModel> qry;
        public VendasRepository()
            : base("VendasIntegradas")
        {
            qry = this.GetQuery(this.GetTableQuery());
        }

        public void deleteLogs()
        {
            var lista = qry.ToList();
            var pkGroup = lista.GroupBy(g => g.PartitionKey).ToList();
            var pks = lista.Select(l => l.PartitionKey).ToList();
            var vendas = lista.Where(g => g.isVendas == true).ToList();
            var bilhetes = lista.Where(g => g.isVendas == false).ToList();
        }

        public void saveBilhetes(List<VendasModel> bilhetes)
        {
            try
            {

                var groups = bilhetes.GroupBy(x => x.PartitionKey).ToList();

                foreach (var group in groups)
                {
                    this.MultiplesAdd(group.ToList());
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void saveVendas(List<VendasModel> vendas)
        {
            try
            {
                var groups = vendas.GroupBy(x => x.PartitionKey).ToList();
                var lista = new List<VendasModel>();

                foreach( var group in groups)
                {

                    /*Faz o merge se existir, senão adiciona - Claudio Marcio 10/02/2018*/
                    this.MultiplesMerge(group.ToList(), ref lista);   
                    
                       
                    var list = new List<VendasModel>();
                    list.AddRange(group.ToList());                    
                    list = list.Except(lista).ToList();

                    if(list.Count > 0)
                    {
                        this.MultiplesAdd(list);
                        lista = new List<VendasModel>();
                    }else if(lista.Count > 0)
                    {
                        this.MultiplesAdd(lista);
                        lista = new List<VendasModel>();
                    }
                        
                    /*Old - Claudio Marcio - 10/02/2018*/ 
                    //this.MultiplesAdd(group.ToList());
                }
            }
            catch (Exception ex)
            {
            }
        }

        //        #region IDisposable Support
        //        private bool disposedValue = false; // To detect redundant calls
        //        protected virtual void Dispose(bool disposing)
        //        {
        //            if (!disposedValue)
        //            {
        //                if (disposing)
        //                {
        //                    qry.d
        //                    //Repository.Dispose();
        //                    //GC.SuppressFinalize(Repository);
        //                    // TODO: dispose managed state (managed objects).
        //                }

        //                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        //                // TODO: set large fields to null.
        //                qry = null;

        //                disposedValue = true;
        //            }
        //        }

        //        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        //        // ~PutVendas() {
        //        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //        //   Dispose(false);
        //        // }

        //        // This code added to correctly implement the disposable pattern.
        //        public void Dispose()
        //        {
        //            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //            Dispose(true);
        //            // TODO: uncomment the following line if the finalizer is overridden above.
        //            GC.SuppressFinalize(this);
        //        }
        //#endregion
    }
}
