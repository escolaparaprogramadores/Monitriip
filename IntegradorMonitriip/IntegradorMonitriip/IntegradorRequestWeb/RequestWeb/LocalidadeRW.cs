using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IntegradorRequestWeb.RequestWeb
{
    public class LocalidadeRW
    {
        public static XmlDocument BuscarDadosLocalidades(string requestUrl, int count, int idCliente)
        {
            if (count > 4)
                return null;

            NewsGPS.Logic.BusinessRules.Log.PublicarTrace(string.Format("{0}", requestUrl), "RJ");
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Timeout = Parameters.TIMEOUT;

                string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(Parameters.USERNAME + ":" + Parameters.PASSWORD));
                request.Headers.Add("Authorization", "Basic " + encoded);

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());

                return xmlDoc;
            }
            catch (Exception e)
            {
                return BuscarDadosLocalidades(requestUrl, count + 1, idCliente);
            }
        }
    }
}
