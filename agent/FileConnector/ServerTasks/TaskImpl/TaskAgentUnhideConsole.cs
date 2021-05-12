using FileConnector.Models;
using FileConnector.Utils;
using FileConnectorCommon;
using System;
using System.Diagnostics;
using System.Threading;

namespace FileConnector.ServerTasks.Impl
{
    class TaskAgentUnhideConsole
    {
        private Logger log;
        public TaskAgentUnhideConsole()
        {
            log = Config.ConfigLog.getLog(); 
        }
        public void Run()
        {
            try
            {
                IntPtr h = Config.ConfigProcess.getHandle();
                log.Debug($"Window handle: ({h})");
                Interop.ShowWindow(h, 1);
            }
            catch (Exception e)
            {
                log.Debug($"Unable to get window handle: {e.Message}");
            }

            new Thread(() => System.Windows.Forms.MessageBox.Show("hi!")).Start();

        }
    }
}
