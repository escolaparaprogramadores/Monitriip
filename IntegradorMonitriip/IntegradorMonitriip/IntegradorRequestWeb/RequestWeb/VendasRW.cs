using antt.gov.br.monitriip.v1._0;
using System;
using System.Net;
using System.Xml;
using System.Net.Http;
using System.Net.Http.Headers;
using IntegradorModel.Model;
using IntegradorMonitriip.DataRepository;
using IntegradorRepositoryAzure;
using IntegradorMonitriip.Model;
using System.Threading;

namespace IntegradorRequestWeb.RequestWeb
{
    public class VendasRW
    {
        public static XmlDocument BuscarDadosPassagens(string requestUrl, int count, int idCliente)
        {
            if (count > 3)
                return null;

            NewsGPS.Logic.BusinessRules.Log.PublicarTrace(string.Format("{0}", requestUrl), "RJ");
            try
            {

                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Timeout = Parameters.TIMEOUT;

                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(Parameters.USERNAME + ":" + Parameters.PASSWORD));
                request.Headers.Add("Authorization", "Basic " + encoded);

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());

                //Thread job = new Thread(
                //                unused => ExitoIntegracao("Grades", requestUrl, idCliente)
                //                );
                //job.Start();
                ExitoIntegracao("Vendas", requestUrl, idCliente);


                return xmlDoc;

            }       
            catch (Exception e)
            {

                if (count > 3 || e.Message.Contains("O servidor remoto retornou um erro: (404)"))
                {
                    count = 3;
                    var idTpErro = 2;
                    var entity = new IntegradorMonitriip.Model.ErrosIntegracaoLog(DateTime.UtcNow, idTpErro, idCliente);
                    entity.IDCliente = idCliente;
                    entity.Metodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    entity.Modulo = "Vendas Integradas";
                    entity.idTpErro = idTpErro;
                    entity.descricao = e.Message;
                    entity.stacktrace = e.StackTrace;
                    entity.DataHoraEvento = DateTime.UtcNow;
                    entity.InnerException = e.InnerException != null ? e.InnerException.ToString() : "Inner Nula";
              
                    ErroIntegracao("Vendas", requestUrl, entity);
                    //fazer chamada recursiva em caso de erro
                    NewsGPS.Logic.BusinessRules.Log.PublicarTrace(string.Format("{0} - {1}", requestUrl, e.Message), "RJ");
                }

                return BuscarDadosPassagens(requestUrl, count + 1, idCliente);
            }
        }

        public static XmlDocument BuscarDadosPassagensItamarati(string requestUrl, int count, int idCliente)
        {
            if (count > 3)
                return null;

            //NewsGPS.Logic.BusinessRules.Log.PublicarTrace(string.Format("{0}", requestUrl), "RJ");
            try
            {
                //requestUrl = requestUrl + "?empresa=1&data=" + data.ToString("yyMMdd");//"171101";
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Timeout = Parameters.TIMEOUT;
                request.Headers.Add("Authorization", "Token token=e1bb21633b1458200ce728f546325e7a");

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());

                //Thread job = new Thread(
                //                unused => ExitoIntegracao("Vendas", requestUrl, idCliente)
                //                );
                //job.Start();
                ExitoIntegracao("Vendas", requestUrl, idCliente);

                return xmlDoc;

            }
            catch (Exception e)
            {
                if (count > 3 || e.Message.Contains("O servidor remoto retornou um erro: (404)"))
                {
                    count = 3;
                    var idTpErro = 2;
                    var entity = new IntegradorMonitriip.Model.ErrosIntegracaoLog(DateTime.UtcNow, idTpErro, idCliente);
                    entity.IDCliente = idCliente;
                    entity.Metodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    entity.Modulo = "Vendas Integradas";
                    entity.idTpErro = idTpErro;
                    entity.descricao = e.Message;
                    entity.stacktrace = e.StackTrace;
                    entity.DataHoraEvento = DateTime.UtcNow;
                    entity.InnerException = e.InnerException != null ? e.InnerException.ToString() : "Inner Nula";

                    //Thread job = new Thread(
                    //                unused => ErroIntegracao("Vendas", requestUrl, entity)
                    //                );
                    //job.Start();

                    ErroIntegracao("Vendas", requestUrl, entity);
                    //fazer chamada recursiva em caso de erro
                    NewsGPS.Logic.BusinessRules.Log.PublicarTrace(string.Format("{0} - {1}", requestUrl, e.Message), "RJ");

                }

                return BuscarDadosPassagensItamarati(requestUrl, count + 1, idCliente);
            }
        }

        public static resultadoOperacao EnviaANTTWeb(vendaPassagem x, int idCliente)
        {
            var tkn = Parameters.TOKEN;
            var tokenZoomSat = Parameters.TOKEN_ZOOMSAT;
            var tokenSmart = Parameters.TOKEN_SMART;
            var tokenAgadelha = Parameters.TOKEN_Agadelha;

            if (idCliente == 7955)
            {
                tkn = tokenSmart;
            }
            else if (idCliente == 695)
            {
                tkn = tokenAgadelha;
            }
            else
            {
                var instancy = new GetEmpresas();
                var isZoom = instancy.isZoomSat(idCliente);

                if (isZoom)
                    tkn = tokenZoomSat;
            }

            resultadoOperacao ret = new resultadoOperacao();
            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Parameters.URL_ANTT);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tkn);

                    var task = System.Threading.Tasks.Task.Run<antt.gov.br.monitriip.v1._0.resultadoOperacao>(() =>
                    {

                        var res = client.PostAsXmlAsync<vendaPassagem>("rest/InserirLogVendaPassagem", x);
                        if (res.Result.IsSuccessStatusCode)
                        {
                            return res.Result.Content.ReadAsAsync<resultadoOperacao>();
                        }
                        else
                        {
                            return res.Result.Content.ReadAsAsync<antt.gov.br.monitriip.v1._0.resultadoOperacao>();
                        }
                    });

                    task.Wait();
                    ret = task.Result;
                    return ret;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    erro[] erro = new erro[] {
                            new erro() {
                            descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                            }};
                    return new resultadoOperacao() { erros = erro };
                }
                catch (Exception e)
                {
                    erro[] erro = new erro[] {
                                new erro() {
                                descricao = "Mensagem: Erro no envio logs para ANTT"
                                }};
                    return new resultadoOperacao() { erros = erro };
                }
            }
        }

        public static resultadoOperacao EnviaANTTWebCancelados(VendasModel x, int idCliente)
        {
            var tkn = Parameters.TOKEN;
            var tokenZoomSat = Parameters.TOKEN_ZOOMSAT;
            var tokenSmart = Parameters.TOKEN_SMART;
            var tokenAgadelha = Parameters.TOKEN_Agadelha;

            if (idCliente == 7955)
            {
                tkn = tokenSmart;
            }
            else if (idCliente == 695)
            {
                tkn = tokenAgadelha;
            }
            else
            {
                var instancy = new GetEmpresas();
                var isZoom = instancy.isZoomSat(idCliente);

                if (isZoom)
                    tkn = tokenZoomSat;
            }

            var obj = convertToCancelamento(x);
            resultadoOperacao ret = new resultadoOperacao();
            try
            {

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Parameters.URL_ANTT);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tkn);

                    var task = System.Threading.Tasks.Task.Run<antt.gov.br.monitriip.v1._0.resultadoOperacao>(() =>
                    {

                        var res = client.PostAsXmlAsync<passagemCancelada>("rest/InserirLogCancelarPassagem", obj);
                        if (res.Result.IsSuccessStatusCode)
                        {
                            return res.Result.Content.ReadAsAsync<resultadoOperacao>();
                        }
                        else
                        {
                            return res.Result.Content.ReadAsAsync<antt.gov.br.monitriip.v1._0.resultadoOperacao>();
                        }
                    });

                    task.Wait();
                    ret = task.Result;
                    return ret;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    erro[] erro = new erro[] {
                            new erro() {
                            descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                            }};
                    return new resultadoOperacao() { erros = erro };
                }
                catch (Exception e)
                {
                    erro[] erro = new erro[] {
                                new erro() {
                                descricao = "Mensagem: Erro no envio logs para ANTT"
                                }};
                    return new resultadoOperacao() { erros = erro };
                }
            }
        }

        static passagemCancelada convertToCancelamento(VendasModel venda)
        {
            var passagem = new passagemCancelada();
            passagem.identificacaoLinha = venda.linha.Trim().Replace("-", "").PadLeft(6, '0');

            if(Parameters.IDENTIFICADOR_ITAMARATI == venda.idCliente)
               passagem.numeroNovoBilheteEmbarque = (venda.numeroNovoBilheteEmbarque != null ? venda.numeroNovoBilheteEmbarque.Trim().PadLeft(6, '0') : null);
            else
            {
                var teste = "";
            }
            passagem.idLog = venda.idLog.Trim();
            passagem.dataViagem = venda.dataViagem.Trim();
            passagem.horaViagem = venda.horaViagem.Trim();
            passagem.numeroBilheteEmbarque = venda.numBilheteEmbarque.Trim().PadLeft(6, '0');
            passagem.codigoMotivoCancelamento = venda.status.Trim();
            passagem.dataHoraCancelamento = convertStringToDate(venda.dataEmissao.Trim(), venda.horaEmissao.Trim());

            return passagem;
        }

        static string convertStringToDate(string data, string hora)
        {
            var horaViagem = "";
            var dataViagem = "";
            if (hora.Length < 6)
            {
                horaViagem = (hora.Length < 4) ?
                                        "0" + hora.Substring(0, 1) + ":" +
                                        hora.Substring(1) + ":00"
                                                : hora.Substring(0, 2) + ":" +
                                                hora.Substring(2) + ":00";
            }
            else
            {
                horaViagem = hora.Substring(0, 2) + ":" +
                                hora.Substring(2, 2) + ":" +
                                hora.Substring(4);
            }

            if (data.Length < 8)
            {
                dataViagem = "20" + data.Substring(0, 2) + "-"
                                + data.Substring(2, 2) + "-" + data.Substring(4) + " "
                                + horaViagem;
            }
            else
            {
                dataViagem = data.Substring(0, 4) + "-"
                    + data.Substring(4, 2) + "-" + data.Substring(6) + "T"
                    + horaViagem;
            }

            return dataViagem;
        }

        private static void ErroIntegracao(string servico, string url, ErrosIntegracaoLog entity)
        {
            if (entity != null)
            {
                try
                {
                    try
                    {
                        var rep = new ErrosIntegracaoRepository();
                        rep.Add(entity);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                catch (Exception ex)
                {
                }
            }

            var statusRep = new StatusRequestRepository();
            statusRep.Add(entity, true, url);
        }

        private static void ExitoIntegracao(string servico, string url, int idCliente)
        {
            var entity = new StatusLog(DateTime.UtcNow, 1, idCliente);
            entity.IDCliente = idCliente;
            entity.Modulo = "Vendas Integradas";
            entity.Metodo = "";
            entity.idTpErro = 2;
            entity.descricao = "";
            entity.stacktrace = "";
            entity.InnerException = "";
            entity.DataHoraEvento = DateTime.UtcNow;
            entity.erro = false;
            entity.url = url;

            var statusRep = new StatusRequestRepository();

            var modelErro = statusRep.GetErroByIDCliente(idCliente);
            if (modelErro != null)
                statusRep.Delete(modelErro);
            statusRep.saveLog(entity);
        }
    }
}
