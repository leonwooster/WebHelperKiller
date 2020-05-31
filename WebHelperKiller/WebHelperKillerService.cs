using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.Management;
using System.Linq;
using System.ServiceProcess;

using log4net;
using log4net.Appender;

namespace WebHelperKiller
{
    public class WebHelperKillerService : ServiceBase
    {
        public WebHelperKillerService()
        {
            ServiceName = "Web Helper Killer";
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        private static void CheckForWebHelper()
        {
        }
    }
}
