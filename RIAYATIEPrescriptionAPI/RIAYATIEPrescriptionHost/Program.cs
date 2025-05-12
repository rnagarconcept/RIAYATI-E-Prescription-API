using ApplicationInsight;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RIAYATIEPrescriptionHost
{
    //class Program
    //{
    //    private static readonly bool debugg = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DEBUGG"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["DEBUGG"]);
    //    static void Main(string[] args)
    //    {
    //        if (debugg)
    //        {
    //            var awaitTask = RequestProcessingService.GetInstance.Process();
    //            awaitTask.ContinueWith(task =>
    //            {
    //                if (task.Exception != null)
    //                {
    //                    Console.WriteLine("Error: " + task.Exception.Message);
    //                }
    //                else
    //                {
    //                    Console.WriteLine("Completed!");
    //                }
    //            });
    //        }
    //        else
    //        {

    //        }
    //    }
    //}

    public static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private static readonly bool EnabledDebugg = string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnabledDebugg"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["EnabledDebugg"]);

        static void Main(string[] args)
        {
            log.Info($"Service has been started. {DateTime.Now.ToLongDateString()}");

            try
            {
                if (EnabledDebugg)
                {
                    Debugg();
                    Console.ReadKey();
                }
                else
                {
                    //ServiceBase[] ServicesToRun;
                    //ServicesToRun = new ServiceBase[] { new Service1() };
                    //ServiceBase.Run(ServicesToRun);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error in service {ex.Message}", ex);
            }
        }

        private static void Debugg()
        {
            try
            {
                // Populate Pending Request Table
                var awaitTask1 =  RequestProcessingServiceEx.GetInstance.Process();
                //var awaitTask = RequestProcessingService.GetInstance.Process();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
