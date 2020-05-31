using System;
using System.Threading;
using System.Threading.Tasks;   
using System.Diagnostics;
using System.ComponentModel;
using System.Management;
using System.Linq;

namespace WebHelperKiller
{
    class Program
    {
        private static Timer _timer;
        static void Main(string[] args)
        {

            //PrintPropertiesOfWmiClass("", "Win32_Process");
            _timer = new Timer(
                callback:new TimerCallback(TimerTask),
                null,
                dueTime: 0,
                period: 60000
                );

            Thread.Sleep(Timeout.Infinite);
        }

        static void TimerTask(object timerState)
        {
            //check for webhelper processes
            //check for bitcomet process
            //var query = string.Format("Select * From Win32_Process Where ParentProcessId = {0}");
            //query = string.Format("Select * From Win32_Process Where Name = \"Bitcomet\"");

            Console.WriteLine(DateTime.Now.ToString() + " Checking.......");
            Process[] bitcomet = Process.GetProcessesByName("BitTorrent");

            foreach (var instance in bitcomet)
            {                
                var query = string.Format("Select * From Win32_Process Where ParentProcessId={0}", instance.Id);
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection processList = searcher.Get();

                if (processList.Count > 0)
                {
                    foreach (ManagementObject obj in processList)
                    {
                        UInt32 childProcessId = (UInt32)obj["ProcessId"];
                        Console.WriteLine("--------------------------------------------------------------");
                        Console.WriteLine("Process Id:" + childProcessId);

                        Process child = Process.GetProcessById((int)childProcessId);
                        child.Kill();
                    }
                }
            }
        }

        static void PrintPropertiesOfWmiClass(string namespaceName, string wmiClassName)
        {
            ManagementPath managementPath = new ManagementPath();
            managementPath.Path = namespaceName;
            ManagementScope managementScope = new ManagementScope(managementPath);
            ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM " + wmiClassName);
            ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher(managementScope, objectQuery);
            ManagementObjectCollection objectCollection = objectSearcher.Get();
            foreach (ManagementObject managementObject in objectCollection)
            {
                PropertyDataCollection props = managementObject.Properties;
                foreach (PropertyData prop in props)
                {
                    Console.WriteLine("Property name: {0}", prop.Name);
                    Console.WriteLine("Property type: {0}", prop.Type);
                    Console.WriteLine("Property value: {0}", prop.Value);
                }
            }
        }
    }
}
