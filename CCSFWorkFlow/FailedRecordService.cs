using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;


namespace CCSFWorkFlow
{
    public partial class FailedRecordService : ServiceBase
    {
        public FailedRecordService()
        {
            InitializeComponent(); //Initializing the service which need to run after starting the service
        }

        protected override void OnStart(string[] args)
        {

            SwitchOperator switchOperator = new SwitchOperator(); //Creating the Object for SwitchOperator
            switchOperator.ExecuteSFFailedRecords();         //Calls the method which triggers the CC WEB API for sales force failed records
            switchOperator.ExecuteFDFailedRecords();        //Calls the method which triggers the CC WEB API for fresh desk failed records
        }

        protected override void OnStop()
        {
        }
    }
}
