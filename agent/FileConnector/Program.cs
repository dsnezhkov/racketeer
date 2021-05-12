using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CommandLine;

namespace FileConnector
{
    class FConnect
    {

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // [STAThread]
        public static void Main(string[] args)
        {

            // Parse Arguments
            Parser.Default.ParseArguments<
                ConfigOptionsServer, ConfigOptionsClient>(args)
                .WithParsed<ConfigOptionsServer>(ConfigServerCtl.Run)
                // .WithParsed<ConfigOptionsClient>(ConfigClientCtl.Run)
                .WithNotParsed(HandleParseError);
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            if (errs.IsVersion()) { return; }
            if (errs.IsHelp()) { Environment.Exit(1); }

            foreach (var e in errs)
            {
                Console.WriteLine($"Error: {e}");
            }

            Environment.Exit(2);
        }
    }
}