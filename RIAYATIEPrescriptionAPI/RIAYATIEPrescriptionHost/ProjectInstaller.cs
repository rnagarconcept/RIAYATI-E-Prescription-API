using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace RIAYATIEPrescriptionHost
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void RiayatiPrescriptionServiceProcessInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            ServiceController sc = new ServiceController("Riayati E Prescription Service");
            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    new ServiceController(RiayatiEPrescriptionServiceInstaller.ServiceName).Stop();
                    break;
            }
        }
    }
}
