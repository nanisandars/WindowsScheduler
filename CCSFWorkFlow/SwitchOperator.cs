using System;
using System.Collections.Generic;
using System.Collections;
using System.Timers;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;

namespace CCSFWorkFlow
{

    class SwitchOperator
    {

        #region Members

        private Timer m_SFTimer;
        private bool isInRunSF;

        //FD Integration
        private Timer m_FDTimer;
        private bool isInRunFD;

        #endregion

        #region Constructors

        public SwitchOperator()
        { }

        #endregion

        public void ExecuteSFFailedRecords()
        {
            try
            {
                string timeInterval = ConfigurationManager.AppSettings["timeIntervalSF"].ToString();  //Gets the TimeInterval set up in the APP Settings
                isInRunSF = false;  //Used to checking whether the task has completed or not 
                m_SFTimer = new Timer(); //Instance of a new TIMER
                m_SFTimer.AutoReset = true;
                m_SFTimer.Interval = double.Parse(timeInterval);
                m_SFTimer.Elapsed += new ElapsedEventHandler(Timer_ElapsedSF); // Registering the event handler which need to be called
                m_SFTimer.Start();  //Starts the Timer Method
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("CCSF FailedRecords Service SF", ex.Message, EventLogEntryType.Error);
            }
        }


        private void Timer_ElapsedSF(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!isInRunSF)  //Checks the status of completion
                {
                    isInRunSF = true;
                    string urlParameter = "";
                    var client = new HttpClient();
                    string ccurl = ConfigurationManager.AppSettings["CCSFURL"].ToString();  //Gets the CC WEB API URL set up in the app config
                    client.BaseAddress = new Uri(ccurl);
                    try
                    {
                        var task = client.GetAsync(urlParameter); //Sends the request to the CC WebAPI
                        task.Wait();
                        var response = task.Result;
                        if (response.IsSuccessStatusCode) //Checks the status code 
                        {
                            var readTask = response.Content.ReadAsStringAsync();
                            readTask.Wait();
                            var dataObj = readTask.Result;
                            EventLog.WriteEntry("CCSF FailedRecords Service SF", response.Content.ToString(), EventLogEntryType.SuccessAudit);
                        }
                        else
                        {
                            EventLog.WriteEntry("CCSF FailedRecords Service SF", response.Content.ToString(), EventLogEntryType.FailureAudit);
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        EventLog.WriteEntry("CCSF FailedRecords Service SF", msg, EventLogEntryType.Error);
                    }

                    isInRunSF = false;  //After completion of the single api call reassigning with false 
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("CCSF FailedRecords Service SF", ex.Message, EventLogEntryType.Error);
                isInRunSF = false;
            }
        }




        #region FD Integration

        public void ExecuteFDFailedRecords()
        {
            try
            {
                string timeInterval = ConfigurationManager.AppSettings["timeIntervalFD"].ToString();  //Gets the TimeInterval set up in the APP Settings
                isInRunFD = false;  //Used to checking whether the task has completed or not 
                m_FDTimer = new Timer(); //Instance of a new TIMER
                m_FDTimer.AutoReset = true;
                m_FDTimer.Interval = double.Parse(timeInterval);
                m_FDTimer.Elapsed += new ElapsedEventHandler(Timer_ElapsedFD); // Registering the event handler which need to be called
                m_FDTimer.Start();  //Starts the Timer Method
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("CCFD FailedRecords Service FD", ex.Message, EventLogEntryType.Error);
            }
        }


        private void Timer_ElapsedFD(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!isInRunFD)  //Checks the status of completion
                {
                    isInRunFD = true;
                    string urlParameter = "";
                    var client = new HttpClient();
                    string ccurl = ConfigurationManager.AppSettings["CCFDURL"].ToString();  //Gets the CC WEB API URL set up in the app config
                    client.BaseAddress = new Uri(ccurl);
                    try
                    {
                        var task = client.GetAsync(urlParameter); //Sends the request to the CC WebAPI
                        task.Wait();
                        var response = task.Result;
                        if (response.IsSuccessStatusCode) //Checks the status code 
                        {
                            var readTask = response.Content.ReadAsStringAsync();
                            readTask.Wait();
                            var dataObj = readTask.Result;
                            EventLog.WriteEntry("CCFD FailedRecords Service FD", response.Content.ToString(), EventLogEntryType.SuccessAudit);
                        }
                        else
                        {
                            EventLog.WriteEntry("CCFD FailedRecords Service FD", response.Content.ToString(), EventLogEntryType.FailureAudit);
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        EventLog.WriteEntry("CCFD FailedRecords Service FD", msg, EventLogEntryType.Error);
                    }

                    isInRunFD = false;  //After completion of the single api call reassigning with false 
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("CCFD FailedRecords Service FD", ex.Message, EventLogEntryType.Error);
                isInRunFD = false;
            }
        }

        #endregion
    }
}