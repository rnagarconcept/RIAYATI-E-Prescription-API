using ApplicationInsight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIAYATIEPrescriptionHost
{
    class Program
    {
        static void Main(string[] args)
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
    }
}
