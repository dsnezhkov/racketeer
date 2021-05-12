using FileConnector.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Serialization;

namespace FileConnector.Utils
{
    public static class CommandSerializers
    {

        public static string SerializeCommandRequest(this CommandMessageReq toSerialize)
        {
            String commandStr = JsonConvert.SerializeObject(toSerialize,
               new JsonSerializerSettings
               {
                   NullValueHandling = NullValueHandling.Ignore,
               });

            return commandStr;

        }

        public static CommandMessageReq DeserializeCommandRequest(this string toDeserialize)
        {
            CommandMessageReq command = JsonConvert.DeserializeObject<CommandMessageReq>(toDeserialize,
               new JsonSerializerSettings
               {
                   NullValueHandling = NullValueHandling.Ignore,
               });

            return command;

          }

        public static string SerializeCommandResponse(this CommandMessageResp toSerialize)
        {
            String commandStr = JsonConvert.SerializeObject(toSerialize,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                });

            return commandStr;
        }

        public static CommandMessageResp DeserializeCommandResponse(this string toDeserialize)
        {
            CommandMessageResp command = JsonConvert.DeserializeObject<CommandMessageResp>(toDeserialize,
               new JsonSerializerSettings
               {
                   NullValueHandling = NullValueHandling.Ignore,
               });

            return command;

        }
    }
}
