using System;
using System.Threading;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using FileConnector.Models;
using FileConnector.Utils;
using FileConnector.ServerTasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using FileConnectorCommon;
using FileConnector.HTTP;

namespace FileConnector.HTTP
{
    class ServerConnector
    {
        private Logger log;
        private HttpClient client;
        private DriverConfig conf;
        private ExtEndpoint ep;
        private ExtComm ec;
        private CommandMessageReq cmReq;
        private CommandMessageResp cmRes;

        private bool setComms(ref string errMsg)
        {
            bool state = false;

            List<ExtEndpoint> eps = new List<ExtEndpoint>();

            // excepton
            try
            {
                ep = conf.ExtEndpoints.Where( arr => arr.Ident.Equals( conf.ExtComm.As )).First();
                Config.HttpClient.setEp(ep);
                ec = conf.ExtComm;
                Config.HttpClient.setEc(ec);

                log.Debug($"Using Ident: {ep.Ident} for comms");
                log.Debug($"{ep.Base} (TSL: {ep.TLS}) comms");
                state = true;
                
            } catch (Exception e)
            {
                errMsg = e.Message;
            }

            return state;
        }
        public ServerConnector()
        {
            log = Config.ConfigLog.getLog();
            conf = Config.ConfigFile.getCfg();
            string errMsg = string.Empty;

            if (!setComms(ref errMsg))
            {
                throw new ArgumentException(errMsg);
            }
        }
        public void Start()
        {
            new Thread(Server).Start();
        }
        private void ProcessRequestFromServer(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp)
        {

            log.Debug($"Dcommand to deserialize: {cmReq.taskName}");
            TaskRouter.RouteRequest(CommMethod.HTTP, ref cmReq, ref cmResp);
        }

        private void ProcessResponseToServer(ref CommandMessageResp cmResp)
        {

            TaskRouter.RouteResponse(CommMethod.HTTP, client, ref cmResp);
        }

        void Server()
        {

            int checkinInterval = ep.Profile.Interval * 1000; 
            while (true)
            { 
                if (ep.TLS)
                {
                    // Ignore self-signed certs
                    ServicePointManager.ServerCertificateValidationCallback += 
                        (sender, cert, chain, sslPolicyErrors) => true;
                }
                client = new HttpClient();
                try
                {
                    CheckTaskAsync().GetAwaiter().GetResult();
                }
                catch (NullReferenceException)
                {
                    log.Debug($"\n Server may not be reachable\n");
                }
                catch (Exception e)
                {
                    log.Debug($"\n Exception {e.Message} {e.InnerException.Message}\n");
                }

                log.Debug($"Sleeping for {checkinInterval}");
                Thread.Sleep(checkinInterval); 
            }
        }

        async Task CheckTaskAsync()
        {

            string schema =  "http://";

            if (ep.TLS)
                schema = "https://";


            client.BaseAddress = new Uri(schema + ep.Base);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            
            try
            {
                // Get the task
                cmReq = await MessageShuttle.GetAsync(client, "/task/" + conf.Keys.SiteID );

                // TODO: should we check at no new requests before router?
                cmRes = new CommandMessageResp();
                ProcessRequestFromServer(ref cmReq, ref cmRes);
                ProcessResponseToServer(ref cmRes);
            }
            catch (Exception e)
            {
                log.Error($"Exception GetTaskAsync : {e.Message} {e.InnerException.Message}");
            }

        }

       
    }
}
