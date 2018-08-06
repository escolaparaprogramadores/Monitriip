using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using EntityFramework.BulkInsert.Extensions;
using System.Data.Entity.Core.Objects;
using System.Threading;
using IntegradorRepository.DataRepository;
using IntegradorRepository.LocalDatabase.Repository.Entity;
using IntegradorRepositoryAzure;
using System.Diagnostics;
using Microsoft.SqlServer.Types;

namespace IntegradorRepository.LocalDatabase
{
    public class ServicoRepository
    {
        public DatabaseContext dbContext
        {
            get
            {
                return new DatabaseContext();
            }
        }

        public ServicoRepository()
        {
        }

        public void GetInfoLinha(short linha, ref GPS_Linha_Rota Linha, int IDOrigem, int IDDestino, List<GPS_Prefixo_Linha> linhas, DatabaseContext dbContext/*, ref List<countErros> listaErros*/)
        {
            try
            {
                //Linha = linhas.Where(x => x.Prefixo.Replace("-", "").Replace(" ", "").Equals(linha)).
                //    FirstOrDefault().
                //    GPS_Linha.GPS_Linha_Rota.
                //    Where(r => r.GPS_Rota.IDPontoOrigem == IDOrigem
                //    && r.GPS_Rota.IDPontoDestino == IDDestino
                //    && r.GPS_Rota.Ativo == true).
                //    FirstOrDefault();

                //Linha = dbContext.LinhaRota.
                //    Where(r => r.GPS_Rota.IDPontoOrigem == IDOrigem
                //    && r.GPS_Rota.IDPontoDestino == IDDestino
                //    && r.IDLinha == linha
                //    && r.Ativo == true
                //    && r.GPS_Rota.Ativo == true).FirstOrDefault();


                /*obtendo apenas rotas associada a linha - Claudio Marcio 20/03/2018*/
                //var listaLinha = dbContext.LinhaRota.
                //    Where(r=> r.IDLinha == linha
                //    && r.Ativo == true
                //    && r.GPS_Rota.Ativo == true).ToList();

                //Linha = listaLinha.Where(r => r.GPS_Rota.IDPontoOrigem == IDOrigem
                //    && r.GPS_Rota.IDPontoDestino == IDDestino).FirstOrDefault();

                Linha = (from lr in dbContext.LinhaRota
                         join r in dbContext.Rota on lr.IDRota equals r.ID
                         where lr.Ativo == true && lr.IDLinha == linha && 
                               r.Ativo == true && r.IDPontoOrigem == IDOrigem && r.IDPontoDestino == IDDestino
                         select lr).FirstOrDefault();

                //if (Linha == null)
                //{
                //    var rotasCount = dbContext.LinhaRota.
                //        Where(r => r.GPS_Rota.IDPontoOrigem == IDOrigem
                //        && r.GPS_Rota.IDPontoDestino == IDDestino
                //        //&& (r.GPS_Linha.CodigoANTT.Replace("-", "").Replace(" ", "").ToLower().Equals(linha.Replace("-", "").Replace(" ", "").ToLower())
                //        //&& (r.GPS_Linha.CodigoANTT.Replace("-","").Trim().ToLower().Equals(linha.Trim().ToLower())
                //        //|| r.GPS_Linha.Numero.Replace("-", "").Replace(" ", "").ToLower().Equals(linha.Replace("-", "").Replace(" ", "").ToLower()))
                //        //|| r.GPS_Linha.Numero.Trim().ToLower().Equals(linha.Trim().ToLower()) )
                //        //&& r.GPS_Linha.Ativo == true
                //        && r.GPS_Rota.Ativo == true
                //        ).
                //        Count();

                //    var linhasCount = dbContext.LinhaRota.
                //            Where(r =>
                //            (
                //            //r.GPS_Linha.CodigoANTT.Replace("-", "").Replace(" ", "").ToLower().Equals(linha.Replace("-", "").Replace(" ", "").ToLower())
                //            //&& (r.GPS_Linha.CodigoANTT.Replace("-","").Trim().ToLower().Equals(linha.Trim().ToLower())
                //            //|| r.GPS_Linha.Numero.Replace("-", "").Replace(" ", "").ToLower().Equals(linha.Replace("-", "").Replace(" ", "").ToLower()))
                //            r.GPS_Linha.ID == linha
                //            && r.GPS_Linha.Ativo == true
                //            //&& r.GPS_Rota.Ativo == true
                //            )).
                //            Count();
                //    var erros = new countErros();
                //    erros.rotasCount = rotasCount;
                //    erros.linhasCount = linhasCount;
                //    erros.Prefixo = linha.ToString();
                //    erros.IDOrigem = IDOrigem;
                //    erros.IDDestino = IDOrigem;

                //    listaErros.Add(erros);
                //}

            }
            catch (Exception ex)
            {
            }
        }

        public void GetInfoLinha(short linha, ref GPS_Linha_Rota Linha, int IDOrigem, int IDDestino, List<GPS_Prefixo_Linha> linhas, DatabaseContext dbContext,
            ref int linhaNula, ref int rotaNula)
        {
            try
            {

                //Linha = linhas.Where(x => x.Prefixo.Replace("-", "").Replace(" ", "").Equals(linha)).
                //    FirstOrDefault().
                //    GPS_Linha.GPS_Linha_Rota.
                //    Where(r => r.GPS_Rota.IDPontoOrigem == IDOrigem
                //    && r.GPS_Rota.IDPontoDestino == IDDestino
                //    && r.GPS_Rota.Ativo == true).
                //    FirstOrDefault();

                Linha = dbContext.LinhaRota.
                    Where(r => r.GPS_Rota.IDPontoOrigem == IDOrigem
                    && r.GPS_Rota.IDPontoDestino == IDDestino
                    && r.IDLinha == linha
                    //&& (prefixos.Any(y => linha.Equals(y)) == true)
                    //|| numeros.Any(y => linha.Equals(y)) == true)
                    && r.GPS_Rota.Ativo == true
                    ).
                    FirstOrDefault();

                //if (Linha == null)
                //{
                //    var rotasCount = dbContext.LinhaRota.
                //        Where(r => r.GPS_Rota.IDPontoOrigem == IDOrigem
                //        && r.GPS_Rota.IDPontoDestino == IDDestino
                //        //&& (r.GPS_Linha.CodigoANTT.Replace("-", "").Replace(" ", "").ToLower().Equals(linha.Replace("-", "").Replace(" ", "").ToLower())
                //        //&& (r.GPS_Linha.CodigoANTT.Replace("-","").Trim().ToLower().Equals(linha.Trim().ToLower())
                //        //|| r.GPS_Linha.Numero.Replace("-", "").Replace(" ", "").ToLower().Equals(linha.Replace("-", "").Replace(" ", "").ToLower()))
                //        //|| r.GPS_Linha.Numero.Trim().ToLower().Equals(linha.Trim().ToLower()) )
                //        //&& r.GPS_Linha.Ativo == true
                //        && r.GPS_Rota.Ativo == true
                //        ).
                //        Count();

                //    var linhasCount = dbContext.LinhaRota.
                //            Where(r =>
                //            (r.GPS_Linha.CodigoANTT.Replace("-", "").Replace(" ", "").ToLower().Equals(linha.Replace("-", "").Replace(" ", "").ToLower())
                //            //&& (r.GPS_Linha.CodigoANTT.Replace("-","").Trim().ToLower().Equals(linha.Trim().ToLower())
                //            || r.GPS_Linha.Numero.Replace("-", "").Replace(" ", "").ToLower().Equals(linha.Replace("-", "").Replace(" ", "").ToLower()))
                //            && r.GPS_Linha.Ativo == true
                //            //&& r.GPS_Rota.Ativo == true
                //            ).
                //            Count();

                //    var erros = new countErros();
                //    erros.rotasCount = rotasCount;
                //    erros.linhasCount = linhasCount;
                //    erros.Prefixo = linha;
                //    erros.IDOrigem = IDOrigem;
                //    erros.IDDestino = IDOrigem;

                //    listaErros.Add(erros);
                //}

                //}

            }
            catch (Exception ex)
            {
            }
        }

        private List<Ope_GradeOperacaoOnibus> findEqual(int idCliente, List<Ope_GradeOperacaoOnibus> lista, DatabaseContext context)
        {
            var data = DateTime.UtcNow.Date;
            var listaHoje = context.GradeOperacaoOnibus.Where(g =>
            DbFunctions.TruncateTime(g.Ope_GradeOperacao.DataPartidaPrevista.Value) == DbFunctions.TruncateTime(data)
            && g.Ope_GradeOperacao.IDCliente == idCliente).ToList();

            List<Ope_GradeOperacaoOnibus> ret = new List<Ope_GradeOperacaoOnibus>();

            foreach (var item in lista)
            {

                var has = listaHoje.Where(x => x.CodigoSRVP.Trim().ToLower().Equals(item.CodigoSRVP.Trim().ToLower())).FirstOrDefault();
                var count = ret.Where(x => x.CodigoSRVP.Trim().ToLower().Equals(item.CodigoSRVP.Trim().ToLower())).Count() > 0;

                if (has != null)
                    continue;

                if (count)
                    continue;

                ret.Add(item);
            }

            return ret;
        }
        public void saveGrades(List<Ope_GradeOperacaoOnibus> lista, int idCliente, DatabaseContext context)
        {

            var entityAtual = new Ope_GradeOperacaoOnibus();
            var gradeSalva = new Ope_GradeOperacaoOnibus();

            foreach (var item in lista)
            {
                TentarSalvarNovamente:
                int count = 0;
                entityAtual = item;
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        //string sSource;
                        //string sLog;
                        //string sEvent;
                        //sSource = "Integrador de dados";
                        //sLog = "Application";
                        //sEvent = "Pre Loop";
                        //if (!EventLog.SourceExists(sSource))
                        //    EventLog.CreateEventSource(sSource, sLog);
                        //EventLog.WriteEntry(sSource, sEvent);
                        //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);

                        findEqual(idCliente, item, context, ref gradeSalva);

                        scope.Complete();
                        scope.Dispose();
                        
                        //sSource = "Integrador de dados";
                        //sLog = "Application";
                        //sEvent = "Fechou a transacao";
                        //if (!EventLog.SourceExists(sSource))
                        //    EventLog.CreateEventSource(sSource, sLog);
                        //EventLog.WriteEntry(sSource, sEvent);
                        //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);
                    }
                    catch (Exception ex)
                    {
                        string sSource;
                        string sLog;
                        string sEvent;
                        sSource = "Integrador de dados";
                        sLog = "Application";
                        sEvent = "Erro na transacao: " + ex.Message + " /n  Inner:" + ex.InnerException + " /n StackTrace" + ex.StackTrace;
                        if (!EventLog.SourceExists(sSource))
                            EventLog.CreateEventSource(sSource, sLog);
                        EventLog.WriteEntry(sSource, sEvent);
                        EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);
                        count++;

                        scope.Complete();
                        scope.Dispose();

                        if (count < 3)
                            goto TentarSalvarNovamente;
                                               
                    }
                }
               

                if (gradeSalva != null && entityAtual != null && gradeSalva.ID > 0)
                {
                    ServicosRelacionadosRepository srRep = new ServicosRelacionadosRepository();

                    if (entityAtual.ListaServicosRelacionados != null)
                    {
                        foreach (var item2 in entityAtual.ListaServicosRelacionados)
                        {

                            item2.idGradeOperacao = gradeSalva.ID;

                            if (!srRep.CheckRegistro(item2.ID, gradeSalva.ID))
                            {
                                srRep.Add(item2,context);
                            }
                            else
                            {
                                srRep.Update(item2, context);
                            }
                        }
                    }

                }
            }

            context.Dispose();


        }

        public List<GPS_Prefixo_Linha> getLineByClient(int iDCliente, DatabaseContext dbContext)
        {
            var list = dbContext.PrefixoLinha.
                Where(x => x.IDCliente == iDCliente
                && string.IsNullOrEmpty(x.Prefixo) == false
                && x.GPS_Linha.Ativo == true).
                AsNoTracking().
                ToList();
            return list;
        }

        private void findEqual(int idCliente, Ope_GradeOperacaoOnibus entity, DatabaseContext context, ref Ope_GradeOperacaoOnibus modelOpe)
        {
            try
            {
                Ope_GradeOperacaoOnibus grade = null;
                

                grade = context.GradeOperacaoOnibus.Where(g =>
             (DbFunctions.CreateDateTime(
                    g.Ope_GradeOperacao.DataPartidaPrevista.Value.Year,
                    g.Ope_GradeOperacao.DataPartidaPrevista.Value.Month,
                    g.Ope_GradeOperacao.DataPartidaPrevista.Value.Day,
                    g.Ope_GradeOperacao.DataPartidaPrevista.Value.Hour,
                    g.Ope_GradeOperacao.DataPartidaPrevista.Value.Minute,
                    0)
                    == entity.Ope_GradeOperacao.DataPartidaPrevista)
             //&& g.IdLinhaRota == entity.IdLinhaRota
             && g.GPS_Linha_Rota.GPS_Rota.IDPontoDestino == entity.GPS_Linha_Rota.GPS_Rota.IDPontoDestino
             && g.GPS_Linha_Rota.GPS_Rota.IDPontoOrigem == entity.GPS_Linha_Rota.GPS_Rota.IDPontoOrigem
             && g.Ope_GradeOperacao.IDCliente == idCliente            
             ).FirstOrDefault();

                //string sSource;
                //string sLog;
                //string sEvent;
                //sSource = "Integrador de dados";
                //sLog = "Application";
                //sEvent = "pos qry e testando condicionais";
                //if (!EventLog.SourceExists(sSource))
                //    EventLog.CreateEventSource(sSource, sLog);
                //EventLog.WriteEntry(sSource, sEvent);
                //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);

                Ope_GradeOperacaoOnibus GradeSalva = null;

                //Registro com mesma hora de partida no mesmo dia Edita
                if (grade != null)
                {   //registro com o mesmo SRVP sai
                    if (grade.CodigoSRVP != null && !grade.CodigoSRVP.Trim().ToLower().Equals(entity.CodigoSRVP.Trim().ToLower()))
                    {   //registro que têm multiplos SRVP's
                        if (grade.CodigoSRVP.Trim().ToLower().Contains("/"))
                        {   //registro com multiplos SRVP que não contenha o SRVP inserido
                            var srvps = grade.CodigoSRVP.Trim().Split('/');
                            var contem = false;
                            //valida se algum SRVP é igual ao SRVP que vai ser adicionado
                            foreach (var item in srvps)
                            {
                                if (item.Equals(entity.CodigoSRVP.Trim().ToLower()))
                                {
                                    contem = true;
                                    break;
                                }
                            }
                            if (!contem)
                            {                             
                                grade.CodigoSRVP = (grade.CodigoSRVP.Contains(entity.CodigoSRVP.Trim()) ? grade.CodigoSRVP.Trim() : grade.CodigoSRVP.Trim() + "/" + entity.CodigoSRVP.Trim());

                                if (grade.Ope_GradeOperacaoSeccao == null && entity.Ope_GradeOperacaoSeccao != null)
                                {
                                    grade.Ope_GradeOperacaoSeccao = entity.Ope_GradeOperacaoSeccao;
                                    modelOpe = saveEntity(grade, EntityState.Modified, context, EntityState.Added);
                                }
                                else if (grade.Ope_GradeOperacaoSeccao != null)
                                {
                                    if (grade.Ope_GradeOperacaoSeccao.IDMotorista == null)
                                        grade.Ope_GradeOperacaoSeccao.IDMotorista = entity.Ope_GradeOperacaoSeccao.IDMotorista;

                                    if (grade.Ope_GradeOperacaoSeccao.IDVeiculo == null)
                                        grade.Ope_GradeOperacaoSeccao.IDVeiculo = entity.Ope_GradeOperacaoSeccao.IDVeiculo;

                                    modelOpe = saveEntity(grade, EntityState.Modified, context, EntityState.Unchanged);

                                }
                                else
                                {
                                    modelOpe = saveEntity(grade, EntityState.Modified, context, EntityState.Unchanged);
                                }
                            }
                        }
                        else
                        {   //registro inicialmente com 1 SRVP, adiciona mais 1 SRVP
                            grade.CodigoSRVP = (grade.CodigoSRVP.Contains(entity.CodigoSRVP.Trim()) ? grade.CodigoSRVP.Trim() : grade.CodigoSRVP.Trim() + "/" + entity.CodigoSRVP.Trim());

                            if (grade.Ope_GradeOperacaoSeccao == null && entity.Ope_GradeOperacaoSeccao != null)
                            {
                                grade.Ope_GradeOperacaoSeccao = entity.Ope_GradeOperacaoSeccao;
                                modelOpe = saveEntity(grade, EntityState.Modified, context, EntityState.Added);
                            }
                            else if (grade.Ope_GradeOperacaoSeccao != null)
                            {
                                if (grade.Ope_GradeOperacaoSeccao.IDMotorista == null)
                                    grade.Ope_GradeOperacaoSeccao.IDMotorista = entity.Ope_GradeOperacaoSeccao.IDMotorista;

                                if (grade.Ope_GradeOperacaoSeccao.IDVeiculo == null)
                                    grade.Ope_GradeOperacaoSeccao.IDVeiculo = entity.Ope_GradeOperacaoSeccao.IDVeiculo;

                                modelOpe = saveEntity(grade, EntityState.Modified, context, EntityState.Unchanged);

                            }
                            else
                            {
                                modelOpe = saveEntity(grade, EntityState.Modified, context, EntityState.Unchanged);
                            }

                           
                        }
                    }else
                    {
                        /*Atualiza Chegada Prevista*/
                        if (!grade.Ope_GradeOperacao.DataChegadaPrevista.Equals(entity.Ope_GradeOperacao.DataChegadaPrevista))
                        {
                            grade.Ope_GradeOperacao.DataChegadaPrevista = entity.Ope_GradeOperacao.DataChegadaPrevista;

                            if (grade.Ope_GradeOperacaoSeccao == null && entity.Ope_GradeOperacaoSeccao != null)
                            {
                                grade.Ope_GradeOperacaoSeccao = entity.Ope_GradeOperacaoSeccao;
                                modelOpe = saveEntity(grade, EntityState.Modified, context, EntityState.Added);
                            }
                            else if (grade.Ope_GradeOperacaoSeccao != null)
                            {
                                if (grade.Ope_GradeOperacaoSeccao.IDMotorista == null)
                                    grade.Ope_GradeOperacaoSeccao.IDMotorista = entity.Ope_GradeOperacaoSeccao.IDMotorista;

                                if (grade.Ope_GradeOperacaoSeccao.IDVeiculo == null)
                                    grade.Ope_GradeOperacaoSeccao.IDVeiculo = entity.Ope_GradeOperacaoSeccao.IDVeiculo;

                                modelOpe = saveEntity(grade, EntityState.Modified, context, EntityState.Unchanged);

                            }
                            else
                            {
                                modelOpe = saveEntity(grade, EntityState.Modified, context, EntityState.Unchanged);
                            }
                        }

                    }
                }
                else
                {   //adiciona um novo registro ( grade )
                    //entity. entity.GPS_Linha_Rota.GPS_Linha.ToleranciaAnterior;
                    modelOpe = saveEntity(entity, EntityState.Added, context, EntityState.Unchanged);


                }

                

                //if(GradeSalva != null)
                //{
                //    ServicosRelacionadosRepository srRep = new ServicosRelacionadosRepository();

                //    if (entity.ListaServicosRelacionados != null)
                //    {
                //        foreach (var item in entity.ListaServicosRelacionados)
                //        {

                //            item.idGradeOperacao = GradeSalva.ID;

                //            if (!srRep.CheckRegistro(item.ID, GradeSalva.ID))
                //            {
                //                srRep.Add(item);
                //            }
                //            else
                //            {
                //                srRep.Update(item, context);
                //            }
                //        }
                //    }
                    
                //}

                //sSource = "Integrador de dados";
                //sLog = "Application";
                //sEvent = "saindo da interacao";
                //if (!EventLog.SourceExists(sSource))
                //    EventLog.CreateEventSource(sSource, sLog);
                //EventLog.WriteEntry(sSource, sEvent);
                //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);
            }
            catch(System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string sSource;
                string sLog;
                string sEvent;

                sSource = "Integrador de dados - Empresa " + idCliente;
                sLog = "Application";
                sEvent = "Erro ao salvar/atualizar grade outside: " + ex.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage + " /n  Inner:" + ex.InnerException + " /n StackTrace" + ex.StackTrace;

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent);
                EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);

              
            }
            catch (Exception ex)
            {
                string sSource;
                string sLog;
                string sEvent;           

                sSource = "Integrador de dados  - Empresa " + idCliente;
                sLog = "Application";
                sEvent = "Erro ao salvar/atualizar grade outside: " + ex.Message + " /n  Inner:" + ex.InnerException + " /n StackTrace" + ex.StackTrace;

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent);
                EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);

            }
        }

        static Ope_GradeOperacaoOnibus saveEntity(Ope_GradeOperacaoOnibus entity, EntityState state, DatabaseContext context, EntityState stateSeccao)
        {
            //try
            //{
            //string sSource;
            //string sLog;
            //string sEvent;
            //sSource = "Integrador de dados";
            //sLog = "Application";
            //sEvent = "Pre salvamento normal";
            //if (!EventLog.SourceExists(sSource))
            //    EventLog.CreateEventSource(sSource, sLog);
            //EventLog.WriteEntry(sSource, sEvent);
            //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);

            Ope_GradeOperacaoSeccao seccao = null;
            if (entity.Ope_GradeOperacaoSeccao != null)
            {
                seccao = entity.Ope_GradeOperacaoSeccao;
                entity.Ope_GradeOperacaoSeccao = null;
            }

          
            context.Entry(entity).State = state;
            context.SaveChanges();

            //sSource = "Integrador de dados";
            //sLog = "Application";
            //sEvent = "Pre salvamento seccao";
            //if (!EventLog.SourceExists(sSource))
            //    EventLog.CreateEventSource(sSource, sLog);
            //EventLog.WriteEntry(sSource, sEvent);
            //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);

            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            if ((entity.Ope_GradeOperacao.IDCliente == 1582 || entity.Ope_GradeOperacao.IDCliente == 1581) && seccao != null)
            {
                seccao.IDGradeOperacao = entity.ID;
                context.Entry(seccao).State = stateSeccao == EntityState.Unchanged ? state : stateSeccao;
                context.SaveChanges();
            }
            saveGrade(entity);

            return entity;
            //}
            //catch (Exception ex)
            //{
            //    string sSource;
            //    string sLog;
            //    string sEvent;

            //    sSource = "Integrador de dados";
            //    sLog = "Application";
            //    sEvent = "Erro ao salvar/atualizar grade: " + ex.Message + " /n  Inner:" + ex.InnerException + " /n StackTrace" + ex.StackTrace + "/n TargetSite:" + ex.TargetSite 
            //        + " /n Data:" + ex.Data;

            //    if (!EventLog.SourceExists(sSource))
            //        EventLog.CreateEventSource(sSource, sLog);

            //    EventLog.WriteEntry(sSource, sEvent);
            //    EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);
            //}
        }


        static void saveGrade(Ope_GradeOperacaoOnibus entity)
        {
            try
            {
                //string sSource;
                //string sLog;
                //string sEvent;

                //sSource = "Integrador de dados";
                //sLog = "Application";
                //sEvent = "Pre salvamento SaveGrade Tables";

                //if (!EventLog.SourceExists(sSource))
                //    EventLog.CreateEventSource(sSource, sLog);

                //EventLog.WriteEntry(sSource, sEvent);
                //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);

                var SRVP = entity.CodigoSRVP.Split('/');

                var Grade = new NewsGPS.Domain.Grades(entity.Ope_GradeOperacao.IDCliente,
                    entity.Ope_GradeOperacao.DataPartidaPrevista.Value.DateTime,
                    SRVP.Length > 1 ? SRVP[1] : SRVP[0],
                    entity.GPS_Linha_Rota.GPS_Rota.GPS_PontoReferencia1.ID,
                    entity.GPS_Linha_Rota.GPS_Rota.GPS_PontoReferencia.ID)
                {
                    //como tratar multiplos SRVPs
                    SRVP = SRVP.Length > 1 ? SRVP[1] : SRVP[0],
                    DataControle = Convert.ToInt32(DateTime.Now.Date.ToString("yyyyMMdd")),
                    IDCliente = entity.Ope_GradeOperacao.IDCliente,
                    DataHoraViagem = entity.Ope_GradeOperacao.DataPartidaPrevista.Value.DateTime,
                    IDGrade = entity.ID,
                    Origem = entity.GPS_Linha_Rota.GPS_Rota.GPS_PontoReferencia1.ID,
                    Destino = entity.GPS_Linha_Rota.GPS_Rota.GPS_PontoReferencia.ID,
                    //ver multiplos prefixos
                    //PrefixoANTT = entity.PrefixoRJNET
                    //PrefixoANTT = entity.GPS_Linha_Rota.GPS_Linha.GPS_Prefixo_Linha.
                };

                var rep = new NewsGPS.Repository.GradesRepository();
                rep.Add(Grade);
            }
            catch (Exception ex)
            {
                //string sSource;
                //string sLog;
                //string sEvent;

                //sSource = "Integrador de dados";
                //sLog = "Application";
                //sEvent = "Erro ao salvar parte 2: " + ex.Message + " /n  Inner:" + ex.InnerException + " /n StackTrace" + ex.StackTrace;

                //if (!EventLog.SourceExists(sSource))
                //    EventLog.CreateEventSource(sSource, sLog);

                //EventLog.WriteEntry(sSource, sEvent);
                //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);

                if (ex.InnerException.ToString().Contains("(409)"))
                    return;

                var rep = new ErrosGradesRepository();
                var obj = new NewsGPS.Domain.ErrosGrades();
                obj.DataHoraEvento = DateTime.UtcNow;
                obj.descricao = ex.Message;
                obj.IDCliente = entity.Ope_GradeOperacao.IDCliente;
                obj.stacktrace = ex.StackTrace;
                rep.saveLog(obj);
            }
        }

        public void GetInfoPontoReferencia(string origem, string destino, int IDCliente, ref int IDOrigem, ref int IDDestino, DatabaseContext dbContext)
        {
            try
            {
                IDOrigem = dbContext.RefIntegracao.
                    Where(r =>
                    r.CodIntegracao.Trim().Equals(origem.Trim())
                    && r.IDCliente == IDCliente
                    && r.Ativo == true
                    ).
                    Select(r => r.IDPontoReferencia).
                    FirstOrDefault() ?? 0;

                IDDestino = dbContext.RefIntegracao.
                    Where(r =>
                    r.CodIntegracao.Trim().Equals(destino.Trim())
                    && r.IDCliente == IDCliente
                    && r.Ativo == true
                    ).
                    Select(r => r.IDPontoReferencia).
                    FirstOrDefault() ?? 0;
            }
            catch (Exception ex)
            {
            }
        }

        public void GetInfoRota(int IDOrigem, int IDDestino, int IDCliente)
        {

        }
    }
}
