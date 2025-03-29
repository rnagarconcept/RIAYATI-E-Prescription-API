using ApplicationInsight;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace RIAYATIEPrescriptionHost
{
    public partial class Service1 : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Service1));
        private static readonly int interval = string.IsNullOrEmpty(ConfigurationManager.AppSettings["ServiceInterval"]) ? 600 : Convert.ToInt32(ConfigurationManager.AppSettings["ServiceInterval"]);
        Timer timer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log.Info($"Service has been started at {DateTime.Now}");
            ScheduleService();
            log.Info($"Service has been completed at {DateTime.Now}");
        }

        protected override void OnStop()
        {
            log.Info($"Service has been stopped at {DateTime.Now}");
        }

        private void Process()
        {
            try
            {
                var taskAwaiter = RequestProcessingService.GetInstance.Process().GetAwaiter();
                taskAwaiter.GetResult();
            }
            catch (Exception ex)
            {
                log.Error($"Error in Process {ex.Message}", ex);
            }
        }

        public void ScheduleService()
        {
            log.Info($"Start ScheduleService");
            try
            {

                DateTime nowTime = DateTime.Now;
                var tickTime = interval * 1000;
                timer = new Timer(tickTime);
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                timer.Start();
            }
            catch (Exception ex)
            {
                log.Error($"Error ScheduleService {ex.Message}", ex);
                throw ex;
            }
        }
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            log.Info($"Service has been started to processing");
            Process();
            log.Info($"Service processing has been completed");
            Console.WriteLine("### Timer Stopped ### \n");
            timer.Stop();
            Console.WriteLine("### Scheduled Task Started ### \n\n");
            ScheduleService();
        }
    }
}
