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
    class ServerConnector
    {
        private string _pipeName;
        private ManualResetEvent evt;

        public ServerConnector(string pipeName)
        {
            _pipeName = pipeName;
            evt = new ManualResetEvent(false);

        }
        public void Start()
        {
            new Thread(Server).Start();
            evt.WaitOne();
        }
        private void ProcessRequestFromClient(ref CommandMessageReq cmReq, ref CommandMessageResp cmResp)
        {

            Console.WriteLine($"Dcommand to deserialize: {cmReq.taskName}");
            TaskRouter.RouteRequest(CommMethod.IPC, ref cmReq, ref cmResp);
        }

        void Server()
        {
            using (NamedPipeServerStream server = new NamedPipeServerStream(_pipeName, 
                                PipeDirection.InOut,
                                1,
                                PipeTransmissionMode.Message
                            ))
            {
                evt.Set();
                Console.WriteLine("Waiting for client connection ...");
                server.WaitForConnection();
                Console.WriteLine("New client at the door");

                    while (true)
                    { 
                        // TODO: We can leave Memory streams with data, as we could:
                        // - write memory stream to MemoryMappedIO non-file backed store and share over IPC
                        // - convert to something else
                        MemoryStream ms = new MemoryStream();
                        try
                        {
                            byte[] buffer = new byte[1024];
                            var sb = new StringBuilder();
                            int read;

                            do
                            {
                                read = server.Read(buffer, 0, buffer.Length);
                                Console.WriteLine("Server received length {0}\n", read);
                                sb.Append(Encoding.ASCII.GetString(buffer, 0, read));

                            } while (read > 0 && !server.IsMessageComplete);

                            Console.WriteLine("Pre- memory IO string buffer: {0}, read: {1}", sb.ToString(), read);
                            using (var writer = new StreamWriter(ms))
                            {
                                writer.Write(sb);
                            }
                        } catch (IOException sio){
                            Console.WriteLine("Receive: IO exception {0}", sio.Message);
                        }

                        string stringData = Encoding.UTF8.GetString(ms.ToArray());
                        Console.WriteLine("Server received REQUEST: Serialized as: {0}\n", stringData);


                        CommandMessageReq cmReq = CommandSerializers.DeserializeCommandRequest(stringData);
                        CommandMessageResp cmResp = new CommandMessageResp();

                        ProcessRequestFromClient(ref cmReq, ref cmResp);

                        // Response
                        String sms = CommandSerializers.SerializeCommandResponse(cmResp);

                        var smsBytes = Encoding.ASCII.GetBytes(sms);
                        Console.WriteLine("Server sends RESPONSE: Serialized as: {0}\n", sms);

                        try
                        {
                            server.Write(smsBytes, 0, smsBytes.Length);
                            server.Flush();
                        }catch (IOException sio){
                            Console.WriteLine("Receive: IO exception {0}", sio.Message);
                        }

                        Thread.Sleep(1000); // TODO: tune
                    }
            }
        }

    }
}
