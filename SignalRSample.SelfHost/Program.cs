using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Configuration;

namespace SignalRSample.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = ConfigurationManager.AppSettings["HubUrl"].ToString();

            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);

                Console.ReadLine();
            }
        }
    }
}
