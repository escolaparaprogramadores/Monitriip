using IntegradorRepository.LocalDatabase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorMonitriip.DataRepository
{
    public class ValidaGrade : IDisposable
    {
        private DatabaseContext db;

        public ValidaGrade()
        {
            db = new DatabaseContext();
        }

        public bool hasGrade(string srvp, string data)
        {
            try
            {
                data = "20" + data.Substring(0, 2) + "-"
                        + data.Substring(2, 2) + "-" + data.Substring(4);
                var _data = Convert.ToDateTime(data);
                var hasSRVP = db.GradeOperacaoOnibus.Count(x => x.CodigoSRVP.ToLower().Contains(srvp.ToLower())
                  && DbFunctions.TruncateTime(x.Ope_GradeOperacao.DataPartidaTolerancia) == DbFunctions.TruncateTime(_data));

                return hasSRVP > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                db = null;
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ValidaGrade() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
