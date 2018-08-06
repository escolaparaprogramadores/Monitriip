namespace NewsGPS.Logic.Integracao.RJ
{
    public abstract class ImportadorRJ : IntegradorRJ
    {
        protected ImportadorRJ(int idCliente)
            : base(idCliente)
        {

        }

        //public static ImportadorRJ Create(EEntidade entidade, EEntidade entidadeExterna, int idCliente)
        //{
        //    ImportadorRJ ret = null;
        //    switch (entidadeExterna)
        //    {
        //        case EEntidade.RJ_Servico:
        //            ret = ImportadorServico.Create(idCliente);
        //            break;
        //        case EEntidade.RJ_Passagem:
        //            ret = ImportadorPassagem.Create(idCliente, true, "");
        //            break;
        //        //case EEntidade.RJ_Sac:
        //        //    ret = ImportadorSac.Create(idCliente);
        //        //    break;

        //        default:
        //            string msgErro = String.Format("Não existe integrador para a especificação. Sistema: RJ, Tipo: Importação, Entidade: {0}", entidade.ToString());
        //            throw new Exception(msgErro);
        //    }

        //    return ret;
        //}
    }

    //public abstract class ImportadorRJ<TDTO, TEntityExterna> : ImportadorRJ
    //    where TDTO : class, IEntityDTO
    //    where TEntityExterna : class, IEntityDTO
    //{
    //    #region Propriedades

    //    protected abstract override Contracts.Enums.EEntidade Entidade { get; }

    //    protected override Contracts.Enums.ESistemaRastreamento Sistema
    //    {
    //        get { return Contracts.Enums.ESistemaRastreamento.RJ; }
    //    }

    //    #endregion Propriedades

    //    #region Construtores


    //    protected ImportadorRJ(int idCliente)
    //        : base(idCliente)
    //    {

    //    }

    //    #endregion Construtores

        
    //    int idInterno;
      
    //}
}
