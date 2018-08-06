using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace EucaturIntegrador
{
    public static class RequestWeb1
    {
        //public static void BuscarDadosPassagens()
        //{
        //    try
        //    {
                
        //        string data = "2017-01-04";
        //        string codEmpresa = "00001";

        //        string parametros = "?data=" + data + "&empresa=" + codEmpresa;
        //        string urlComposta = URL_MONITRIIP + metodo2 + parametros;
        //        string urlComposta2 = URL_MONITRIIP + metodo;
        //        //HttpWebRequest request = WebRequest.Create(URL_MONITRIIP + metodo) as HttpWebRequest;
        //        HttpWebRequest request = 
        //            WebRequest.Create(urlComposta2) as HttpWebRequest;
        //        request.Timeout = 30000;

        //        //string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(Parameters.USERNAME + ":" + Parameters.PASSWORD));
        //        request.Headers.Add("X-Eucatur-Api-Id", "0001");
        //        request.Headers.Add("X-Eucatur-Api-Key", "D37DFE7A-BD4C-11E6-B32B-40F2E92DC7AA");

        //        HttpWebResponse response = request.GetResponse() as HttpWebResponse;

        //        var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();

        //        var Js = JsonConvert.DeserializeObject(rawJson);

        //        //var json = converterJson(rawJson);
        //        //XmlDocument xmlDoc = new XmlDocument();
        //        //xmlDoc.Load(response.GetResponseStream());
        //        //var retorno = response.GetResponseStream();
        //        //Parameters.DataUltimaIntegracao = DateTime.UtcNow.AddMinutes(-1);
        //        //return xmlDoc;

        //    }
        //    catch (Exception e)
        //    {
        //        //NewsGPS.Logic.BusinessRules.Log.PublicarTrace(string.Format("{0} - {1}", requestUrl, e.Message), "RJ");
        //        //return BuscarDadosPassagens(requestUrl, count + 1);
        //    }

        //}

        //public static string converterJson(object obj)
        //{
        //    return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        //}

    }
}
