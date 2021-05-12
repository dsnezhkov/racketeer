using FileConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FileConnector.Utils;
using FileConnectorCommon;
using Newtonsoft.Json;

namespace FileConnector.HTTP
{
    static class MessageShuttle
    {

        static Logger log;
        static ExtEndpoint ep;
        static MessageShuttle()
        {
            log = Config.ConfigLog.getLog();
            ep = Config.HttpClient.getEp(); 
        }

        public static async Task<CommandMessageReq> Send(HttpClient client, string controller, HMethod method, object o)
        {
            switch (method)
            {
                case HMethod.None:
                    break;
                case HMethod.Get:
                    return  await MessageShuttle.GetAsync(client, controller);
                case HMethod.Post:
                    return await MessageShuttle.PostAsync(client, controller, o);
                default:
                    break;
            }

            return null;
        }

        public static async Task<CommandMessageReq> GetAsync( HttpClient client, string controller)
        {
            CommandMessageReq cmReq = null;
            string cmReqStr = string.Empty;
            HttpResponseMessage response;

            double timeout = ep.Timeout;

            using (CancellationTokenSource cancelAfterDelay = new CancellationTokenSource(TimeSpan.FromSeconds(timeout)))
            {
                DateTime startedTime = DateTime.Now;

                try
                {

                    response = await client.GetAsync(controller, cancelAfterDelay.Token);

                    if (response.IsSuccessStatusCode)
                    {
                        cmReqStr = await response.Content.ReadAsStringAsync();
                        log.Debug("Response from base:" + cmReqStr);
                        // cmReq = JsonConvert.DeserializeObject<CommandMessageReq>(cmReqStr);
                        cmReq = CommandSerializers.DeserializeCommandRequest(cmReqStr);
                    }
                    else
                    {
                        log.Error($"Response code from base: {response.StatusCode}");
                        return null;
                    }
                    return cmReq;

                }
                catch (TaskCanceledException)
                {
                    DateTime cancelledTime = DateTime.Now;
                    if (startedTime.AddSeconds(timeout - 1) <= cancelledTime)
                    {
                        throw new TimeoutException($"An HTTP request to {controller} timed out ({timeout} seconds)");
                    }
                    else
                        throw;
                }
            }
        }

        public static async Task<CommandMessageReq> PostAsync(HttpClient client, string controller, object o)
        {
            CommandMessageReq cmReq = null;
            string cmReqStr = string.Empty;
            HttpResponseMessage response;

            double timeout = ep.Timeout;

            // string content = JsonConvert.SerializeObject(o);
            string content = CommandSerializers.SerializeCommandResponse((CommandMessageResp) o);
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            using (CancellationTokenSource cancelAfterDelay = new CancellationTokenSource(TimeSpan.FromSeconds(timeout)))
            {
                DateTime startedTime = DateTime.Now;

                try
                {

                    response = await client.PostAsync(controller, byteContent, cancelAfterDelay.Token);

                    if (response.IsSuccessStatusCode)
                    {
                        cmReqStr = await response.Content.ReadAsStringAsync();
                        log.Debug("POST Response from base:" + cmReqStr);
                        // cmReq = JsonConvert.DeserializeObject<CommandMessageReq>(cmReqStr);
                        cmReq = cmReq = CommandSerializers.DeserializeCommandRequest(cmReqStr);
                    }
                    return cmReq;

                }
                catch (TaskCanceledException)
                {
                    DateTime cancelledTime = DateTime.Now;
                    if (startedTime.AddSeconds(timeout - 1) <= cancelledTime)
                    {
                        throw new TimeoutException($"An HTTP request to {controller} timed out ({timeout} seconds)");
                    }
                    else
                        throw;
                }
            }
        }
    }
}
