using IntegradorRepository.LocalDatabase;
using System;
using System.Linq;

namespace IntegradorMonitriip.DataRepository
{
    public class GetInfoGrade : IDisposable
    {
        DatabaseContext db;

        public GetInfoGrade()
        {
            db = new DatabaseContext();
        }
        public void GetInfoLinha(string linha, ref GPS_Linha Linha, int IDOrigem, int IDDestino)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    Linha = db.Linha.
                        Where(r => r.IDPontoOrigem == IDOrigem
                        && r.IDPontoDestino == IDDestino
                        && r.Numero.Equals(linha)).
                        //Select(r => r.ID).
                        FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
            }
        }

        public void GetInfoPontoReferencia(string origem, string destino, int IDCliente, ref int IDOrigem, ref int IDDestino)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var ori = Convert.ToInt32(origem);
                    var des = Convert.ToInt32(destino);

                    IDOrigem = (int)db.RefIntegracao.
                        Where(r => r.CodIntegracao.ToLower().Trim().Equals(origem.Trim().ToLower())).
                        Select(r => r.IDPontoReferencia).
                        FirstOrDefault();

                    IDDestino = (int)db.RefIntegracao.
                        Where(r => r.CodIntegracao.ToLower().Trim().Equals(destino.Trim().ToLower())).
                        Select(r => r.IDPontoReferencia).
                        FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void GetInfoRota(int IDOrigem, int IDDestino, int IDCliente)
        {

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
        // ~GetInfoGrade() {
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
