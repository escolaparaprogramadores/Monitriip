using EucaturIntegrador.BeforeRequest;
using EucaturIntegrador.ProcessJson;
using IntegradorMonitriip.DataRepository;
using IntegradorRepository.LocalDatabase;
using IntegradorRequestWeb.RequestWeb;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EucaturIntegrador.Jobs
{
    public class ServicoEucaturJob : ServicoBR
    {
        public static void ProcessarServico(string url)
        {
            DateTime data = DateTime.UtcNow;
            //var emp = new GetEmpresas();
            //var conexoes = emp.getCodigosEmpresas();

            //foreach (var con in conexoes)
            //{
                Thread job =
                  new Thread(
                    unused => jobServico(data, url)
                  );
                job.Start();
            //}
        }

        static void jobServico(DateTime data, string url)
        {
            int dias = 5;
            //string metodo = "/empresas";
            string metodo = "/viagens?";
            string empresa = "&empresa=00001";
            
            List<JToken> viagens;
            var idEucatur = 270;
            //"viagens_resumidas": "https://monitriip.eucatur.com.br/v1/viagens?data=2017-02-23&empresa=00001"

            for (int i = 0; i < dias; i++)
            {
                string sdata = "data=" + data.AddDays(i).Date.ToString("yyyy-MM-dd");
                string metodoComposto = metodo + sdata + empresa;
                string requestUrl = string.Format(Parameters.EUCATUR_URL_MONITRIIP, metodoComposto);

                try
                {
                    viagens = BaixaServicosEucatur(requestUrl);
                }
                catch
                {
                    viagens = null;
                }

                try
                {
                    var servicos = ProcessJS.processJson(viagens);

                    if (servicos != null)
                    {
                        var repository = new PutServicos();
                        repository.salvarGrades(servicos, idEucatur);
                    }
                }
                catch (Exception e)
                { }
                Parameters.DataUltimaImportacao = data;
            }
        }
    }
}
