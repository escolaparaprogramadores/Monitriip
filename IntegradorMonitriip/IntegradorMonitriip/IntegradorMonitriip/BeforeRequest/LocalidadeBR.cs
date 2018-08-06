using IntegradorRequestWeb.RequestWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IntegradorMonitriip.BeforeRequest
{
    public class LocalidadeBR : LocalidadeRW
    {
        public static XmlDocument BaixarLocalidades(string requestUrl, int idCliente)
        {
            try
            {
                string[] detalhe = new String[2];

                XmlDocument dados = BuscarDadosLocalidades(requestUrl, 1, idCliente);

                return dados;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
