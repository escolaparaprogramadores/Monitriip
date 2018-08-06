
using IntegradorRepository.LocalDatabase.Repository.Entity;
using IntegradorRequestWeb.RequestWeb;
using System;
using System.Threading;
using System.Xml;

namespace IntegradorMonitriip.BeforeRequest
{
    public class ServicoBR : ServicoRW
    {
        public static XmlDocument BaixaServicos( string requestUrl, int idCliente)
        {         
            try                
            {               
                string[] detalhe = new String[2];

                XmlDocument dados = BuscarDadosServico(requestUrl, 1, idCliente);

                return dados;
            }
            catch (Exception ex)
            {               
                throw ex;
            }

        }
        public static XmlDocument BaixaServicosUnesul(string requestUrl, int idCliente)
        {
            try
            {
                string[] detalhe = new String[2];

                XmlDocument dados = BaixaServicosUnesul(requestUrl, 1, idCliente);

                return dados;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static XmlDocument BaixaServicosItamarati(string requestUrl, DateTime data, int idCliente)
        {
            try
            {
                string[] detalhe = new String[2];

                XmlDocument dados = BaixaServicosItamarati(requestUrl, 1, data, idCliente);

                return dados;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static XmlDocument BaixaServicosGM(string requestUrl, DateTime data, Codigo_Conexao model)
        {
            try
            {
                string[] detalhe = new String[2];

                XmlDocument dados = BaixaServicosGMEmpresas(requestUrl, 1, data, model);

                return dados;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
