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

namespace FileConnector.HTTP.Tasks
{
    class TaskHeartbeat : IHttpServerTask
    {
        private Logger log;
        private ExtEndpoint ep;
        private ExtComm ec;
        private DriverConfig cfg;

        private string taskName;
        private Contract contract;

        public TaskHeartbeat(){

            taskName = "heartbeat";
            cfg = Config.ConfigFile.getCfg();
            log = Config.ConfigLog.getLog();
            ep = Config.HttpClient.getEp();
            ec = Config.HttpClient.getEc();

            try
            {
                contract = ec.Contracts.SingleOrDefault(p => p.Key == taskName).Value;
                if (contract == null)
                {
                    throw new ArgumentException("Empty contract." );
                }
            }catch(ArgumentException ae)
            {
                log.Error($"Resource >heartbeat< is not found {ae.Message}");
                throw new ArgumentException();
            }
        }
        public void DoRequest(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp )
        {

            log.Debug($"{cmReq.taskName} {cmReq.taskType} {cmReq.taskArgs} {cmReq.taskB64Payload} ");
            TaskHeartbeatToServer hts = new TaskHeartbeatToServer();
            ServerTasks.Impl.TaskHeartbeat task = new ServerTasks.Impl.TaskHeartbeat();
            task.Run(ref hts);

            cmResp.SiteId = cfg.Keys.SiteID;
            cmResp.taskMessage = Util.obj2JSON(false, hts);
            cmResp.taskName = taskName;
            cmResp.taskStatus = Models.TaskStatus.Success;
        }
        public async void DoResponse(HttpClient client, CommandMessageResp cmResp)
        {
            log.Debug($"Prepared RESPONSE: taskName: {cmResp.taskName}, " +
                $"taskStatus: {cmResp.taskStatus}," +
                $"taskMessage: {cmResp.taskMessage}, taskPayload: {cmResp.taskB64Payload}");


            log.Debug($"Resource {contract.Resource} method: {contract.Response.Method}");

            // Can process return message if needed
            _ = await MessageShuttle.Send(client,contract.Resource, contract.Response.Method, cmResp);
        }
    }
}
