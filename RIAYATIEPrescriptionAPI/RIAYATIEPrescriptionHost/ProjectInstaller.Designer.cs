
namespace RIAYATIEPrescriptionHost
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RiayatiPrescriptionServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.RiayatiEPrescriptionServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // RiayatiPrescriptionServiceProcessInstaller
            // 
            this.RiayatiPrescriptionServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.RiayatiPrescriptionServiceProcessInstaller.Password = null;
            this.RiayatiPrescriptionServiceProcessInstaller.Username = null;
            this.RiayatiPrescriptionServiceProcessInstaller.BeforeUninstall += new System.Configuration.Install.InstallEventHandler(this.RiayatiPrescriptionServiceProcessInstaller_BeforeUninstall);
            // 
            // RiayatiEPrescriptionServiceInstaller
            // 
            this.RiayatiEPrescriptionServiceInstaller.Description = "Riayati E Prescription Service";
            this.RiayatiEPrescriptionServiceInstaller.DisplayName = "Riayati E Prescription Service";
            this.RiayatiEPrescriptionServiceInstaller.ServiceName = "Riayati E Prescription Service";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.RiayatiPrescriptionServiceProcessInstaller,
            this.RiayatiEPrescriptionServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller RiayatiPrescriptionServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller RiayatiEPrescriptionServiceInstaller;
    }
}