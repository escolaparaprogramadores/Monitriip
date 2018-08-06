using IntegradorModel.Model;
using IntegradorMonitriip.BeforeRequest;
using IntegradorMonitriip.DataRepository;
using IntegradorRepository.LocalDatabase;
using IntegradorRepository.LocalDatabase.Repository.Entity;
using IntegradorRequestWeb.RequestWeb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Web.Script.Serialization;
using System.Xml;

namespace IntegradorMonitriip.Jobs
{
    public class ServicoJob : ServicoBR
    {
        private static List<int> execucaoServicos = new List<int>();












        //SERVIÇO PASSO 03
        public static void ProcessarServico(string url)
        {
            DateTime data = DateTime.UtcNow;
            try
            {
                var emp = new GetEmpresas();
                var conexoes = emp.getCodigosEmpresas();  // 03.1 RETORNA A LISTA DE EMPRESAS
                var count = -1;
                foreach (var con in conexoes)
                {
                    if (con.IDCliente != 7937 && con.IDCliente != 15461 && con.IDCliente != 15460 && con.IDCliente != 15506 && con.IDCliente != 1582)  // 03.2 ENTRA NO IF CASO A EMPRESA NÃO SEJA DA RJ
                    {
                        if (isAtivo(con.IDCliente))
                        {
                            count++;
                            Thread job =
                                        new Thread(
                                        unused => jobServico(data, con, url)  // 03.03 FAZ A CHAMADA DO SERVICO
                                        );                           
                            job.Start();
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }




        //SERVIÇO PASSO 04
        static void jobServico(DateTime data, Codigo_Conexao item, string url)
        {
            int dias = 7;

            for (int i = 0; i < dias; i++)
            {

                try
                {

                    url = getUrlBase(item.IDCliente, url);

                    string requestUrl = "";
                    if (item.IDCliente == 1581)
                        requestUrl = string.Format(Parameters.BUSCA_SERVICO_VIACAO_OURO_PRATA, url, 297, item.Codigo2.Trim(), data.AddDays(i).ToString("yyMMdd"));
                    else
                        requestUrl = string.Format(Parameters.BUSCA_SERVICO, url, item.Codigo1.Trim(), item.Codigo2.Trim(), data.AddDays(i).ToString("yyMMdd"));

                    XmlDocument Servicos;
                    try
                    {
                        Servicos = BaixaServicos(requestUrl, item.IDCliente);
                    }
                    catch
                    {
                        Servicos = null;
                    }

                    try
                    {
                        var processXML = new ServicoPX();
                      var servicos = processXML.TratarRetorno(Servicos, item.IDCliente);

                        if (servicos != null)
                        {
                            var repository = new PutServicos();

                            /*Analise para exportacao das grades*/
                            //var jsonSerialiser = new JavaScriptSerializer();
                            //var json = "";//jsonSerialiser.Serialize(servicos);                    
                            //json = "{lista=" + JsonConvert.SerializeObject(servicos, Newtonsoft.Json.Formatting.Indented) + "}";
                            //Console.WriteLine(json);

                            repository.salvarGrades(servicos, item.IDCliente);

                            Parameters.DataUltimaImportacao = data;
                        }

                    }
                    catch (Exception e)
                    { }
                }catch(Exception ex)
                {

                }
            }

            execucaoServicos.Remove(item.IDCliente);
        }



        //SERVIÇO PASSO 05
        public static string getUrlBase(int idCliente, string url)
        {
            //3   EXPRESSO UNIÃO LTDA-- 7938
            //8   VIACAO PIRACICABANA LTDA--7940
            //66  EMPRESA PRINCESA DO NORTE S.A. --7941
            //67  EMPRESA CRUZ DE TRANSPORTES LTDA--7943
            //98  EMPRESA AUTO ONIBUS MANOEL RODRIGUES S.A. --7942
            //    Empresa Viação Ouro e Prata -- 1581 
            var retorno = "";
            switch (idCliente)
            {
               
                case 18568:
                    retorno = "http://vmgliese:9690/";
                    break;
                case 3847:
                    retorno = "http://newsgps.expnordeste.com.br:9991/";
                    break;
                case 143:
                    retorno = "http://sistema.andorinha.com:9992/";
                    break;
                case 136:
                case 8161:
                case 8162:
                    retorno = "http://passaromarron.com.br:9991/";
                    break;
                case 7938:
                case 7940:
                case 7941:
                case 7943:
                case 7942:
                    retorno = "http://186.234.232.4:8080/";
                    break;
                case 1581:
                    retorno = "http://200.213.3.162/";
                    break;
                default:
                    retorno = url;
                    break;
            }
            return retorno;
        }








        public static void ProcessaServicoUnesul()
        {
            DateTime data = DateTime.UtcNow;
            try
            {
                Thread Unesul =
                                   new Thread(
                                   unused => jobUnesul(data)
                                   );
                Unesul.Start();
            }
            catch (Exception ex)
            {

            }
        }

        public static void ProcessarServicoItamarati()
        {

            DateTime data = DateTime.UtcNow;

            try
            {
                Thread Itamarati =
                                  new Thread(
                                  unused => jobItamarati(data)
                                  );
                Itamarati.Start();
            }
            catch (Exception ex)
            {

            }

        }
        static List<string> makeUrlArray(int count)
        {
            var lista = new List<string>();
            lista.Add(Parameters.URL_SERVICO);

            for (var i = 1; i < count; i++)
            {
                if (i % 2 == 0)
                    lista.Add(Parameters.URL_SERVICO);
                else
                    lista.Add(Parameters.URL_SERVICO2);
            }

            return lista;
        }

        static void jobUnesul(DateTime data)
        {

            var idUnesul = Parameters.IDENTIFICADOR_UNESUL;
            if (isAtivo(idUnesul))
            {
                int dias = 7;

                for (int i = 0; i < dias; i++)
                {
                    string requestUrl = string.Format(Parameters.BUSCA_SERVICO_UNESUL, Parameters.CODIGO_EMPRESA_UNESUL, data.AddDays(i).ToString("yyMMdd"));

                    string sSource;
                    string sLog;
                    string sEvent;

                    XmlDocument Servicos;
                    try
                    {
                        Servicos = BaixaServicosUnesul(requestUrl, idUnesul);

                    }
                    catch
                    {
                        Servicos = null;
                    }

                    try
                    {
                        var processXML = new ServicoPX();
                        var servicos = processXML.TratarRetornoPlacaMotorista(Servicos);

                        if (servicos != null)
                        {
                            var repository = new PutServicos();

                            //sSource = "Integrador de dados";
                            //sLog = "Application";
                            //sEvent = "pegou as grades. qtd =" + servicos.Count.ToString();
                            //if (!EventLog.SourceExists(sSource))
                            //    EventLog.CreateEventSource(sSource, sLog);
                            //EventLog.WriteEntry(sSource, sEvent);
                            //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);

                            repository.salvarGrades(servicos, idUnesul);
                        }
                    }
                    catch (Exception ex)
                    {
                        sSource = "Integrador Serviços";
                        sLog = "Application";
                        sEvent = "Cliente: " + idUnesul + " Erro saindo da grade: " + ex.Message + " /n  Inner:" + ex.InnerException + " /n StackTrace" + ex.StackTrace;
                        if (!EventLog.SourceExists(sSource))
                            EventLog.CreateEventSource(sSource, sLog);
                        EventLog.WriteEntry(sSource, sEvent);
                        EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 777);

                    }
                }

                execucaoServicos.Remove(idUnesul);
            }
        }

        static bool isAtivo(int id)
        {
            if (!execucaoServicos.Contains(id))
            {
                execucaoServicos.Add(id);
                return true;
            }
            else
            {
                return false;
            }
        }

      

















        static void jobItamarati(DateTime data)
        {
            var idItamarati = Parameters.IDENTIFICADOR_ITAMARATI;
            if (isAtivo(idItamarati))
            {
                int dias = 7;

                for (int i = 0; i < dias; i++)
                {
                    var requestUrl = Parameters.BUSCA_SERVICO_ITAMARATI;
                    //string requestUrl = string.Format(Parameters.BUSCA_SERVICO_ITAPEMIRIM, 1, data.AddDays(i).ToString("yyMMdd"));

                    XmlDocument Servicos;
                    try
                    {
                        Servicos = BaixaServicosItamarati(requestUrl, data.AddDays(i), 7937);
                    }
                    catch
                    {
                        Servicos = null;
                    }

                    try
                    {
                        var processXML = new ServicoPX();
                        var servicos = processXML.TratarRetornoPlacaMotorista(Servicos);

                        if (servicos != null)
                        {

                            /*Analise para exportacao das grades*/
                            //var jsonserialiser = new JavaScriptSerializer();
                            //var json = "";//jsonserialiser.serialize(servicos);                    
                            //json = "{lista=" + JsonConvert.SerializeObject(servicos, Newtonsoft.Json.Formatting.Indented) + "}";
                            //Console.WriteLine(json);

                            var repository = new PutServicos();
                            repository.salvarGrades(servicos, Parameters.IDENTIFICADOR_ITAMARATI);

                            Parameters.DataUltimaImportacao = data;
                        }
                    }
                    catch (Exception e)
                    { }
                }
                execucaoServicos.Remove(idItamarati);
            }

        }

        public static void ProcessarServicoGM()
        {

            DateTime data = DateTime.UtcNow;

            try
            {

                var emp = new GetEmpresas();
                var conexoes = emp.getCodigosEmpresasGM();

                foreach (var con in conexoes)
                {
                    var count = 0;

                    if(count == 0)
                    {
                        Thread job =
                            new Thread(
                            unused => jobGM(data, con)
                            );
                        job.Start();
                    }

                    count++;
                }

            }
            catch (Exception ex)
            {

            }
        }


        static void jobGM(DateTime data, Codigo_Conexao model)
        {
            var idGM = model.IDCliente;
            if (isAtivo(idGM))
            {
                int dias = 8;

                for (int i = 0; i < dias; i++)
                {
                    var requestUrl = Parameters.BUSCA_SERVICO_GM;
                    //string requestUrl = string.Format(Parameters.BUSCA_SERVICO_ITAPEMIRIM, 1, data.AddDays(i).ToString("yyMMdd"));

                    XmlDocument Servicos;
                    try
                    {
                        Servicos = BaixaServicosGM(requestUrl, data.AddDays(i), model);
                    }
                    catch
                    {
                        Servicos = null;
                    }

                    try
                    {
                        var processXML = new ServicoPX();                        
                        var servicos = processXML.TratarRetornoPlacaMotoristaGM(Servicos);

                        if (servicos != null && servicos.Count > 0)
                        {

                            /*Analise para exportacao das grades*/
                            //var jsonserialiser = new JavaScriptSerializer();
                            //var json = "";//jsonserialiser.serialize(servicos);                    
                            //json = "{lista=" + JsonConvert.SerializeObject(servicos, Newtonsoft.Json.Formatting.Indented) + "}";
                            //Console.WriteLine(json);

                            var repository = new PutServicos();
                            repository.salvarGrades(servicos, idGM);

                            Parameters.DataUltimaImportacao = data;
                        }
                    }
                    catch (Exception e)
                    { }
                }
                execucaoServicos.Remove(idGM);
            }

        }
    }
}
