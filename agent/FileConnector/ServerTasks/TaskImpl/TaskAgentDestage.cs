using FileConnector.Models;
using FileConnector.Utils;
using FileConnectorCommon;
using System;
using System.Diagnostics;
using System.Threading;

namespace FileConnector.ServerTasks.Impl
{
    class TaskAgentDestage
    {
        private Logger log;
        public TaskAgentDestage()
        {
            log = Config.ConfigLog.getLog(); 
        }
        public void Run()
        {
            try
            {
                cleanup();
                ProcessMgmt.TerminateProcess(Process.GetCurrentProcess());
            }
            catch (Exception e)
            {
                log.Debug($"Unable to kill process : {e.Message}");
            }
        }
        private void cleanup()
        {
            log.Debug($"Agent clean up");
            return;
        }
    }
}
