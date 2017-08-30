using FourIB.TestTask.WebApi;
using Microsoft.Owin.Hosting;
using System;

namespace FourIB.TestTask.OwinSelfHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                string startMsg = string.Format("Owin selfhost server listening on {0}", baseAddress);
                Logger.Log.Info(startMsg);
                Console.WriteLine(startMsg);
                Console.WriteLine("Press [ENTER] to end");
                Console.WriteLine(ApiInfo.Help);
                Console.ReadLine();
            }

            Logger.Log.Info(string.Format("Owin selfhost server stoped on {0}", baseAddress));
        }
    }
}