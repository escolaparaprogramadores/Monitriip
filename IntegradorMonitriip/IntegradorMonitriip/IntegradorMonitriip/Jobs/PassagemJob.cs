using System;
using System.Xml;
using System.Xml.Serialization;
using IntegradorMonitriip.DataRepository;
using IntegradorMonitriip.BeforeRequest;
using System.Collections.Generic;
using antt.gov.br.monitriip.v1._0;
using IntegradorRequestWeb.RequestWeb;
using IntegradorRepositoryAzure.AzureTables;
using IntegradorRepository.LocalDatabase;
using System.Threading;
using IntegradorModel.Model.XmlModel;
using IntegradorModel.Model;
using IntegradorRepository.LocalDatabase.Repository.Entity;
using System.Linq;
using IntegradorModel.ProcessXml;

namespace IntegradorMonitriip.Jobs
{
    public class PassagemJob : VendasBR
    {
        private static List<int> execucaoVendas = new List<int>();
        public static void ProcessarPassagens(string url)
        {
            //var emp = new GetEmpresas();
            //var conexoes = emp.getCodigosEmpresas();
            //DateTime data = DateTime.UtcNow;

            //#region Bilhetes
            //foreach (var item in conexoes)
            //{
            //    Thread job =
            //      new Thread(
            //        unused => jobBilhetes(item, url)
            //      );
            //    job.Start();
            //}
            //#endregion

            //Parameters.DataUltimaIntegracao = data;
        }

        public static void ProcessarVendas(string url)
        {
            DateTime data = DateTime.UtcNow.AddHours(-3);
            var emp = new GetEmpresas();
            var conexoes = emp.getCodigosEmpresas();
            var count = -1;

            foreach (var item in conexoes)
            {
                
                if (item.IDCliente != 7937 && item.IDCliente != 15461 && item.IDCliente != 15460 && item.IDCliente != 15506)
                {
                    if (isAtivo(item.IDCliente))
                    {
                        count++;
                        Thread job =
                      new Thread(
                        unused => jobVendas(item, url, Parameters.DataUltimaIntegracaoVendas)
                      );
                        job.Start();

                    }
                }
            }


            Parameters.DataUltimaIntegracaoVendas = data;
        }

        public static void ProcessarVendasItamarati(string url)
        {
            DateTime data = DateTime.UtcNow.AddHours(-3);


            Thread Itamarati =
                              new Thread(
                              unused => jobVendasItamarati(data)
                              );
            Itamarati.Start();


            Parameters.DataUltimaIntegracaoItamarati = data;
        }

        public static void ProcessarVendasGM()
        {
            DateTime data = DateTime.UtcNow.AddHours(-3);
            var emp = new GetEmpresas();
            var conexoes = emp.getCodigosEmpresasGM();

            foreach (var item in conexoes)
            {
                Thread Itamarati =
                          new Thread(
                          unused => jobVendasGM(data, item)
                          );
                Itamarati.Start();
            }

            Parameters.DataUltimaIntegracaoItamarati = data;
        }

        public static void ProcessarVendasUnesul()
        {
            DateTime data = DateTime.Now;
            //var emp = new GetEmpresas();
            //var conexoes = emp.getCodigosEmpresas();

            var urls = "http://sistemas.unesul.com.br/monitriip/buscaBilhetes/1/715794/170721";
            var cliente = 1582;
            #region Vendas
            //foreach (var item in conexoes)
            //{
            //if (isAtivo(cliente))
            //{
            //    Thread job =
            //  new Thread(
            //    unused => jobVendasUnesul(cliente, urls, data)
            //  );
            //    job.Start();
            //}
            //}
            #endregion
            Parameters.DataUltimaIntegracaoVendas = data;
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
        public static string getUrlBase(int idCliente, string url)
        {
            var retorno = "";
            switch (idCliente)
            {
                case 18568:
                    retorno = "http://vmgliese:9690/";
                    break;
                case 8703:
                    retorno = "http://qslinuxrj.cloudapp.net:9991/";
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
                //3   EXPRESSO UNIÃO LTDA-- 7938
                //8   VIACAO PIRACICABANA LTDA--7940
                //66  EMPRESA PRINCESA DO NORTE S.A. --7941
                //67  EMPRESA CRUZ DE TRANSPORTES LTDA--7943
                //98  EMPRESA AUTO ONIBUS MANOEL RODRIGUES S.A. --7942
                case 7938:
                case 7940:
                case 7941:
                case 7943:
                case 7942:
                    retorno = "http://186.234.232.4:8080/";
                    break;
                default:
                    retorno = url;
                    break;
            }
            return retorno;
        }
        static bool isAtivo(int id)
        {
            if (!execucaoVendas.Contains(id))
            {
                execucaoVendas.Add(id);
                return true;
            }
            else
            {
                return false;
            }
        }

        static void jobVendas(Codigo_Conexao item, string url, DateTime data)
        {
            try
            {
                List<XmlDocument> arrayPassagens = new List<XmlDocument>();

                url = getUrlBase(item.IDCliente, url);

                XmlDocument Passagens = ProcessarPassagens(url, item.Codigo1.Trim(), item.Codigo2.Trim(), "",
                    !Parameters.inicializado
                    ? DateTime.UtcNow.AddHours(-3).AddMinutes(-179)
                    : data,
                    true, item.IDCliente);

                Parameters.inicializado = true;
                if (Passagens != null)
                {
                    var processXML = new VendasPX();
                    var passagemRJ = processXML.TratarRetorno(Passagens);

                    if (passagemRJ != null)
                    {
                        var repository = new PutVendas();
                        var passagensEnvio = new List<VendasModel>();

                        repository.salvarVendas(passagemRJ, item.IDCliente, ref passagensEnvio);

                        EnviaANTT(ref passagensEnvio);

                        repository.updateANTT(passagensEnvio);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            execucaoVendas.Remove(item.IDCliente);
        }

        static void jobVendasItamarati( DateTime data)
        {
            try
            {                

                List<XmlDocument> arrayPassagens = new List<XmlDocument>();

                //url = getUrlBase(Parameters.IDENTIFICADOR_ITAMARATI, url);
                var url = @"http://monitriip.expressoitamarati.com.br/buscaVendas?empresa=1&data="; // 161101";

                //"http://monitriip.expressoitamarati.com.br/buscaVendas?empresa=1&data=180110&hora=090000"
                data = !Parameters.inicializado1
                    ? DateTime.UtcNow.AddHours(-3).AddHours(-3)
                    : data;

                url = url + data.ToString("yyMMdd");
                url = url + "&hora=" + data.ToString("HHmmss");
                XmlDocument Passagens = ProcessarPassagensItamarati(url);

                

                Parameters.inicializado1 = true;
                if (Passagens != null)
                {
                    var processXML = new VendasPX();
                    var passagemRJ = processXML.TratarRetornoItamarati(Passagens);

                    if (passagemRJ != null && passagemRJ.passagens.Count > 0)
                    {
                        var repository = new PutVendas();
                        var passagensEnvio = new List<VendasModel>();

                        repository.salvarVendas(passagemRJ, Parameters.IDENTIFICADOR_ITAMARATI, ref passagensEnvio);

                        EnviaANTT(ref passagensEnvio);

                        repository.updateANTT(passagensEnvio);
                    }
                }
                Parameters.DataUltimaIntegracaoItamarati = data.AddMinutes(-1);
            }
            catch (Exception ex)
            {
            }

            execucaoVendas.Remove(Parameters.IDENTIFICADOR_ITAMARATI);
        }

        static void jobVendasGM(DateTime data, Codigo_Conexao item)
        {
            try
            {

                List<XmlDocument> arrayPassagens = new List<XmlDocument>();

                var url = Parameters.BUSCA_VENDAS_GM;
             
                data = !Parameters.inicializado1
                    ? DateTime.UtcNow.AddHours(-3).AddHours(-3)
                    : data;         

                url = String.Format("{0}?codigo1={1}&codigo2={2}&dataBase={3}&horaBase={4}", url, item.Codigo2.Trim(), item.Codigo2.Trim(), data.ToString("yyMMdd"),
                     data.ToString("HHmm"));
            
                XmlDocument Passagens = BuscarDadosPassagens(url, 1, item.IDCliente);

                Parameters.inicializado1 = true;
                if (Passagens != null)
                {
                    var processXML = new VendasPX();
                    var passagemRJ = processXML.TratarRetornoGM(Passagens);

                    if (passagemRJ != null && passagemRJ.passagens.Count > 0)
                    {
                        var repository = new PutVendas();
                        var passagensEnvio = new List<VendasModel>();

                        repository.salvarVendas(passagemRJ, item.IDCliente, ref passagensEnvio);

                        EnviaANTT(ref passagensEnvio);

                        repository.updateANTT(passagensEnvio);
                    }
                }
                Parameters.DataUltimaIntegracaoVendas = data;
            }
            catch (Exception ex)
            {
            }

            execucaoVendas.Remove(item.IDCliente);
        }

        //static void jobVendasUnesul(int cliente, string url, DateTime data)
        //{
        //    try
        //    {
        //        List<XmlDocument> arrayPassagens = new List<XmlDocument>();

        //        //if (item.IDCliente == 143 || item.IDCliente == 3847)
        //        //url = getUrlBase(item.IDCliente, url);

        //        XmlDocument Passagens = ProcessarPassagens(url, item.Codigo1.Trim(), item.Codigo2.Trim(), "", Parameters.DataUltimaIntegracao == default(DateTime) ? data.AddMinutes(-230) : Parameters.DataUltimaIntegracao, true, item.IDCliente);
        //        if (Passagens != null)
        //        {
        //            var xmlReader = new XmlNodeReader(Passagens);

        //            vendas passagemRJ = null;

        //            XmlSerializer serializer = new XmlSerializer(typeof(vendas));
        //            passagemRJ = (vendas)serializer.Deserialize(xmlReader);

        //            var repository = new PutVendas();
        //            var passagensEnvio = new List<VendasModel>();

        //            repository.salvarVendas(passagemRJ, item.IDCliente, ref passagensEnvio);

        //            EnviaANTT(ref passagensEnvio);

        //            repository.updateANTT(passagensEnvio);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    execucaoVendas.Remove(item.IDCliente);
        //}
    }

}
