using System;
using System.Collections.Generic;
using CommandLine;

namespace FileConnectorKeyGen
{

    interface ICommonOptions
    {
        [Option('d', "debug", Required = false, HelpText = "Debug output.")]
        bool Debug { get; set; }
        [Option('l', "log", Required = false, Default = "c",
            HelpText =
            "Log to source: (c)onsole\n" +
            "Example: -l c"
            )]
        string LogSource { get; set; }
    }

    #region SiteOptions
    interface ISiteOptions
    {

        [Option('o', "out", Required = false,
            HelpText =
            "Generate new site ID, saving to file\n" +
            "Example: -o filename "
            )]
        string outFile { get; set; }
    }

    [Verb("site", HelpText = "Generate Site ID")]
    public class SiteOptions : ISiteOptions, ICommonOptions
    {
        public string outFile { get; set; } = String.Empty;
        public string LogSource { get; set; }
        public bool Debug { get; set; }
    }
    #endregion

    #region MasterKeyOptions
    interface IMasterKeyOptions
    {

        [Option('o', "out", Required = false,
            HelpText =
            "Save MK hash to file\n" +
            "Example: -o filename "
            )]
        string outFile { get; set; }

        [Option('s', "site", Required = true,
            HelpText =
            "Provide site id to prime MK\n" +
            "Example: -s GUID "
            )]
        string siteId { get; set; }
    }

    [Verb("masterkey", HelpText = "Generate master key for site")]
    public class MasterKeyOptions : IMasterKeyOptions, ICommonOptions
    {
        public string outFile { get; set; } = String.Empty;
        public string siteId { get; set; } = String.Empty;
        public string LogSource { get; set; }
        public bool Debug { get; set; }
    }
    #endregion

    #region FileKeyOptions
    interface IFileKeyOptions
    {

        [Option('o', "out", Required = false,
            HelpText =
            "Save keys to file\n" +
            "Example: -o filename "
            )]
        string outFile { get; set; }

        [Option('c', "count", Required = false,
            HelpText =
            "Generate a number of keys\n" +
            "Example: -c <number> "
            )]
        int fkCount { get; set; }
    }

    [Verb("filekey", HelpText = "Generate one or more of file keys")]
    public class FileKeyOptions : IFileKeyOptions
    {
        public string outFile { get; set; } = string.Empty;
        public int fkCount { get; set; } = 0;
        public string LogSource { get; set; }
        public bool Debug { get; set; }
    }
    #endregion

    #region CredKeyOptions
    interface ICredKeyEncOptions
    {
        
        [Option('E', "encrypt", Required = true, SetName = "encrypt",
            HelpText =
            "Encrypt action\n" +
            "Example: -E "
            )]
        bool encrypt { get; set; }
    }
    interface ICredKeyDecOptions
    {
        [Option('D', "decrypt", Required = true, SetName = "decrypt",
            HelpText =
            "Decrypt action\n" +
            "Example: -D "
            )]
        bool decrypt { get; set; }

        [Option('e', "encred", Required = true, SetName = "decrypt",
            HelpText =
            "Encrypted credential to decrypt and/or verify\n" +
            "Example: -e <encryped cred string> "
            )]
        string enCred { get; set; }
    }
    interface ICredKeyOptions
    {

        [Option('o', "out", Required = false,
            HelpText =
            "Save encrypted credential to file\n" +
            "Example: -o filename "
            )]
        string outFile { get; set; }

        [Option('s', "site", Required = true,
            HelpText =
            "Provide site id to prime MK\n" +
            "Example: -s GUID "
            )]
        string siteId { get; set; }

        [Option('m', "mhash", Required = true,
            HelpText =
            "Provide master key hash to verify match\n" +
            "Example: -m $RNS$V1$10000$XfTStpyXz1F/0HALCVk6wXG4MxJXGTyMqZtxWOLZ+7aoc4Da "
            )]
        string masterKeyHash { get; set; }
    }

    [Verb("credkey", HelpText = "Secure cred storage management")]
    public class CredKeyOptions : ICredKeyEncOptions, ICredKeyDecOptions, ICredKeyOptions, ICommonOptions
    {
        public bool decrypt { get; set; } = false;
        public bool encrypt { get; set; } = false;
        public string enCred { get; set; } = string.Empty;
        public string outFile { get; set; } = string.Empty;
        public string siteId { get; set; } = string.Empty;
        public string masterKeyHash { get; set; } = string.Empty;
        public string LogSource { get; set; }
        public bool Debug { get; set; }
    }
    #endregion

    #region FileOptions
    interface IFileEncOptions
    {

        [Option('E', "encrypt", Required = true, SetName = "file_encrypt",
            HelpText =
            "Encrypt action\n" +
            "Example: -E "
            )]
        bool encrypt { get; set; }
    }
    interface IFileDecOptions
    {
        [Option('D', "decrypt", Required = true, SetName = "file_decrypt",
            HelpText =
            "Decrypt action\n" +
            "Example: -D "
            )]
        bool decrypt { get; set; }

    }
    interface IFileOptions
    {

        [Option('o', "out", Required = true,
            HelpText =
            "Sink file\n" +
            "Example: -o filename "
            )]
        string outFile { get; set; }

        [Option('i', "in", Required = true,
            HelpText =
            "Source file\n" +
            "Example: -o filename "
            )]
        string inFile { get; set; }

        [Option('k', "key", Required = true,
            HelpText =
            "Provide file key\n" +
            "Example: -k <key> "
            )]
        string fileKey { get; set; }

    }

    [Verb("file", HelpText = "Secure file management")]
    public class FileOptions : IFileEncOptions, IFileDecOptions, IFileOptions
    {
        public bool decrypt { get; set; } = false;
        public bool encrypt { get; set; } = false;
        public string inFile { get; set; } = string.Empty;
        public string outFile { get; set; } = string.Empty;
        public string fileKey { get; set; } = string.Empty;
        public string LogSource { get; set; }
        public bool Debug { get; set; }
    }
    #endregion

}
