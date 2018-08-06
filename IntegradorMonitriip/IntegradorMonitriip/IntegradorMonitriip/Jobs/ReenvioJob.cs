using System;
using System.Xml;
using System.Xml.Serialization;
using IntegradorMonitriip.DataRepository;
using IntegradorMonitriip.BeforeRequest;
using System.Collections.Generic;
using antt.gov.br.monitriip.v1._0;
using IntegradorRequestWeb.RequestWeb;
using IntegradorRepositoryAzure.AzureTables;
using IntegradorRepository.LocalDatabase;
using System.Threading;
using IntegradorModel.Model.XmlModel;
using IntegradorModel.Model;
using IntegradorRepository.LocalDatabase.Repository.Entity;
using System.Linq;
using IntegradorModel.ProcessXml;
using IntegradorRepositoryAzure;

namespace IntegradorMonitriip.Jobs
{
    public class ReenvioJob : ViagensBR
    {

        private static List<int> execucaoReenvio = new List<int>();      

        public static void IncrementarListaLogs (ref List<ViagemModel> listaLogs)
        {

            AlimentarFilaNovamente:

            DateTime data = DateTime.UtcNow.AddHours(-3).AddHours(-10);

            ViagensRepository rep = new ViagensRepository();

            try
            {

                var lista = rep.GetQuery().Where(x => x.codigoTipoLogID == 7 && x.isErro == true && x.dataHoraEvento >= data).ToList(); // && x.Erros.Trim().Equals("Timeout de envio para ANTT.")).ToList();//&& x.dataHoraEvento >= data).ToList();

                if (listaLogs.Count > 0)
                {
                    foreach (var item in lista)
                    {

                        var contains = listaLogs.Where(x => x.PartitionKey == item.PartitionKey).Count();
                        if(contains == 0)
                           listaLogs.Add(item);
                    }
                }
                else
                {
                    listaLogs.AddRange(lista);
                }

            }
            catch(Exception ex)
            {
                Thread.Sleep(30000);
                goto AlimentarFilaNovamente;
            }

            
            
        }

        public static void ProcessarReenvio(List<ViagemModel> listaLogs)
        {
          
            foreach (var item in listaLogs)
            {                                                     
                Thread job =
                new Thread(
                unused => jobReenvio(item, listaLogs)
                );
                job.Start();                
            }        
           
        }

        static void jobReenvio(ViagemModel item, List<ViagemModel> listaLogs)
        {
            try
            {
                
                //var result =  EnviaANTT(item);

                //if (!result.Erros.Trim().Equals("Timeout de envio para ANTT."))
                //{
                //    result.isErro = false;
                //    item.dataEnvioAntt = DateTime.UtcNow;

                //    ViagensRepository rep = new ViagensRepository();
                //    rep.Update(item);

                //    listaLogs.Remove(item);


                //}
               
            }
            catch (Exception ex)
            {
            }
        
        }
    }
}
