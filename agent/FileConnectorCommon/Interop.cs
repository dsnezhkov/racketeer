using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FileConnectorCommon
{
    static class Interop
    {

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //  Call this function to remove the key from memory after use for security
        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);


        public static int GetLastError()
        {
            return (Marshal.GetLastWin32Error());
        }

        public static string GetLastErrorString()
        {
            int lastError = GetLastError();
            if (0 == lastError) return ("");
            else
            {
                StringBuilder msgOut = new StringBuilder(256);
                int size = FormatMessage(FORMAT_MESSAGE.ALLOCATE_BUFFER | FORMAT_MESSAGE.FROM_SYSTEM | FORMAT_MESSAGE.IGNORE_INSERTS,
                              IntPtr.Zero, lastError, 0, out msgOut, msgOut.Capacity, IntPtr.Zero);
                return (msgOut.ToString().Trim());
            }
        }

        enum FORMAT_MESSAGE : uint
        {
            ALLOCATE_BUFFER = 0x00000100,
            IGNORE_INSERTS = 0x00000200,
            FROM_SYSTEM = 0x00001000,
            ARGUMENT_ARRAY = 0x00002000,
            FROM_HMODULE = 0x00000800,
            FROM_STRING = 0x00000400
        }

        [DllImport("kernel32.dll")]
        static extern uint FormatMessage(uint dwFlags, IntPtr lpSource,
        uint dwMessageId, uint dwLanguageId, [Out] StringBuilder lpBuffer,
        uint nSize, IntPtr Arguments);

        // the version, the sample is built upon:
        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern uint FormatMessage(uint dwFlags, IntPtr lpSource,
           uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer,
           uint nSize, IntPtr pArguments);

        // the parameters can also be passed as a string array:
        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern uint FormatMessage(uint dwFlags, IntPtr lpSource,
           uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer,
           uint nSize, string[] Arguments);

        // simplified for usability
        [DllImport("kernel32.dll")]
        static extern int FormatMessage(
            FORMAT_MESSAGE dwFlags,
            IntPtr lpSource,
            int dwMessageId,
            uint dwLanguageId,
            out StringBuilder msgOut,
            int nSize,
            IntPtr Arguments
        );


        [DllImport("mpr.dll")]
        public static extern int WNetAddConnection2(NetResource netResource,
          string password, string username, int flags);

        [DllImport("mpr.dll")]
        public static extern int WNetCancelConnection2(string name, int flags,
            bool force);

        [StructLayout(LayoutKind.Sequential)]
        public class NetResource
        {
            public ResourceScope Scope;
            public ResourceType ResourceType;
            public ResourceDisplaytype DisplayType;
            public int Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        public enum ResourceScope : int
        {
            Connected = 1,
            GlobalNetwork,
            Remembered,
            Recent,
            Context
        };

        public enum ResourceType : int
        {
            Any = 0,
            Disk = 1,
            Print = 2,
            Reserved = 8,
        }

        public enum ResourceDisplaytype : int
        {
            Generic = 0x0,
            Domain = 0x01,
            Server = 0x02,
            Share = 0x03,
            File = 0x04,
            Group = 0x05,
            Network = 0x06,
            Root = 0x07,
            Shareadmin = 0x08,
            Directory = 0x09,
            Tree = 0x0a,
            Ndscontainer = 0x0b
        }
    }
}
