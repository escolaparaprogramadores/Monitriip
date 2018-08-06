using NewsGPS.Common.Constantes;
using NewsGPS.Common.Service.CustomHeader;
using NewsGPS.Contracts.Enums;
using NewsGPS.Domain.Entities.Suporte;


using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using NewsGPS.Common.IoC;
using Castle.MicroKernel.Lifestyle;

namespace NewsGPS.Logic.Integracao
{
    public abstract class IntegradorBase
    {
        #region Controle

        private static object _logLockObj = new object();
        private static object _traceLockObj = new object();

        protected int _IntervaloProcessamento;

        /// <summary>
        /// Intervalo do tick do processamento em milisegundos...
        /// </summary>
        //protected int IntervaloProcessamento
        //{
        //    get
        //    {

        //        if (_IntervaloProcessamento == 0)
        //        {
        //            int segIntervaloConfig = int.Parse(ConfigurationManager.AppSettings["IntervaloPadraoIntegradores"]);

        //            _IntervaloProcessamento = segIntervaloConfig * 1000;
        //        }

        //        return _IntervaloProcessamento;
        //    }
        //    set
        //    {
        //        _IntervaloProcessamento = value;
        //    }
        //}


        protected System.Timers.Timer _Timer;
        protected bool _IsRunning { get; set; }
        protected bool _IsBusy { get; set; }
        #endregion Controle

        #region Propriedades

        protected int IDCliente { get; set; }

        /// <summary>
        /// Data e hora da última integração
        /// </summary>
        public DateTime? DataUltimaIntegracao { get; set; }
        public DateTime? DataUltimaIntegracaoFuncionou { get; set; }

        public int IntervaloProcessamento { get; set; }

        public bool trace { get; set; }


        public int IDIntegracao { get; set; }

        private Pessoa _Cliente { get; set; }

        //protected Pessoa Cliente
        //{
        //    get
        //    {
        //        if (this._Cliente == null)
        //        {
        //            this._Cliente = EFContext.GetDBSetAdmin<Pessoa>().Where(x => x.ID == this.IDCliente).FirstOrDefault();
        //        }
        //        return this._Cliente;
        //    }
        //}


        private string DiretorioLogErros
        {
            get
            {
                return "";// ConfigurationManager.AppSettings["DiretorioErros"];
            }
        }

        private string DiretorioLogTrace
        {
            get
            {
                return "";// ConfigurationManager.AppSettings["DiretorioTrace"];
            }
        }

        public EventLog EventLogger { get; set; }

        #endregion Propriedades

        #region Construtores

        protected IntegradorBase(int idCliente)
        {
            this.IDCliente = idCliente;
        }

        #endregion Construtores

        public void Iniciar()
        {
            if (this._IsRunning)
                return;

            this._IsRunning = true;

            this._Timer = new System.Timers.Timer();

            this._Timer.Elapsed += TimerTick;
            this._Timer.AutoReset = true;

            //Não pode ser disparado da thread principal pois trava a inicialização do serviço enfileirando tudo.
            //Pra resolver a inicialização imediata, setamos o intervalo do timer apenas após o primeiro tick, que roda a 100ms, interval padrão.
            //ProcessarTick(); 

            this._Timer.Start();

        }

        public void Parar()
        {
            if (!this._IsRunning)
                return;

            this._Timer.Stop();

            this._IsRunning = false;
        }

        private object lockObj = new object();

        private void TimerTick(object sender, ElapsedEventArgs e)
        {
            lock (lockObj)
            {
                this._Timer.Interval = IntervaloProcessamento;
                try
                {
                    if (this._IsBusy)
                        return;

                    this._Timer.Enabled = false;

                    this._IsBusy = true;

                    ClientContext.SetUserInfo(Usuarios.UsuarioSistema, this.IDCliente);
                    using (var sc = IoCContainer.Windsor.BeginScope())
                    {
                        this.Processar();
                    }

                    this._IsBusy = false;
                }
                catch (Exception ex)
                {
                    NewsGPS.Logic.BusinessRules.Log.PublicarErro(ex);
                    this._IsBusy = false;
                }
                finally
                {
                    this._Timer.Enabled = true;
                }
            }
        }

        protected void LogarErro(string mensagem)
        {
            lock (_logLockObj)
            {
                if (DiretorioLogErros == null)
                    return;

                try
                {
                    string fileName = String.Format("{0}_{1}_Erro.txt"
                    , DateTime.Today.ToString("yyyyMMdd")
                    , this.GetType().Name);

                    if (!Directory.Exists(DiretorioLogErros))
                        Directory.CreateDirectory(DiretorioLogErros);

                    fileName = Path.Combine(DiretorioLogErros, fileName);

                    var msg = String.Format("{0} - {1}" + Environment.NewLine, DateTime.UtcNow.ToString("HH:mm:ss"), mensagem ?? "");

                    File.AppendAllText(fileName, msg);
                }
                catch
                {
                }

            }
        }

        protected void LogarErro(Exception ex, string mensagem = null)
        {
            lock (_logLockObj)
            {
                if (DiretorioLogErros == null)
                    return;

                try
                {
                    string fileName = String.Format("{0}_{1}_Erro.txt"
                    , DateTime.Today.ToString("yyyyMMdd")
                    , this.GetType().Name);

                    if (!Directory.Exists(DiretorioLogErros))
                        Directory.CreateDirectory(DiretorioLogErros);

                    fileName = Path.Combine(DiretorioLogErros, fileName);

                    var msg = String.Format("{0} - {1}" + Environment.NewLine, DateTime.UtcNow.ToString("HH:mm:ss"), mensagem ?? "");
                    msg = msg + ex.Message + Environment.NewLine + Environment.NewLine;
                    File.AppendAllText(fileName, msg);
                }
                catch
                {
                }

            }
        }

        protected void LogarInfo(string mensagem)
        {
            lock (_traceLockObj)
            {
                if (DiretorioLogTrace == null)
                    return;

                try
                {
                    string fileName = String.Format("{0}_{1}_Info.txt"
                    , DateTime.Today.ToString("yyyyMMdd")
                    , this.GetType().Name);

                    if (!Directory.Exists(DiretorioLogErros))
                        Directory.CreateDirectory(DiretorioLogErros);

                    fileName = Path.Combine(DiretorioLogErros, fileName);

                    var msg = String.Format("{0} - {1}" + Environment.NewLine, DateTime.UtcNow.ToString("HH:mm:ss"), mensagem ?? "");

                    File.AppendAllText(fileName, msg);
                }
                catch
                {
                }

            }
        }

        public abstract void ProcessarUnitario(object item);
        public abstract void Processar();

       
    }

    public abstract class Integrador : IntegradorBase
    {

        #region Propriedades

        /// <summary>
        /// Entidade a ser Integrada. Ex: 
        /// Cliente
        /// Carroceria
        /// Veiculo
        /// PontoReferencia
        /// Motorista
        /// Rota / Trecho
        /// Linha
        /// GradeOperacao 
        /// </summary>
        protected abstract EEntidade Entidade { get; }

        protected abstract EEntidade EntidadeExterna { get; }

        /// <summary>
        /// Sistema a ser integrado
        /// </summary>
        protected abstract ESistemaRastreamento Sistema { get; }

        /// <summary>
        /// Importação ou Exportação
        /// </summary>
        protected ETipoIntegracao TipoIntegracao { get; set; }

        #endregion Propriedades

        #region Construtores

        protected Integrador(int idCliente)
            : base(idCliente)
        {

        }

        #endregion Construtores

        #region Factory

        public static Integrador Create(ESistemaRastreamento sistema, EEntidade entidade, EEntidade entidadeExterna, ETipoIntegracao tipoIntegracao, int idCliente, bool trace)
        {
            switch (sistema)
            {
                //case ESistemaRastreamento.Fetranspor_VaDeOnibus:
                //    return ExportadorFetranspor.Create(idCliente, trace);
                //case ESistemaRastreamento.GlobalBus:
                //    return IntegradorGlobalBus.Create(tipoIntegracao, entidade, entidadeExterna, idCliente,trace);
                case ESistemaRastreamento.GlobalSearch:
                //case ESistemaRastreamento.NewsGPS:
                //    return IntegradorSuporte.Create(tipoIntegracao, entidade, idCliente,trace);
                //case ESistemaRastreamento.Pirelli:
                //    return IntegradorPirelli.Create(entidade, idCliente);
                //case ESistemaRastreamento.AppOnibus:
                //    return NewsGPS.Logic.Integracao.AppOnibus.ExportadorAppOnibus.Create(entidade, entidadeExterna, idCliente, trace);
                //case ESistemaRastreamento.RJ:
                //    return NewsGPS.Logic.Integracao.RJ.IntegradorRJ.Create(tipoIntegracao, entidade, entidadeExterna, idCliente, trace);
                //case ESistemaRastreamento.Sigla:
                //    return NewsGPS.Logic.Integracao.Sigla.IntegradorSigla.Create(tipoIntegracao, entidade, entidadeExterna, idCliente   );
                case ESistemaRastreamento.Optz:
                    return null;
                default:
                    return null;
            }
        }

        #endregion Factory

        //public static Domain.Entities.Suporte.Integracao ObterDeParaPorIdExterno(int? idExterno, ESistemaRastreamento sistema, EEntidade entidade, EEntidade entidadeExterna, int? idCliente)
        //{
        //    string idAlvo = idExterno.HasValue ? idExterno.ToString() : null;

        //    var ret = EFContext.GetDBSetAdmin<Domain.Entities.Suporte.Integracao>()
        //               .Where(x => x.IDSistemaAlvo == (int)sistema
        //                   && x.IDEntidade == (int)entidade
        //                   && x.IDEntidadeExterna == (int)entidadeExterna
        //                   && x.IDAlvo == idAlvo
        //                   && x.IDCliente == idCliente)
        //                   .FirstOrDefault();

        //    return ret;
        //}

        //internal Domain.Entities.Suporte.Integracao ObterDeParaPorIdExterno(int? idExterno, int? idCliente)
        //{
        //    var ret = ObterDeParaPorIdExterno(idExterno, this.Sistema, this.Entidade, this.EntidadeExterna, idCliente);
        //    return ret;
        //}

        //public static Domain.Entities.Suporte.Integracao ObterDeParaPorIdInterno(int? idInterno, ESistemaRastreamento sistemaAlvo, EEntidade entidade, EEntidade entidadeExterna, int? idCliente)
        //{
        //    string _idInterno = idInterno.HasValue ? idInterno.ToString() : null;

        //    var ret = EFContext.GetDBSetAdmin<Domain.Entities.Suporte.Integracao>()
        //                .Where(x => x.IDSistemaAlvo == (int)sistemaAlvo
        //                    && x.IDEntidade == (int)entidade
        //                    && x.IDEntidadeExterna == (int)entidadeExterna
        //                    && x.IDBase == _idInterno
        //                    && (idCliente == null ? 1==1 : x.IDCliente == idCliente))
        //                    .FirstOrDefault();

        //    return ret;
        //}

        //protected List<int> ObterIdsEntidadesIntegradas()
        //{
        //    var dbSetDePara = EFContext.GetDBSetAdmin<Domain.Entities.Suporte.Integracao>();

        //    var ret = dbSetDePara.Where(x => x.IDEntidade == (int)this.Entidade
        //                         && x.IDEntidadeExterna == (int)this.EntidadeExterna)
        //                         .Select(x => x.IDBase)
        //                         .ToList() //vai no banco!
        //                         .Select(x => int.Parse(x))
        //                         .ToList();

        //    return ret;
        //}

        //internal Domain.Entities.Suporte.Integracao ObterDeParaPorIdInterno(int? idInterno, int? idCliente)
        //{
        //    var ret = ObterDeParaPorIdInterno(idInterno, this.Sistema, this.Entidade, this.EntidadeExterna, idCliente);
        //    return ret;
        //}

        //private Domain.Entities.Suporte.Integracao CreateDePara()
        //{
        //    var dbSetIntegracao = EFContext.GetDBSetAdmin<Domain.Entities.Suporte.Integracao>();
        //    var dePara = dbSetIntegracao.Create();

        //    dePara.DataImportacao = DateTime.UtcNow;
        //    dePara.IDSistemaBase = (int)ESistemaRastreamento.NewsGPS; //TODO: Deve carregar valor da tabela de config. de integração... Por enquanto.. o sistema base é o suporte!
        //    dePara.IDSistemaAlvo = (int)this.Sistema;
        //    dePara.IDEntidade = (int)this.Entidade;
        //    dePara.IDEntidadeExterna = (int)this.EntidadeExterna;
        //    dePara.IDCliente = this.IDCliente;
        //    return dePara;
        //}

        ///// <summary>
        ///// Inclui ou Atualiza o de-para... 
        ///// Atualiza a data de ultima integração na instancia do integrador...
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="idExterno"></param>
        //protected void SalvarDePara(int id, bool importacao, int? idExterno = null)
        //{
        //    string idExternoString = idExterno == null ? null : idExterno.ToString();

        //    SalvarDePara(id, importacao, idExternoString);
        //}

        //protected void SalvarDePara(int id, bool importacao, string idExterno = null)
        //{
        //    using (var tranAzure = new TransactionScope(importacao ? TransactionScopeOption.Required : TransactionScopeOption.Suppress))
        //    //using (var tranAzure = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        //Cria ou atualiza o de-para...
        //        var dePara = this.ObterDeParaPorIdInterno(id, this.IDCliente);
        //        if (dePara == null)
        //        {
        //            dePara = this.CreateDePara();

        //            dePara.IDBase = id.ToString();
        //            dePara.IDAlvo = id.ToString();
        //            dePara.IDCliente = this.IDCliente;
        //            //Adiciona no contexto...
        //            EFContext.GetDBSetAdmin<Domain.Entities.Suporte.Integracao>().Add(dePara);
        //        }

        //        if (idExterno != null)
        //        {
        //            dePara.IDAlvo = idExterno;
        //        }

        //        if (this.Sistema == ESistemaRastreamento.NewsGPS)

        //            this.DataUltimaIntegracao = DateTime.UtcNow;

        //        if (importacao)
        //            dePara.DataImportacao = DateTime.UtcNow;
        //        else
        //            dePara.DataExportacao = DateTime.UtcNow;

        //        try
        //        {
        //            EFContext.GetContextAdmin().SaveChanges();
        //            tranAzure.Complete();

        //        }
        //        catch (DbEntityValidationException dbEx)
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    string msg = String.Format("Entity: {0} Property: {1} Error: {2}"
        //                        , validationErrors.Entry.Entity.ToString()
        //                        , validationError.PropertyName
        //                        , validationError.ErrorMessage);
        //                    sb.AppendLine(msg);
        //                    Trace.TraceInformation(msg);


        //                }
        //            }
        //            NewsGPS.Logic.BusinessRules.Log.PublicarErro(sb.ToString());
        //            throw new Exception(sb.ToString());
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }

        //    }
        //}

        //protected void ExcluirDePara(int idInterno)
        //{
        //    using (var tranAzure = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        //Cria ou atualiza o de-para...
        //        var dePara = this.ObterDeParaPorIdInterno(idInterno, this.IDCliente);
        //        if (dePara != null)
        //        {
        //            EFContext.GetDBSetAdmin<Domain.Entities.Suporte.Integracao>().Remove(dePara);
        //            EFContext.GetContextAdmin().SaveChanges();
        //            tranAzure.Complete();
        //        }
        //    }
        //}

        //protected void SalvarDataUltimaImportacao(int idIntegracao, DateTime DataUltimaImportacao)
        //{
        //    ConfiguracaoIntegracaoRepository config = new ConfiguracaoIntegracaoRepository();
        //    var cfg = config.Get(idIntegracao);


        //    cfg.DataUltimaImportacao = DataUltimaImportacao;
        //    config.Update(cfg);
        //    config.SaveChanges();

        //}

        //protected void SalvarDataUltimaExportacao(int idIntegracao, DateTime DataUltimaExportacao)
        //{
        //    ConfiguracaoIntegracaoRepository config = new ConfiguracaoIntegracaoRepository();
        //    var cfg = config.Get(idIntegracao);


        //    cfg.DataUltimaExportacao = DataUltimaExportacao;
        //    config.Update(cfg);
        //    config.SaveChanges();

        //}

        private SistemaCliente _SistemaCliente;
        protected SistemaCliente SistemaCliente
        {
            get
            {
                if (_SistemaCliente == null)
                {
                    //_SistemaCliente = EFContext.GetDBSetAdmin<SistemaCliente>()
                    //    .Where(x => x.IDSistema == (int)this.Sistema && x.IDCliente == this.IDCliente)
                     //   .AsNoTracking()
                     //   .FirstOrDefault();
                }
                return _SistemaCliente;
            }
        }
        public abstract override void ProcessarUnitario(object item);
        public abstract override void Processar();
    }
}
