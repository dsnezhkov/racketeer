using FileConnector.Models;
using FileConnector.Utils;
using FileConnectorCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security;

namespace FileConnector.HTTP.Tasks
{
    class TaskSetLogLevel : IHttpServerTask
    {
        private Logger log;

        public TaskSetLogLevel(){

            log = Config.ConfigLog.getLog();
        }
        public void DoRequest(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp )
        {
            int logLevel;

            if (cmReq.taskArgs.Count != 0)
            {

                try
                {
                    string logLevelStr = cmReq.taskArgs.FirstOrDefault();
                    log.Debug($"task Arg log level: {logLevelStr}");
                    if (int.TryParse((string) cmReq.taskArgs.FirstOrDefault(), out logLevel))
                    {
                        log.Debug($"task Arg log level (int): {logLevel}");
                        switch (logLevel)
                        {
                            case (int)Logger.Level.Debug:
                                log.Debug($"Setting logging level to Debug");
                                log.SetLevel(Logger.Level.Debug);
                                break;
                            case (int)Logger.Level.Info:
                                log.Debug($"Setting logging level to Info");
                                log.SetLevel(Logger.Level.Info);
                                break;
                            case (int)Logger.Level.Warn:
                                log.Debug($"Setting logging level to Warn");
                                log.SetLevel(Logger.Level.Warn);
                                break;
                            case (int)Logger.Level.Error:
                                log.Debug($"Setting logging level to Error");
                                log.SetLevel(Logger.Level.Error);
                                break;
                            default:
                                break;
                        }
                    }
                }catch(Exception e)
                {
                    log.Error($"Cannot process command request argument (log level): {e.Message}");
                }

            }
        }
        public void DoResponse(HttpClient client, CommandMessageResp cmResp) { }
    }
}
