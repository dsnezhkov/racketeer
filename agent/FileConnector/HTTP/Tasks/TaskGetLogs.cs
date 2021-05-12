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
    class TaskGetLogs : IHttpServerTask
    {
        private Logger log;
        private DriverConfig cfg;
        private ExtEndpoint ep;
        private ExtComm ec;

        private string taskName;
        private Contract contract;

        public TaskGetLogs(){

            taskName = "getlogs";
            cfg = Config.ConfigFile.getCfg();
            log = Config.ConfigLog.getLog();
            ep = Config.HttpClient.getEp();
            ec = Config.HttpClient.getEc();

            try
            {
                // TODO: check if available
                contract = ec.Contracts.SingleOrDefault(p => p.Key == taskName).Value;
                if (contract == null)
                {
                    throw new ArgumentException("Empty contract." );
                }
            }catch(ArgumentException ae)
            {
                log.Error($"Resource >getlogs< is not found {ae.Message}");
                throw new ArgumentException();
            }
        }
        public void DoRequest(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp )
        {

            log.Debug($"DoRequest: {cmReq.taskName} {cmReq.taskType} {cmReq.taskArgs} {cmReq.taskB64Payload} ");

            cmResp.taskName = taskName;
            string logContent;

            if (log.GetLogTarget() == Logger.Target.Memory)
            {
               logContent = log.GetLogsFromMemory();
                if (logContent == String.Empty)
                {
                    logContent = "No log content.";
                }
            }
            else
            {
                logContent = "Logs are not im memory. Console used.";
            }

            cmResp.SiteId = cfg.Keys.SiteID;
            cmResp.taskStatus = Models.TaskStatus.Success;
            cmResp.taskMessage = Util.obj2JSON(false, String.Empty);
            cmResp.taskB64Payload = Util.Base64Encode(logContent);
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
