
using IntegradorMonitriip.Model;
using IntegradorRepository.LocalDatabase.Repository.Entity;
using IntegradorRepositoryAzure;
using NewsGPS.Contracts.DTO.RJ;
using NewsGPS.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Xml;

namespace IntegradorRequestWeb.RequestWeb
{
    public class ServicoRW
    {

        //Chama o webservice
        public static XmlDocument BuscarDadosServico(string requestUrl, int count, int idCliente)
        {
            if (count > 5)
                return null;

            NewsGPS.Logic.BusinessRules.Log.PublicarTrace(string.Format("{0}", requestUrl), "RJ");
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Timeout = 300000;//300000;///Parameters.TIMEOUT;

                string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(Parameters.USERNAME + ":" + Parameters.PASSWORD));
                request.Headers.Add("Authorization", "Basic " + encoded);

                request.Proxy = null;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                XmlDocument xmlDoc = new XmlDocument();
                string xml = "";

                xmlDoc.Load(response.GetResponseStream());

                try
                {
                    ExitoIntegracao("Grades", requestUrl, idCliente);
                }
                catch (Exception ex)
                {

                }

                return xmlDoc;
            }
            catch (Exception e)
            {

                if (count == 5 || e.Message.Contains("O servidor remoto retornou um erro: (404)"))
                {
                    count = 5;
                    var idTpErro = 1;
                    var entity = new ErrosIntegracaoLog(DateTime.UtcNow, idTpErro, idCliente);
                    entity.IDCliente = idCliente;
                    entity.Metodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    entity.Modulo = "Grades de Horarios";
                    entity.idTpErro = idTpErro;
                    entity.descricao = e.Message;
                    entity.stacktrace = e.StackTrace;
                    entity.DataHoraEvento = DateTime.UtcNow;
                    entity.InnerException = e.InnerException != null ? e.InnerException.ToString() : "Inner Nula";
                 
                    try
                    {
                        ErroIntegracao("Grades", requestUrl, entity);
                    }
                    catch (Exception ex)
                    {

                    }
                }else if(!e.Message.Contains("timeout"))
                  Thread.Sleep(120000);

                return BuscarDadosServico(requestUrl, count + 1, idCliente);
            }
        }

        public static XmlDocument BaixaServicosItamarati(string requestUrl, int count, DateTime data, int idCliente)
        {
            if (count > 5)
                return null;

            NewsGPS.Logic.BusinessRules.Log.PublicarTrace(string.Format("{0}", requestUrl), "RJ");
            try
            {
              
                requestUrl = requestUrl + "?empresa=1&data=" + data.ToString("yyMMdd");//"171101";
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Timeout = Parameters.TIMEOUT;               
                request.Headers.Add("Authorization", "Token token=e1bb21633b1458200ce728f546325e7a");

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;             
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());           

                return xmlDoc;
            }
            catch (Exception e)
            {

                if (count == 5 || e.Message.Contains("O servidor remoto retornou um erro: (404)"))
                {
                    count = 5;
                    //var idTpErro = 1;
                    //var entity = new ErrosIntegracaoLog(DateTime.UtcNow, idTpErro, idCliente);
                    //entity.IDCliente = idCliente;
                    //entity.Metodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    //entity.Modulo = "Grades de Horarios";
                    //entity.idTpErro = idTpErro;
                    //entity.descricao = e.Message;
                    //entity.stacktrace = e.StackTrace;
                    //entity.DataHoraEvento = DateTime.UtcNow;
                    //entity.InnerException = e.InnerException != null ? e.InnerException.ToString() : "Inner Nula";

                    ////fazer chamada recursiva em caso de erro
                    ////Thread job = new Thread(
                    ////                unused => ErroIntegracao("Grades", requestUrl, entity)
                    ////                );
                    ////job.Start();

                    //try
                    //{
                    //    ErroIntegracao("Grades", requestUrl, entity);
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                }
                else if (!e.Message.Contains("timeout"))
                    Thread.Sleep(120000);


                return BaixaServicosItamarati(requestUrl, count + 1, data, idCliente);
            }
        }

        public static XmlDocument BaixaServicosUnesul(string requestUrl, int count, int idCliente)
        {
            if (count > 5)
                return null;

            NewsGPS.Logic.BusinessRules.Log.PublicarTrace(string.Format("{0}", requestUrl), "RJ");
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Timeout = Parameters.TIMEOUT;


                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());

                //try
                //{
                //    ExitoIntegracao("Grades", requestUrl, idCliente);
                //}
                //catch (Exception ex)
                //{

                //}

                return xmlDoc;
            }
            catch (Exception e)
            {

                if (count == 5 || e.Message.Contains("O servidor remoto retornou um erro: (404)"))
                {
                    count = 5;
                    //var idTpErro = 1;
                    //var entity = new ErrosIntegracaoLog(DateTime.UtcNow, idTpErro, idCliente);
                    //entity.IDCliente = idCliente;
                    //entity.Metodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    //entity.Modulo = "Grades de Horarios";
                    //entity.idTpErro = idTpErro;
                    //entity.descricao = e.Message;
                    //entity.stacktrace = e.StackTrace;
                    //entity.DataHoraEvento = DateTime.UtcNow;
                    //entity.InnerException = e.InnerException != null ? e.InnerException.ToString() : "Inner Nula";

                    //try
                    //{
                    //    ErroIntegracao("Grades", requestUrl, entity);
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                }
                else if (!e.Message.Contains("timeout"))
                    Thread.Sleep(120000);

                return BaixaServicosUnesul(requestUrl, count + 1, idCliente);
            }
        }

        public static XmlDocument BaixaServicosGMEmpresas(string requestUrl, int count, DateTime data, Codigo_Conexao model)
        {
            if (count > 4)
                return null;

            try
            {

                requestUrl = String.Format("{0}?codigo={1}&dataServico={2}", requestUrl, model.Codigo2.Trim(), data.ToString("yyMMdd"));
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Timeout = Parameters.TIMEOUT;

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());

                return xmlDoc;
            }
            catch (Exception e)
            {
                return BaixaServicosGMEmpresas(requestUrl, count + 1, data, model);
            }
        }

        private static void ErroIntegracao(string servico, string url, ErrosIntegracaoLog entity)
        {
            if (entity != null)
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

            var statusRep = new StatusRequestRepository();
            statusRep.Add(entity, true, url);
        }

        private static void ExitoIntegracao(string servico, string url, int idCliente)
        {
            var entity = new StatusLog(DateTime.UtcNow, 1, idCliente);
            entity.IDCliente = idCliente;
            entity.Modulo = "Grades de Horarios";
            entity.Metodo = "";
            entity.idTpErro = 1;
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
