using EucaturIntegrador.RequestWeb;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace EucaturIntegrador.BeforeRequest
{
    public class ServicoBR : EucaturServicoWeb
    {
        public static List<JToken> BaixaServicosEucatur(string requestUrl)
        {
            try
            {
                string[] detalhe = new String[2];

                var dados = BuscaDadosEucaturServico(requestUrl, 1);

                return dados;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
    }
}