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
    class TaskMasterKey : IHttpServerTask
    {
        private Logger log;

        public TaskMasterKey(){

            log = Config.ConfigLog.getLog();
        }
        // Master Key comes over a B64 payload field. 
        public void DoRequest(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp )
        {
            string mk = string.Empty;

            if (cmReq.taskB64Payload != "")
            {
                try{
                    log.Debug($"task payload: {cmReq.taskB64Payload}");
                    mk  = Util.Base64Decode(cmReq.taskB64Payload);
                    // log.Debug($"Master Key: {mk}");
                    // mk = JsonConvert.DeserializeObject<String>(cmdJsonPay);
                } catch (Exception e) {
                    log.Error($"Unable to convert b64 payload: {e.Message}");
                }

                try
                {
                    Config.MasterKey.setMK(Util.ConvertToSecureString(mk));
                    mk = null;

                }catch (Exception e)
                {
                    log.Error($"Unable to convert to secure string: {e.Message}");
                }
            }
        }
        public void DoResponse(HttpClient client, CommandMessageResp cmResp) { }
    }
}
