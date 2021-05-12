using FileConnector.Models;
using FileConnector.Utils;
using FileConnectorCommon;
using System;
using System.Diagnostics;

namespace FileConnector.ServerTasks.Impl
{
    class TaskHeartbeat
    {
        private Logger log;
        public TaskHeartbeat()
        {
            log = Config.ConfigLog.getLog(); 
        }
        public void Run(ref TaskHeartbeatToServer hts)
        {
            try
            {
                hts.ProcessHost = Environment.GetEnvironmentVariable("COMPUTERNAME");
                hts.ProcessHost += " / ";
                hts.ProcessHost += System.Net.Dns.GetHostName();
            }
            catch (Exception e)
            {
                hts.ProcessHost = e.Message;
                log.Debug($"Unable to get host name: {e.Message}");
            }

            try
            {
                hts.ProcessPid = Process.GetCurrentProcess().Id;
            }
            catch (Exception e)
            {
                hts.ProcessPid = 0;
                log.Debug($"Unable to get PID {e.Message}");
            }

            try
            {
                hts.ProcessUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
            catch (Exception e)
            {
                hts.ProcessUser = e.Message;
                log.Debug($"Unable to get User Name {e.Message}");
            }

            if (CredMgmt.IsAdministrator())
            {
                hts.isAdministrator = true;
            }
            else
            {
                hts.isAdministrator = false;
            }
        }
    }
}
