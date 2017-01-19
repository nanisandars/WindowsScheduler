using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CCSFWorkFlow
{
    static class Program
    {
        /// The main entry point for the application.
        /// 
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new FailedRecordService() 
            }; //Initializing the new ServiceBase to run the service 
            ServiceBase.Run(ServicesToRun);
        }
    }
}
