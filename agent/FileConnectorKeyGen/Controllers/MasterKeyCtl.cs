using FileConnectorKeyGen.Utils;
using System;
using System.IO;
using System.Security;
using FileConnectorCommon;

namespace FileConnectorKeyGen
{
    public static class MasterKeyCtl
    {
        private static Logger log;
        static MasterKeyCtl()
        {
            log = new Logger(Logger.Target.Console);
        }

        public static void Run(MasterKeyOptions opts)
        {

            if (opts.Debug)
                log.SetLevel(Logger.Level.Debug);
            else
                log.SetLevel(Logger.Level.Info);

            if (opts.siteId.Length != 0)
            {
                Config.MasterKey.siteId = opts.siteId;
                SecureString masterKey = KeyMgmt.GetSafeConsolePassword("Enter master key: ");

                log.Debug($"\nPlain master key is: {masterKey}");

                Config.MasterKey.masterKeyHash = SecurePasswordHasher.Hash(KeyMgmt.SecStr2Str(masterKey), Config.MasterKey.siteId);
            }
            log.Debug("Site Id: {siteId}");
            log.Debug($"{opts.outFile}, {opts.outFile.Length}");
            if (opts.outFile.Length != 0)
            {
                Config.MasterKey.outFile = opts.outFile;
                try
                {
                    log.Debug($"opening {Config.MasterKey.outFile}");
                    StreamWriter writer = new StreamWriter(Config.MasterKey.outFile);
                    writer.WriteLine(Config.MasterKey.masterKeyHash); writer.Flush(); writer.Close();
                } catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                }
            }
            else
            {
                Console.WriteLine("\n{0}", Config.MasterKey.masterKeyHash);
            }
        }
    }
}
