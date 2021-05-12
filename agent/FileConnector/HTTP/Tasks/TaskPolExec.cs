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
    class TaskPolExec : IHttpServerTask
    {
        private Logger log;

        public TaskPolExec(){

            log = Config.ConfigLog.getLog();
        }
        public void DoRequest(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp )
        {
            string policy = string.Empty;

            if (cmReq.taskB64Payload != "")
            {
                try
                {
                    log.Debug($"task payload: {cmReq.taskB64Payload}");
                    policy = Util.Base64Decode(cmReq.taskB64Payload);
                    log.Debug($"Policy: {policy}");
                    Config.ConfigFile.cfgFile = policy;
                    Config.ConfigFile.location = Location.Network;
                    ConfigLoader cfl = new ConfigLoader();
                    if (!cfl.Load())
                    {
                        log.Error($"Unable to load policy");
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Unable to convert b64 payload: {e.Message}");
                }

            }

            ServerTasks.Impl.TaskRunPolicy tp = new ServerTasks.Impl.TaskRunPolicy();
            tp.Run();
        }
        public void DoResponse(HttpClient client, CommandMessageResp cmResp) { }
    }
}
