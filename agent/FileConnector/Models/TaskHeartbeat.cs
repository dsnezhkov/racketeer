using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConnector.Models
{
    public class TaskHeartbeatToServer
    {
        public int ProcessPid { get; set; }
        public string ProcessHost { get; set; }
        public string ProcessUser { get; set; }
        public bool isAdministrator { get; set; }
    }
}
