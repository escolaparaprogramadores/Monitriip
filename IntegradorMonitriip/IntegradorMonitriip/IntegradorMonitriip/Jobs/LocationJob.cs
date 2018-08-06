using IntegradorModel.ProcessXml;
using IntegradorMonitriip.BeforeRequest;
using IntegradorMonitriip.DataRepository;
using IntegradorRepository.DataRepository;
using IntegradorRepository.LocalDatabase.Repository.Entity;
using IntegradorRequestWeb.RequestWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace IntegradorMonitriip.Jobs
{
    public class LocationJob : LocalidadeBR
    {
        private static List<int> execucaoServicos = new List<int>();
        public static void Main()
        {
            DateTime data = DateTime.UtcNow;
            var emp = new GetEmpresas();
            var conexoes = emp.getCodigosEmpresas();

            var count = -1;
            foreach (var con in conexoes)
            {
                if (isAtivo(con.IDCliente))
                {
                    count++;
                    Thread job =
                                new Thread(
                                unused => integraLocalidades(data, con)
                                );
                    job.Start();
                }
            }
        }

        private static void integraLocalidades(DateTime data, Codigo_Conexao item)
        {
            string url = getUrlBase(item.IDCliente);

            var requestUrl = string.Format(Parameters.BUSCA_LOCALIDADE,url, 
                                item.Codigo1.Trim(), 
                                item.Codigo2.Trim());

            XmlDocument Servicos;
            try
            {
                Servicos = BaixarLocalidades(requestUrl, item.IDCliente);
            }
            catch
            {
                Servicos = null;
            }

            try
            {
                var processXML = new LocalidadePX();
                var servicos = processXML.TratarRetorno(Servicos);

                if (servicos != null)
                {
                    var repository = new UpdateLocarion();
                    repository.update(servicos, item.IDCliente);
                }
                //Parameters.DataUltimaImportacao = data;
            }
            catch (Exception e)
            { }

        }

        public static string getUrlBase(int idCliente)
        {
            var retorno = "";
            switch (idCliente)
            {
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
                    retorno = "http://qslinuxrj.cloudapp.net:9991/";
                    break;
            }
            return retorno;
        }

        static bool isAtivo(int id)
        {
            if (!execucaoServicos.Contains(id))
            {
                execucaoServicos.Add(id);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
