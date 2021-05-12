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
using System.IO.Pipes;

namespace FileConnector.IPC.Tasks
{
    class TaskHeartbeat : IIPCServerTask
    {
        private Logger log;

        private string taskName;

        public TaskHeartbeat(){
            taskName = "heartbeat";
            log = Config.ConfigLog.getLog();
        }
        public void DoRequest(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp )
        {

            TaskHeartbeatToServer hts = new TaskHeartbeatToServer();
            ServerTasks.Impl.TaskHeartbeat task = new ServerTasks.Impl.TaskHeartbeat();
            task.Run(ref hts);

            cmResp.taskMessage = Util.obj2JSON(false, hts);
            cmResp.taskName = taskName;
            cmResp.taskStatus = Models.TaskStatus.Success;
        }
        public void DoResponse(NamedPipeClientStream client, CommandMessageResp cmResp)
        {
            log.Debug($"Got  RESPONSE: taskName: {cmResp.taskName}, " +
               $"taskStatus: {cmResp.taskStatus}," +
               $"taskMessage: {cmResp.taskMessage}, taskPayload: {cmResp.taskB64Payload}");
        }
    }
}
