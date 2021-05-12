using System;
using System.IO;
using FileConnector.Utils;
using FileConnectorCommon;

namespace FileConnector
{
    static class ConfigClientCtl
    {
        public static void Run(ConfigOptionsClient opts)
        {

            Logger log = new Logger(Logger.Target.Memory);

            if (opts.Debug)
                log.SetLevel(Logger.Level.Debug);
            else
                log.SetLevel(Logger.Level.Info);

            Config.ConfigLog.setLog(log);


            if (opts.cmdClient)
            {

                if (opts.cmdIPCPipe?.Length > 0)
                {
                    Console.WriteLine("opts.cmdIPCPipe is set to {0}", opts.cmdIPCPipe);
                    Config.IPCClient.pipeName = opts.cmdIPCPipe;
                }
                else
                {
                    Console.WriteLine("opts.cmdIPCPipe is not set, defaulting to {0}", Config.IPCServer.pipeName);
                }

                // Banner
                Util.banner(ConsoleColor.White, ConsoleColor.DarkRed);
                IPC.CommandClient commandClient = new IPC.CommandClient(Config.IPCClient.pipeName);
                commandClient.Start();
            }

        }
    }
}
