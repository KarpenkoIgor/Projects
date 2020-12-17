using System.ServiceProcess;
using System;
using System.IO;
using System.Threading;
using ConfigurationManager;

namespace DataManager
{
    public partial class Service1 : ServiceBase
    {
        Manager manager;
        DataManagerSettings settings;
        public Service1()
        {
            InitializeComponent();

            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
                ConfigurationSettings confSettings = new ConfigurationSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DMconfig.xml"));
                settings = confSettings.GetSetting<DataManagerSettings>();
                manager = new Manager(settings);
                Thread managerThread = new Thread(new ThreadStart(manager.Start));
                managerThread.Start();
        }

        protected override void OnStop()
        {
            Thread.Sleep(1000);
        }
    }
}
