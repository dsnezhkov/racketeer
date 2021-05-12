using FileConnectorCommon;
using FileConnectorKeyGen.Utils;
using System;
using System.IO;

namespace FileConnectorKeyGen
{
    public static class SiteCtl
    {
        private static Logger log;
        static SiteCtl()
        {
            log = new Logger(Logger.Target.Console);
        }
        public static void Run(SiteOptions opts)
        {

            if (opts.Debug)
                log.SetLevel(Logger.Level.Debug);
            else
                log.SetLevel(Logger.Level.Info);

            string siteId = Guid.NewGuid().ToString();
            Config.SiteId.siteId = siteId;
               
            log.Debug($"Site id: {siteId}");
            log.Debug($"{opts.outFile}, {opts.outFile.Length}");
            if (opts.outFile.Length != 0)
            {
                Utils.Config.SiteId.outFile = opts.outFile;
                try
                {
                    log.Debug($"opening {Config.SiteId.outFile}");
                    StreamWriter writer = new StreamWriter(opts.outFile);
                    writer.WriteLine(siteId); writer.Flush(); writer.Close();
                } catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                }
            }
            else
            {
                Console.WriteLine("{0}", Utils.Config.SiteId.siteId);
            }
        }
    }
}
