using System;
using System.Collections.Generic;

namespace FileConnector.Models
{

    public enum CommMethod: ushort
    {
        None = 0,
        IPC = 1,
        HTTP = 2
    }
    public enum TaskType: ushort
    {
        None = 0,
        Encryption = 1,
        Reporting = 2
    }
    public enum TaskStatus : ushort
    {
        Success = 0,
        Fail = 1,
        Progress = 2,
        Pending = 3
    }


    public class CommandMessageReq
    {
        public string taskName;
        public TaskType taskType;
        public List<string> taskArgs;
        public String taskB64Payload;

        public override string ToString()
        {
            return String.Format("name: {0}, type: {1}, args: {2} payload: len {3}", taskName, taskType, taskArgs, taskB64Payload?.Length);
        }
    }

    public class CommandMessageResp
    {
        public String SiteId;
        public String taskName;
        public TaskStatus taskStatus;
        public String taskMessage;
        public String taskB64Payload;

        public override string ToString()
        {
            return String.Format("name: {0}, status: {1}, message: {2} payload: len {3}", taskName, taskStatus, taskMessage, taskB64Payload?.Length);
        }
    }


}
