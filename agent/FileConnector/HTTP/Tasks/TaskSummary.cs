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
    class TaskSummary : IHttpServerTask
    {
        private Logger log;
        private DriverConfig cfg;
        private ExtEndpoint ep;
        private ExtComm ec;

        private string taskName;
        private Contract contract;

        public TaskSummary(){

            taskName = "summary";
            cfg = Config.ConfigFile.getCfg();
            log = Config.ConfigLog.getLog();
            ep = Config.HttpClient.getEp();
            ec = Config.HttpClient.getEc();

            log.Debug($"Config File: {Config.ConfigFile.cfgFile}");
            log.Debug($"Ec: {ec}");


            contract = ec.Contracts.SingleOrDefault(p => p.Key == taskName).Value;
            if (contract == null)
            {
                throw new ArgumentException("Empty contract.");
            }
        }
        public void DoRequest(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp )
        {

            log.Debug($"DoRequest: {cmReq.taskName} {cmReq.taskType} {cmReq.taskArgs} {cmReq.taskB64Payload} ");

            cmResp.taskName = taskName;

            Models.TaskSummary ts = Config.TaskSummaries.getTaskSummary();
            if (ts.SiteId == null)
            {
                log.Debug($"Task Summary not set {ts}");
                cmResp.taskStatus = Models.TaskStatus.Pending;
                cmResp.taskMessage = Util.obj2JSON(false, String.Empty);
                cmResp.taskB64Payload = Util.obj2JSON(true, ts);
            }
            else
            {
                log.Debug($"Task Summary not null {ts.SiteId}");
                cmResp.taskStatus = Models.TaskStatus.Success;
                cmResp.taskMessage = "";
            }
            cmResp.SiteId = cfg.Keys.SiteID;
            cmResp.taskB64Payload = Util.obj2JSON(true, ts);
        }
        public async void DoResponse(HttpClient client, CommandMessageResp cmResp)
        {
            log.Debug($"DoResponse: taskName: {cmResp.taskName}, " +
                $"taskStatus: {cmResp.taskStatus}," +
                $"taskMessage: {cmResp.taskMessage}, taskPayload: {cmResp.taskB64Payload}");

            log.Debug($"=> Resource {contract.Resource} , method: {contract.Response.Method}");

            // Can process return message if needed
            _ = await MessageShuttle.Send(client,contract.Resource, contract.Response.Method, cmResp);
        }
    }
}
