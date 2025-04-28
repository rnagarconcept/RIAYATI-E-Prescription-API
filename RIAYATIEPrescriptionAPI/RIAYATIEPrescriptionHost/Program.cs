using ApplicationInsight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIAYATIEPrescriptionHost
{
    class Program
    {
        private static readonly bool debugg = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DEBUGG"])? false : Convert.ToBoolean(ConfigurationManager.AppSettings["DEBUGG"]) ;
        static void Main(string[] args)
        {
            if (debugg)
            {
                var awaitTask = RequestProcessingService.GetInstance.Process();
                awaitTask.ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        Console.WriteLine("Error: " + task.Exception.Message);
                    }
                    else
                    {
                        Console.WriteLine("Completed!");
                    }
                });
            }
            else
            {

            }            
        }       
    }
}
