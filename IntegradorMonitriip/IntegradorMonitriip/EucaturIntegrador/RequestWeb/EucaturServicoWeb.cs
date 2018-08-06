using IntegradorRequestWeb.RequestWeb;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EucaturIntegrador.RequestWeb
{
    public class EucaturServicoWeb
    {
        public static List<JToken> BuscaDadosEucaturServico(string requestUrl, int count)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                //HttpWebRequest request = WebRequest.Create("https://monitriip.eucatur.com.br/v1/viagens/1411984") as HttpWebRequest;
                //HttpWebRequest request = WebRequest.Create("https://monitriip.eucatur.com.br/v1/viagens?data=2017-02-23&empresa=00053") as HttpWebRequest;
                //"viagens_resumidas": "https://monitriip.eucatur.com.br/v1/viagens?data=2017-02-23&empresa=00001"
                request.Timeout = Parameters.TIMEOUT;

                //string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(Parameters.USERNAME + ":" + Parameters.PASSWORD));
                request.Headers.Add("X-Eucatur-Api-Id", "0001");
                request.Headers.Add("X-Eucatur-Api-Key", "D37DFE7A-BD4C-11E6-B32B-40F2E92DC7AA");

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();

                var js = JsonConvert.DeserializeObject(rawJson);

                JArray arrayJson = (JArray)js;
                //Newtonsoft.Json.Linq.JArray array = new Newtonsoft.Json.Linq.JArray();

                List<JToken> lista = new List<JToken>();

                foreach (JToken item in arrayJson)
                {
                    //lista.Add(item.ElementAt(0).ElementAt(0).ToString());
                    lista.Add(item);
                }

                return lista;
            }
            catch (Exception e)
            {
                //fazer chamada recursiva em caso de erro
                //NewsGPS.Logic.BusinessRules.Log.PublicarTrace(string.Format("{0} - {1}", requestUrl, e.Message), "EUCATUR");
                return BuscaDadosEucaturServico(requestUrl, count + 1);
            }
        }
    }
}
