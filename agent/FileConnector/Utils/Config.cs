using FileConnector.Models;
using FileConnectorCommon;
using System.Security;

namespace FileConnector.Utils
{
    public enum Location : ushort
    {
        External = 1,
        Embedded = 2,
        Network = 3,
    }

    public static class Config
    {
        public static class ConfigFile
        {
            public static string cfgFile = string.Empty;
            public static Location location = Location.Embedded;
            private static DriverConfig _cfgTree;


            public static void setCfg(DriverConfig cfgTree)
            {
                _cfgTree = cfgTree;
            }
            public static DriverConfig getCfg()
            {
                return _cfgTree;
            }
        }

        public static class MasterKey
        {
            private static SecureString _mk;

            public static void setMK(SecureString mk)
            {
                _mk = mk;
            }
            public static SecureString getMK()
            {
                return _mk;
            }
        }

        public static class IPCServer
        {
            public static string pipeName = "rns";
        }
        public static class IPCClient
        {
            public static string pipeName = "rns";
        }

        public static class HttpClient
        {
            private static ExtEndpoint _ep;
            private static ExtComm _ec;
            public static void setEp(ExtEndpoint ep)
            {
                _ep = ep;
            }
            public static ExtEndpoint getEp()
            {
                return _ep;
            }

            public static void setEc(ExtComm ec)
            {
                _ec = ec;
            }
            public static ExtComm getEc()
            {
                return _ec;
            }
        }

        public static class ConfigLog
        {
            private static Logger _log;
            public static void setLog(Logger log)
            {
                _log = log;
            }
            public static Logger getLog()
            {
                return _log;
            }

        }

        public static class ConfigProcess
        {
            private static System.IntPtr _handle;
            public static void setHandle(System.IntPtr handle)
            {
                _handle = handle;
            }
            public static System.IntPtr getHandle()
            {
                return _handle;
            }
        }


        public static class TaskSummaries
        {
            private static TaskSummary _ts;
            public static TaskSummary setTaskSummary(TaskSummary ts)
            {
                _ts = ts;
                return _ts;
            }
            public static TaskSummary getTaskSummary()
            {
                return _ts;
            }
        }

    }
}
