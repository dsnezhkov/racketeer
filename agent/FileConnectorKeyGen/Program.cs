using System;
using System.Collections.Generic;
using CommandLine;

namespace FileConnectorKeyGen
{
    class Program
    {
        static void Main(string[] args)
        {

            Parser.Default.ParseArguments<
                        SiteOptions, MasterKeyOptions, FileKeyOptions, CredKeyOptions, FileOptions>(args)
                .WithParsed<SiteOptions>(SiteCtl.Run)
                .WithParsed<MasterKeyOptions>(MasterKeyCtl.Run)
                .WithParsed<FileKeyOptions>(FileKeyCtl.Run)
                .WithParsed<FileOptions>(FileCtl.Run)
                .WithParsed<CredKeyOptions>(CredKeyCtl.Run)
                .WithNotParsed(HandleParseError);

        }
        private static void HandleParseError(IEnumerable<Error> errs)
        {
            if (errs.IsVersion()) { return; }
            if (errs.IsHelp()) { return; }

            foreach (var e in errs)
            {
                Console.WriteLine($"Error: {e}");
            }

            // return;
            Environment.Exit(1);
        }
    }
}
