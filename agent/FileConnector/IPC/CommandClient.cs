using System;
using System.Threading;
using System.IO;
using System.IO.Pipes;
using System.Text;
using Newtonsoft.Json;
using FileConnector.Models;
using FileConnector.Utils;
using FileConnector.ServerTasks;

namespace FileConnector.IPC
{
    class CommandClient
    {
        private string _pipeName;
        private ManualResetEvent evt;
        private NamedPipeClientStream client;

        public CommandClient(string pipeName)
        {
            _pipeName = pipeName;
            evt = new ManualResetEvent(false);

        }
        public void Start()
        {
            new Thread(Client).Start();
            evt.WaitOne();
        }

        private void ProcessResponse(ref CommandMessageResp cmResp)
        {
            Console.WriteLine($"Dcommand repsonse deserializes: {cmResp.taskName}");
            TaskRouter.RouteResponse(CommMethod.IPC, client,  ref cmResp);
        }

        void Client()
        {
            using (client = new NamedPipeClientStream(".", _pipeName, 
                                PipeDirection.InOut
                            ))
            {

                String taskName;

                Console.WriteLine("Connecting to server ...");
                client.Connect();
                client.ReadMode = PipeTransmissionMode.Message;
                Console.WriteLine("Connected.");

                Byte[] randbytes = new Byte[] {};
                while (true)
                {

                    Console.Write("-> ");
                    taskName = Console.ReadLine();

                    CommandMessageReq cmReq= new CommandMessageReq { taskName = taskName, taskType = TaskType.Reporting};

                    String cmReqSer = CommandSerializers.SerializeCommandRequest(cmReq);

                    var cmsBytes = Encoding.ASCII.GetBytes(cmReqSer);
                    Console.WriteLine("Client sends REQUEST of size {0}: Serialized as: {1}\n", cmsBytes.Length, cmReqSer);
                    try
                    {
                        client.Write(cmsBytes, 0, cmReqSer.Length);
                        client.Flush();
                    }catch(IOException sio)
                    {
                        Console.WriteLine("Send: IO exception {0}", sio.Message);
                    }

                    try
                    {
                        MemoryStream ms = new MemoryStream();
                        byte[] buffer = new byte[1024];
                        do
                        {
                            ms.Write(buffer, 0, client.Read(buffer, 0, buffer.Length));
                        } while (!client.IsMessageComplete);

                        string stringData = Encoding.UTF8.GetString(ms.ToArray());
                        Console.WriteLine("Client received RESPONSE: De-Serialized as: {0}\n", stringData);

                        CommandMessageResp cmResp = CommandSerializers.DeserializeCommandResponse(stringData);

                        ProcessResponse(ref cmResp);

                    }
                    catch(IOException sio)
                    {
                        Console.WriteLine("Receive: IO exception {0}", sio.Message);
                    }
                }
            }
        }
    }
}
