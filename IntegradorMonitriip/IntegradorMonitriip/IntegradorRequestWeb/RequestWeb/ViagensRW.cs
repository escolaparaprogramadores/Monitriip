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
    public class ViagensRW
    {
        public static resultadoOperacao EnviaANTTWebInicioFimViagemRegular(inicioFimViagemRegular x, int idCliente)
        {
            var tkn = Parameters.TOKEN_PROD_ANTT;
            var tokenZoomSat = Parameters.TOKEN_ZOOMSAT;
            var tokenSmart = Parameters.TOKEN_SMART;
            var tokenAgadelha = Parameters.TOKEN_Agadelha;
            var tokenOrionSat = Parameters.TOKEN_ORIONSAT_HOMOLOG;

            if (idCliente == 7955)
            {
                tkn = tokenSmart;
            }
            else if (idCliente == 695)
            {
                tkn = tokenAgadelha;
            }
            else if (idCliente == 17867)
            {
                tkn = tokenOrionSat;
            }            
            else
            {
                var instancy = new GetEmpresas();
                var isZoom = instancy.isZoomSat(idCliente);

                if (isZoom)
                    tkn = tokenZoomSat;
            }


            while (true)
            {

                resultadoOperacao ret = new resultadoOperacao();

                try
                {
                    using (var client = new HttpClient())
                    {

                        client.BaseAddress = new Uri(Parameters.URL_ANTT_LOG);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tkn);
                        client.Timeout = new TimeSpan(0, 0, 0, 10);

                        var task = System.Threading.Tasks.Task.Run<antt.gov.br.monitriip.v1._0.resultadoOperacao>(() =>
                        {

                            var res = client.PostAsXmlAsync<antt.gov.br.monitriip.v1._0.inicioFimViagemRegular>("InserirLogInicioFimViagemRegular", x);
                            if (res.Result.IsSuccessStatusCode)
                            {
                                return res.Result.Content.ReadAsAsync<antt.gov.br.monitriip.v1._0.resultadoOperacao>();
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

                    erro e = new erro() { descricao = ex.InnerException.Message != null ? ex.InnerException.Message.Replace("'", "").Replace("'", "") : null };
                    erro[] erro = new erro[1];
                    erro[0] = e;
                    ret.erros = erro;
                    ret.mensagem = "Erro de validação";
                    return ret;
                    //try
                    //{
                    //    erro[] erro = new erro[] {
                    //        new erro() {
                    //        descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                    //        }};
                    //    return new resultadoOperacao() { erros = erro };
                    //}
                    //catch (Exception e)
                    //{
                    //    erro[] erro = new erro[] {
                    //            new erro() {
                    //            descricao = "Mensagem: Erro no envio logs para ANTT"
                    //            }};
                    //    return new resultadoOperacao() { erros = erro };
                    //                   
                }
            }
        }

        public static resultadoOperacao EnviaANTTWebInicioFimViagemFretado(inicioFimViagemFretado x, int idCliente)
        {
            var tkn = Parameters.TOKEN_PROD_ANTT;
            var tokenZoomSat = Parameters.TOKEN_ZOOMSAT;
            var tokenSmart = Parameters.TOKEN_SMART;
            var tokenAgadelha = Parameters.TOKEN_Agadelha;
            var tokenOrionSat = Parameters.TOKEN_ORIONSAT_HOMOLOG;

            if (idCliente == 7955)
            {
                tkn = tokenSmart;
            }
            else if (idCliente == 695)
            {
                tkn = tokenAgadelha;
            }
            else if (idCliente == 17867)
            {
                tkn = tokenOrionSat;
            }
            else
            {
                var instancy = new GetEmpresas();
                var isZoom = instancy.isZoomSat(idCliente);

                if (isZoom)
                    tkn = tokenZoomSat;
            }


            while (true)
            {

                resultadoOperacao ret = new resultadoOperacao();

                try
                {
                    using (var client = new HttpClient())
                    {

                        client.BaseAddress = new Uri(Parameters.URL_ANTT);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tkn);
                        client.Timeout = new TimeSpan(0, 0, 0, 10);

                        var task = System.Threading.Tasks.Task.Run<antt.gov.br.monitriip.v1._0.resultadoOperacao>(() =>
                        {
                            var res = client.PostAsXmlAsync<antt.gov.br.monitriip.v1._0.inicioFimViagemFretado>("InserirLogInicioFimViagemFretado", x);
                            if (res.Result.IsSuccessStatusCode)
                            {
                                return res.Result.Content.ReadAsAsync<antt.gov.br.monitriip.v1._0.resultadoOperacao>();
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
                        erro e = new erro() { descricao = ex.InnerException.Message != null ? ex.InnerException.Message.Replace("'", "").Replace("'", "") : null };
                        erro[] erro = new erro[1];
                        erro[0] = e;
                        ret.erros = erro;
                        ret.mensagem = "Erro de validação";
                        return ret;

                    }
                    catch
                    {
                        continue;
                    }

                    //try
                    //{
                    //    erro[] erro = new erro[] {
                    //        new erro() {
                    //        descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                    //        }};
                    //    return new resultadoOperacao() { erros = erro };
                    //}
                    //catch (Exception e)
                    //{
                    //    erro[] erro = new erro[] {
                    //            new erro() {
                    //            descricao = "Mensagem: Erro no envio logs para ANTT"
                    //            }};
                    //    return new resultadoOperacao() { erros = erro };
                    //}
                    
                }
            }
        }

        public static resultadoOperacao EnviaJornadaMotorista(jornadaTrabalhoMotorista x, int idCliente)
        {
            var tkn = Parameters.TOKEN_PROD_ANTT;
            var tokenZoomSat = Parameters.TOKEN_ZOOMSAT;
            var tokenSmart = Parameters.TOKEN_SMART;
            var tokenAgadelha = Parameters.TOKEN_Agadelha;
            var tokenOrionSat = Parameters.TOKEN_ORIONSAT_HOMOLOG;

            if (idCliente == 7955)
            {
                tkn = tokenSmart;
            }
            else if (idCliente == 695)
            {
                tkn = tokenAgadelha;
            }
            else if (idCliente == 17867)
            {
                tkn = tokenOrionSat;
            }
            else
            {
                var instancy = new GetEmpresas();
                var isZoom = instancy.isZoomSat(idCliente);

                if (isZoom)
                    tkn = tokenZoomSat;
            }


            while (true)
            {

                resultadoOperacao ret = new resultadoOperacao();

                try
                {
                    using (var client = new HttpClient())
                    {

                        client.BaseAddress = new Uri(Parameters.URL_ANTT_LOG);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tkn);
                        client.Timeout = new TimeSpan(0, 0, 0, 10);

                        var task = System.Threading.Tasks.Task.Run<antt.gov.br.monitriip.v1._0.resultadoOperacao>(() =>
                        {

                            var res = client.PostAsXmlAsync<antt.gov.br.monitriip.v1._0.jornadaTrabalhoMotorista>("InserirLogJornadaTrabalhoMotorista", x);

                            return res.Result.Content.ReadAsAsync<antt.gov.br.monitriip.v1._0.resultadoOperacao>();

                        });

                        task.Wait();
                        ret = task.Result;
                        return ret;

                    }
                }
                catch (Exception ex)
                {
                    //try
                    //{
                    //    erro[] erro = new erro[] {
                    //        new erro() {
                    //        descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                    //        }};
                    //    return new resultadoOperacao() { erros = erro };
                    //}
                    //catch (Exception e)
                    //{
                    //    erro[] erro = new erro[] {
                    //            new erro() {
                    //            descricao = "Mensagem: Erro no envio logs para ANTT"
                    //            }};
                    //    return new resultadoOperacao() { erros = erro };
                    //}
                    continue;
                }
            }
        }

        public static resultadoOperacao EnviaDetectorParada(detectorParada x, int idCliente)
        {
            var tkn = Parameters.TOKEN_PROD_ANTT;
            var tokenZoomSat = Parameters.TOKEN_ZOOMSAT;
            var tokenSmart = Parameters.TOKEN_SMART;
            var tokenAgadelha = Parameters.TOKEN_Agadelha;
            var tokenOrionSat = Parameters.TOKEN_ORIONSAT_HOMOLOG;

            if (idCliente == 7955)
            {
                tkn = tokenSmart;
            }
            else if (idCliente == 695)
            {
                tkn = tokenAgadelha;
            }
            else if (idCliente == 17867)
            {
                tkn = tokenOrionSat;
            }
            else
            {
                var instancy = new GetEmpresas();
                var isZoom = instancy.isZoomSat(idCliente);

                if (isZoom)
                    tkn = tokenZoomSat;
            }


            while (true)
            {

                resultadoOperacao ret = new resultadoOperacao();

                try
                {
                    using (var client = new HttpClient())
                    {

                        client.BaseAddress = new Uri(Parameters.URL_ANTT_LOG);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tkn);
                        client.Timeout = new TimeSpan(0, 0, 0, 10);

                        var task = System.Threading.Tasks.Task.Run<antt.gov.br.monitriip.v1._0.resultadoOperacao>(() =>
                        {

                            var res = client.PostAsXmlAsync<antt.gov.br.monitriip.v1._0.detectorParada>("InserirLogDetectorParada", x);
                            if (res.Result.IsSuccessStatusCode)
                            {
                                return res.Result.Content.ReadAsAsync<antt.gov.br.monitriip.v1._0.resultadoOperacao>();
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
                    //try
                    //{
                    //    erro[] erro = new erro[] {
                    //        new erro() {
                    //        descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                    //        }};
                    //    return new resultadoOperacao() { erros = erro };
                    //}
                    //catch (Exception e)
                    //{
                    //    erro[] erro = new erro[] {
                    //            new erro() {
                    //            descricao = "Mensagem: Erro no envio logs para ANTT"
                    //            }};
                    //    return new resultadoOperacao() { erros = erro };
                    //}
                    continue;
                }
            }
        }

        public static resultadoOperacao EnviaVelocidadeTempoLocalizacao(velocidadeTempoLocalizacao x, int idCliente)
        {
            var tkn = Parameters.TOKEN_PROD_ANTT;
            var tokenZoomSat = Parameters.TOKEN_ZOOMSAT;
            var tokenSmart = Parameters.TOKEN_SMART;
            var tokenAgadelha = Parameters.TOKEN_Agadelha;
            var tokenOrionSat = Parameters.TOKEN_ORIONSAT_HOMOLOG;

            if (idCliente == 7955)
            {
                tkn = tokenSmart;
            }
            else if (idCliente == 695)
            {
                tkn = tokenAgadelha;
            }
            else if (idCliente == 17867)
            {
                tkn = tokenOrionSat;
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

                    client.BaseAddress = new Uri(Parameters.URL_ANTT_LOG);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tkn);
                    client.Timeout = new TimeSpan(0, 0, 0, 10);

                    var task = System.Threading.Tasks.Task.Run<antt.gov.br.monitriip.v1._0.resultadoOperacao>(() =>
                    {

                        var res = client.PostAsXmlAsync<antt.gov.br.monitriip.v1._0.velocidadeTempoLocalizacao>("InserirLogVelocidadeTempoLocalizacao", x);
                        if (res.Result.IsSuccessStatusCode)
                        {
                            return res.Result.Content.ReadAsAsync<antt.gov.br.monitriip.v1._0.resultadoOperacao>();
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
                //try
                //{
                //    erro[] erro = new erro[] {
                //        new erro() {
                //        descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                //        }};
                //    return new resultadoOperacao() { erros = erro };
                //}
                //catch (Exception e)
                //{
                //    erro[] erro = new erro[] {
                //            new erro() {
                //            descricao = "Mensagem: Erro no envio logs para ANTT"
                //            }};
                //    return new resultadoOperacao() { erros = erro };
                //}
                return ret;
            }

        }

        public static resultadoOperacao EnviaBilheteEmbarque(bilheteEmbarque x, int idCliente)
        {
            var tkn = Parameters.TOKEN_PROD_ANTT;
            var tokenZoomSat = Parameters.TOKEN_ZOOMSAT;
            var tokenSmart = Parameters.TOKEN_SMART;
            var tokenAgadelha = Parameters.TOKEN_Agadelha;
            var tokenOrionSat = Parameters.TOKEN_ORIONSAT_HOMOLOG;

            if (idCliente == 7955)
            {
                tkn = tokenSmart;
            }
            else if (idCliente == 695)
            {
                tkn = tokenAgadelha;
            }
            else if (idCliente == 17867)
            {
                tkn = tokenOrionSat;
            }
            else
            {
                var instancy = new GetEmpresas();
                var isZoom = instancy.isZoomSat(idCliente);

                if (isZoom)
                    tkn = tokenZoomSat;
            }


            while (true)
            {

                resultadoOperacao ret = new resultadoOperacao();

                try
                {
                    using (var client = new HttpClient())
                    {

                        client.BaseAddress = new Uri(Parameters.URL_ANTT_LOG);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tkn);
                        client.Timeout = new TimeSpan(0, 0, 0, 10);

                        var task = System.Threading.Tasks.Task.Run<antt.gov.br.monitriip.v1._0.resultadoOperacao>(() =>
                        {

                            var res = client.PostAsXmlAsync<antt.gov.br.monitriip.v1._0.bilheteEmbarque>("InserirLogBilheteEmbarque", x);
                            if (res.Result.IsSuccessStatusCode)
                            {
                                return res.Result.Content.ReadAsAsync<antt.gov.br.monitriip.v1._0.resultadoOperacao>();
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
                    //try
                    //{
                    //    erro[] erro = new erro[] {
                    //        new erro() {
                    //        descricao = "Mensagem: " + ex.Message + " Erro: " + ex.ToString()
                    //        }};
                    //    return new resultadoOperacao() { erros = erro };
                    //}
                    //catch (Exception e)
                    //{
                    //    erro[] erro = new erro[] {
                    //            new erro() {
                    //            descricao = "Mensagem: Erro no envio logs para ANTT"
                    //            }};
                    //    return new resultadoOperacao() { erros = erro };
                    //}
                    continue;
                }
            }
        }
    }
}
