using FileConnector.Models;
using FileConnector.Utils;
using FileConnectorCommon;
using System;

namespace FileConnector
{
    class ServerDriver
    {
        private IPC.ServerConnector cmdSrvIPC;
        private HTTP.ServerConnector cmdSrvHTTP;
        private Logger log;
        private ConfigLoader cfl;
        private DriverConfig cftree;

        public ServerDriver()
        {
            // Init logger
            log = Config.ConfigLog.getLog();

            // Load initial config
            log.Debug("Initializing ConfigLoader");
            cfl = new ConfigLoader();


            if ( !cfl.Load() )
            {
                throw new ArgumentException("Unable to load policy");
            }

            // Init task summary object
            Config.TaskSummaries.setTaskSummary(new TaskSummary());
        }

        public void Run()
        {

            // Init IPC comms tp accept clients
            cftree = Config.ConfigFile.getCfg();
            log.Debug($"Starting in OpMode: {cftree.OpMode}" );

            switch (cftree.OpMode)
            {
                case OpMode.None:
                    break;
                case OpMode.Interactive:
                    log.Debug("Initializing IPC");
                    cmdSrvIPC = new IPC.ServerConnector(Config.IPCServer.pipeName);
                    cmdSrvIPC.Start();
                    break;
                case OpMode.Distributed:
                    log.Debug("Initializing HTTP");
                    cmdSrvHTTP = new HTTP.ServerConnector();
                    cmdSrvHTTP.Start();
                    // ServerTasks.TaskRunPolicyInit rlop = new ServerTasks.TaskRunPolicyInit();
                    // rlop.DoRequest();
                    break;
                default:
                    break;
            }

        }
    }
}
