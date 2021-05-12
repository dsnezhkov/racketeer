using System;
using System.Diagnostics;
using System.IO;
using FileConnector.Utils;
using FileConnectorCommon;

namespace FileConnector
{
    static class ConfigServerCtl
    {
        static Logger log;
        
        static ConfigServerCtl()
        {
            log = new Logger(Logger.Target.Memory);
        }
        public static void Run(ConfigOptionsServer opts)
        {

            if (ProcessMgmt.IsDuplicateProcessRunning(ProcessMgmt.GetSelf()))
            {
                return;
            }
            if (opts.LogSource != string.Empty)
                switch (opts.LogSource)
                {
                    case "c":
                        log.SetLogTarget(Logger.Target.Console);
                        break;
                    case "m":
                        log.SetLogTarget(Logger.Target.Memory);
                        break;
                    case "f":
                        log.Error("Not yet implemented");
                        break;
                    default:
                        log.Error($"Unknown option: {opts.LogSource}");
                        break;
                }

            if (opts.Debug)
                log.SetLevel(Logger.Level.Debug);
            else
                log.SetLevel(Logger.Level.Info);

            Config.ConfigLog.setLog(log);

            if (opts.cmdServer)
            {
                if (opts.cmdIPCPipe?.Length > 0)
                {
                    log.Debug($"opts.cmdIPCPipe is set to {opts.cmdIPCPipe}");
                    Config.IPCServer.pipeName = opts.cmdIPCPipe;
                }
                else
                {
                    log.Debug($"opts.cmdIPCPipe is not set, defaulting to {Config.IPCServer.pipeName}");
                }

                if (opts.cfgFile.Length != 0)
                {
                    log.Debug($"Log path is specified: {opts.cfgFile}");
                    Config.ConfigFile.cfgFile = opts.cfgFile;
                    Config.ConfigFile.location = Location.External;
                }
                else
                {
                    log.Debug("Log path is NOT specified. Embedded. ...... ");
                    Config.ConfigFile.cfgFile = "FileConnector.policy-minimal-network.json";
                    Config.ConfigFile.location = Location.Embedded;
                }

                // Hide console
                try
                {
                    IntPtr h = Process.GetCurrentProcess().MainWindowHandle;
                    Config.ConfigProcess.setHandle(h);
                    log.Debug($"Hiding Console: ({h})");
                    Interop.ShowWindow(h, 0);
                    if (opts.ShowCon)
                    {
                        Interop.ShowWindow(h, 1);
                    }

                } catch (Exception e) {
                    log.Debug($"Hiding Console error: {e.Message}");
                }

                try { 
                    ServerDriver serverDrv = new ServerDriver();
                    serverDrv.Run();
                }catch(Exception e)
                {
                    log.Fatal($"Execution failed {e.Message} {e.InnerException?.Message}");
                }

            }

        }

    }
}
