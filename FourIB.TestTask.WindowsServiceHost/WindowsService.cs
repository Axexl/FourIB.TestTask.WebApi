using Microsoft.Owin.Hosting;
using System;
using System.ServiceProcess;

namespace FourIB.TestTask.WindowsServiceHost
{
    public partial class Service1 : ServiceBase
    {
        private static IDisposable _server = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string baseAddress = "http://localhost:9000/";
            string msg = WebApi.ApiInfo.Help;

            _server = WebApp.Start<Startup>(url: baseAddress);
        }

        protected override void OnStop()
        {
            _server?.Dispose();
        }
    }
}