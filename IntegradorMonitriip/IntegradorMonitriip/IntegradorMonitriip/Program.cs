using EucaturIntegrador;
using IntegradorMonitriip.Jobs;
using System;
using System.ServiceProcess;
//using NewsGPS.Common.Service;
using System.Threading;

namespace IntegradorMonitriip
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
            new Service1()
            //new AnttLog()
            };
            try
            {
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Process.Start("http://google.com");
                ServiceBase.Run(ServicesToRun);
            }
        }
    }

}
