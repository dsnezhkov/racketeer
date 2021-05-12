using CommandLine;

namespace FileConnector
{

    #region ConfigOptions
    interface IConfigOptionsLog
    {
        [Option('d', "debug", Required = false, HelpText = "Debug output.")]
        bool Debug { get; set; }
        [Option('l', "log", Required = false, Default = "m", 
            HelpText = 
            "Log to source: (m)emory, (c)onsole\n" +
            "Example: -l m"
            )]
        string LogSource { get; set; }
        [Option('s', "showcon", Required = false, Default = false, HelpText = "Show hidden console")]
        bool ShowCon { get; set; }
    }
    interface IConfigOptions
    {

        [Option('c', "cfg", Required = false,
            HelpText =
            "Config file\n" +
            "Example: -c config_file "
            )]
        string cfgFile { get; set; }
    }
    interface IConfigOptionsServer
    {
        [Option('p', "pipe", Required = false, HelpText = "IPC Pipe name.")]
        string cmdIPCPipe { get; set; }
        bool cmdServer { get; set; }
    }

    interface IConfigOptionsClient
    {
        [Option('p', "pipe", Required = false, HelpText = "IPC Pipe name.")]
        string cmdIPCPipe { get; set; }
        bool cmdClient { get; set; }
    }

    [Verb("server", isDefault: true, HelpText = "Server Role")]
    public class ConfigOptionsServer : IConfigOptions,  IConfigOptionsServer, IConfigOptionsLog
    {
        public string cfgFile { get; set; } = string.Empty;
        public string cmdIPCPipe { get; set; }
        public bool cmdServer { get; set; } = true;
        public bool Debug { get; set; } = false;
        public bool ShowCon { get; set; } = false;
        public string LogSource { get; set; } = string.Empty;
    }

    [Verb("client", isDefault: false, HelpText = "Client Role")]
    public class ConfigOptionsClient :  IConfigOptionsClient
    {
        public string cmdIPCPipe { get; set; }
        public bool cmdClient { get; set; } = true;
        public bool Debug { get; set; } = false;
    }

    #endregion

}
