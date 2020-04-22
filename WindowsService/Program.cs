using log4net.Config;
using System;
using System.ServiceProcess;

namespace WindowsService
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new NewService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
