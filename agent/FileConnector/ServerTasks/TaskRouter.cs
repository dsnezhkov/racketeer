using FileConnector.HTTP.Tasks;
using FileConnector.IPC.Tasks;
using FileConnector.Models;
using FileConnector.Utils;
using FileConnectorCommon;
using System;
using System.IO.Pipes;
using System.Net.Http;

namespace FileConnector.ServerTasks
{

    public static class TaskRouter
    {
        private static Logger log;
        static TaskRouter()
        {
            log = Config.ConfigLog.getLog();
        }
        public static void RouteRequest(CommMethod commMethod, ref CommandMessageReq cmReq, ref CommandMessageResp cmResp)
        {
            switch (commMethod)
            {
                case CommMethod.None:
                    break;
                case CommMethod.IPC:
                    RouteIPCRequest(ref cmReq, ref cmResp);
                    break;
                case CommMethod.HTTP:
                    RouteHTTPRequest(ref cmReq, ref cmResp);
                    break;
                default:
                    break;
            }
        }
        public static void RouteResponse(CommMethod commMethod, object client, ref CommandMessageResp cmResp)
        {
            switch (commMethod)
            {
                case CommMethod.None:
                    break;
                case CommMethod.IPC:
                    RouteIPCResponse((NamedPipeClientStream) client,cmResp);
                    break;
                case CommMethod.HTTP:
                    RouteHTTPResponse((HttpClient) client,cmResp);
                    break;
                default:
                    break;
            }
        }
        public static bool RouteIPCRequest(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp)
        {

            switch (cmReq.taskName)
            {
                case "heartbeat":
                    new IPC.Tasks.TaskHeartbeat().DoRequest(ref cmReq, ref cmResp);
                    break;
                default:
                    break;
            }
            return true;
        }

        public static bool RouteHTTPRequest(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp)
        {

            switch (cmReq.taskName)
            {
                case "heartbeat":
                    new HTTP.Tasks.TaskHeartbeat().DoRequest(ref cmReq, ref cmResp);
                    break;
                case "masterkey":
                    new HTTP.Tasks.TaskMasterKey().DoRequest(ref cmReq, ref cmResp);
                    break;
                case "summary":
                    try
                    {
                        HTTP.Tasks.TaskSummary ts = new HTTP.Tasks.TaskSummary();
                        ts.DoRequest(ref cmReq, ref cmResp);

                    }
                    catch (Exception e)
                    {
                        log.Error($"Unable to execute TaskSummary: {e.Message} {e.InnerException?.Message} {e.Source}");
                    }
                    break;
                case "polexec":
                    new HTTP.Tasks.TaskPolExec().DoRequest(ref cmReq, ref cmResp);
                    break;
                case "getlogs":
                    new HTTP.Tasks.TaskGetLogs().DoRequest(ref cmReq, ref cmResp);
                    break;
                case "clearlogs":
                    new HTTP.Tasks.TaskClearLogs().DoRequest(ref cmReq, ref cmResp);
                    break;
                case "setlogs":
                    new HTTP.Tasks.TaskSetLogLevel().DoRequest(ref cmReq, ref cmResp);
                    break;
                case "setlogsource":
                    new HTTP.Tasks.TaskSetLogSource().DoRequest(ref cmReq, ref cmResp);
                    break;
                case "agentselfexec":
                    new HTTP.Tasks.TaskAgentSelfExec().DoRequest(ref cmReq, ref cmResp);
                    break;
                default:
                    break;
            }
            return true;
        }

        public static bool RouteHTTPResponse(HttpClient client, CommandMessageResp cmResp)
        {

            switch (cmResp.taskName)
            {
                case "heartbeat":
                    new HTTP.Tasks.TaskHeartbeat().DoResponse(client, cmResp);
                    break;
                case "masterkey":
                    new HTTP.Tasks.TaskMasterKey().DoResponse(client, cmResp);
                    break;
                case "summary":
                    new HTTP.Tasks.TaskSummary().DoResponse(client, cmResp);
                    break;
                case "polexec":
                    new HTTP.Tasks.TaskPolExec().DoResponse(client, cmResp);
                    break;
                case "getlogs":
                    new HTTP.Tasks.TaskGetLogs().DoResponse(client, cmResp);
                    break;
                case "clearlogs":
                    new HTTP.Tasks.TaskClearLogs().DoResponse(client, cmResp);
                    break;
                case "setlogs":
                    new HTTP.Tasks.TaskSetLogLevel().DoResponse(client, cmResp);
                    break;
                case "setlogsource":
                    new HTTP.Tasks.TaskSetLogSource().DoResponse(client, cmResp);
                    break;
                case "agentselfexec":
                    new HTTP.Tasks.TaskAgentSelfExec().DoResponse(client, cmResp);
                    break;
                default:
                    break;
            }
            return true;
        }

        public static bool RouteIPCResponse(NamedPipeClientStream client, CommandMessageResp cmResp)
        {

            switch (cmResp.taskName)
            {
                case "heartbeat":
                    new IPC.Tasks.TaskHeartbeat().DoResponse(client, cmResp);
                    break;
                default:
                    break;
            }
            return true;
        }

    }
}
