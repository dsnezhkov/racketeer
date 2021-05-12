using FileConnector.Models;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FileConnector.Models
{
    interface IServerTask
    {
        void DoRequest(ref CommandMessageReq cmdReq, ref CommandMessageResp cmdResp);
    }
    interface IIPCServerTask: IServerTask { 
        void DoResponse(NamedPipeClientStream client, CommandMessageResp cmdResp);
    }
    interface IHttpServerTask: IServerTask
    {
        void DoResponse(HttpClient client, CommandMessageResp cmdResp);
    }
}
