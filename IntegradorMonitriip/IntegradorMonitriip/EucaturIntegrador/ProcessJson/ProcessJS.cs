using NewsGPS.Contracts.DTO.RJ;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EucaturIntegrador.ProcessJson
{
    public static class ProcessJS
    {
        public static List<ServicoDTO> processJson(List<JToken> viagens)
        {
            var lista = new List<ServicoDTO>();

            lista = viagens.Select(item => new ServicoDTO
            {
                Data = item.ElementAt(1).First.ToString(),
                HoraSaida = item.ElementAt(2).First.ToString(),
                Origem = item.ElementAt(3).First.ToString(),
                Destino = item.ElementAt(4).First.ToString(),
                Linha = item.ElementAt(5).First.ToString(),
                NumServico = item.ElementAt(0).First.ToString(),
            }).ToList();

            return lista;
        }
    }
}
