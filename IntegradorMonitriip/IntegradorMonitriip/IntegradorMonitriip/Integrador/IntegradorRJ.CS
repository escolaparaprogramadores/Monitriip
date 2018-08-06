using NewsGPS.Contracts.Enums;

namespace NewsGPS.Logic.Integracao.RJ
{
    public abstract class IntegradorRJ : Integrador
    {
        protected abstract override EEntidade Entidade {get; }
        protected abstract override EEntidade EntidadeExterna { get; }

        protected override Contracts.Enums.ESistemaRastreamento Sistema
        {
            get { return ESistemaRastreamento.AppOnibus; }
        }

        #region Construtores

        protected IntegradorRJ(int idCliente) : base(idCliente)
        {

        }


        public static IntegradorRJ Create(ETipoIntegracao tipoIntegracao, EEntidade entidade, EEntidade entidadeExterna, int idCliente)
        {
            switch (tipoIntegracao)
            {
                //case ETipoIntegracao.Importacao:
                //    return ImportadorRJ.Create(entidade, entidadeExterna, idCliente);
                case ETipoIntegracao.Exportacao:
                    //return ExportadorRJ.Create(entidade, idCliente);
                default:
                    return null;
            }
        }

        #endregion Construtores

        public override abstract void Processar();
        
    }
}
