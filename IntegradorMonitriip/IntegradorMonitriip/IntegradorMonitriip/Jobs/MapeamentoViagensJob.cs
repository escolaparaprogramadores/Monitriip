using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorMonitriip.Model;
using NewsGPS.Domain;
using NewsGPS.Repository;
using NewsGPS.Domain.Entities.Suporte;
using NewsGPS.Contracts.Filter;
using NewsGPS.Contracts.DTO;
using IntegradorRepository.DataRepository;
using System.Data.Entity;
using System.Transactions;
using IntegradorRepository.LocalDatabase.Repository.Entity;

namespace IntegradorMonitriip.Jobs
{
    public class MapeamentoViagensJob
    {
        public List<JsonExcelDTO> formatData( )
        {
            var repLog = new AnttLogRepository();
            var ret = new List<JsonExcel>();
            var retorno = new List<JsonExcelDTO>();
            //var dtRef = new DateTime(2017,10,02);
            //var dtRef1 = new DateTime(2017, 10, 03);
            var dtRef = DateTime.Now.Date;
            var dtRef1 = DateTime.Now.AddDays(-1).Date;
            try
            {
                var qryAntt = repLog.GetQuery();
                var qryGrade = new GradeOperacaoRepository().GetQuery();
                var context = new DatabaseContext();
                //var qryGrade = rep;

               var res =
                    context.GradeOperacao
                    .Where(x => x.DataReferencia < dtRef && x.DataReferencia >= dtRef1)
                    .AsNoTracking()
                    .ToList();

                var size = res.Count();
                //var untouched = res;
                for (var i = 0; i < size; i++)
                {
                    //if (i % 100 == 0)
                    //    break;

                    try
                    {
                        if (!string.IsNullOrEmpty(res[i].Ope_GradeOperacaoOnibus.CodFretamento))
                            continue;

                        var Chave = string.Format("L{0}S{1}", res[i].Ope_GradeOperacaoOnibus.GPS_Linha_Rota.IDLinha, res[i].Ope_GradeOperacaoOnibus.IDTipoJornada, res[i].Ope_GradeOperacaoOnibus.GPS_Linha_Rota.IDTipoRota == 23213 ? 1 : 0);

                        var pk = NewsGPS.Domain.AnttLog.GetPartitionKey(res[i].DataPartidaPrevista.Value.DateTime, res[i].IDCliente, Chave);
                        var qry = repLog.GetQueryByPartitionKey(pk);
                        var respose = qry.Where(x => x.codigoTipoLogID == 7).ToList();

                        if (respose.Count() < 1)
                        {
                            continue;
                        }
                        else
                        {
                            var inicio = respose.Where(x => x.codigoTipoRegistroViagem == 1).FirstOrDefault();
                            if (inicio != null)
                            {
                                res[i].IsAberto = !(inicio.isErro == true);
                            }

                            var fim = respose.Where(x => x.codigoTipoRegistroViagem == 0).FirstOrDefault();
                            if (fim != null)
                            {
                                res[i].IsFechado = !(fim.isErro == true);
                            }

                            var transbordoIni = respose.Where(x => x.codigoTipoRegistroViagem == 3).FirstOrDefault();
                            if (transbordoIni != null)
                            {
                                res[i].IsTransbordoAberto = !(transbordoIni.isErro == true);
                            }

                            var transbordoFim = respose.Where(x => x.codigoTipoRegistroViagem == 2).FirstOrDefault();
                            if (transbordoFim != null)
                            {
                                res[i].IsTransbordoFechado = !(transbordoFim.isErro == true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                for(var i = 0; i < size; i = i + 10000)
                {
                    save(res.Skip(i).Take(10000).ToList()/*, untouched.Skip(i).Take(10000).ToList()*/);
                }
                //save(res.Skip(0).Take(10000).ToList());
                //save(res.Skip(10000).Take(10000).ToList());
                //save(res.Skip(20000).Take(10000).ToList());
                //save(res.Skip(30000).ToList());

            }
            catch (Exception ex)
            {
            }

            return retorno.OrderBy(x => x.Numero).ToList();
        }

        public void save(List<Ope_GradeOperacao> res/*, List<Ope_GradeOperacao> untouched*/)
        {
            var rep = new GradeOperacaoRepository();
            rep.save(res/*,untouched.Take(100).ToList()*/);
            //var context = rep._Context;

            //using (TransactionScope scope = new TransactionScope())
            //{
            //    try
            //    {
            //        context.BulkUpdate(res);
            //        context.BulkSaveChanges();
            //        scope.Complete();
            //        scope.Dispose();
            //        context.Dispose();
            //    }
            //    catch (Exception ex)
            //    {
            //        scope.Dispose();
            //        context.Dispose();
            //    }
            //}
            //ver new do contexto
        }

        
        public static void Main(List<ErrosIntegracaoLog> res, DateTime dtRef)
        {
            var sDtRef = Convert.ToInt32(dtRef.ToString("yyyyMMdd"));
            var lstDetalhes = new List<NewsGPS.Domain.TDerros>();

            var groups = res.GroupBy(g => g.IDCliente).ToList();
            var list = new List<NewsGPS.Domain.TDerros>();

            foreach (var group in groups)
            {
                var cliente = group.FirstOrDefault().IDCliente;
                var tpErrorGroup = group.GroupBy(x => x.idTpErro).ToList();

                foreach (var item in tpErrorGroup)
                {
                    var lst = item.ToList();
                    var groupDescErro = lst.GroupBy(g => g.descricao).ToList();

                    foreach (var itemDesc in groupDescErro)
                    {
                        var descricao = itemDesc.FirstOrDefault().InnerException;

                        if (string.IsNullOrEmpty(descricao) || descricao.Equals("Inner Nula"))
                            descricao = itemDesc.FirstOrDefault().descricao;

                        var erro = new NewsGPS.Domain.TDerros(sDtRef, cliente);
                        erro.Descricao = descricao;
                        erro.TipoErro = item.FirstOrDefault().idTpErro;
                        erro.TotalN = itemDesc.Count();

                        list.Add(erro);
                    }
                }
            }

            var rep = new NewsGPS.Repository.TDerrosRepository();
            rep.MultiplesAdd(list);
        }
    }
}
