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
    class TaskClearLogs : IHttpServerTask
    {
        private Logger log;

        public TaskClearLogs(){

            log = Config.ConfigLog.getLog();
        }
        public void DoRequest(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp )
        {
            log.ClearMemoryLogs();
        }
        public void DoResponse(HttpClient client, CommandMessageResp cmResp) { }
    }
}
