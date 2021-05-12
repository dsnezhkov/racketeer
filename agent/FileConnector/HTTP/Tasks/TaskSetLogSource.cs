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
    class TaskSetLogSource : IHttpServerTask
    {
        private Logger log;

        public TaskSetLogSource(){

            log = Config.ConfigLog.getLog();
        }
        public void DoRequest(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp )
        {
            int logSource;

            if (cmReq.taskArgs.Count != 0)
            {

                try
                {
                    string logSourceStr = cmReq.taskArgs.FirstOrDefault();
                    log.Debug($"task Arg log source: {logSourceStr}");
                    if (int.TryParse((string) cmReq.taskArgs.FirstOrDefault(), out logSource))
                    {
                        log.Debug($"task Arg log level (int): {logSource}");
                        switch (logSource)
                        {
                            case (int)Logger.Target.Console:
                                log.Debug($"Setting logging source to console");
                                log.SetLogTarget(Logger.Target.Console);
                                break;
                            case (int)Logger.Target.Memory:
                                log.Debug($"Setting logging source to memory");
                                log.SetLogTarget(Logger.Target.Memory);
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
