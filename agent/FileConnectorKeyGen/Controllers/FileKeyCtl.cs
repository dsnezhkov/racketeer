using FileConnectorCommon;
using FileConnectorKeyGen.Utils;
using System;
using System.IO;

namespace FileConnectorKeyGen
{
    public static class FileKeyCtl
    {
        private static Logger log;
        static FileKeyCtl()
        {
            log = new Logger(Logger.Target.Console);
        }
        public static void Run(FileKeyOptions opts)
        {

            if (opts.Debug)
                log.SetLevel(Logger.Level.Debug);
            else
                log.SetLevel(Logger.Level.Info);

            if (opts.fkCount == 0)
            {
                Config.FileKey.fileKey.Add(Guid.NewGuid().ToString());
            }else {
                for (int i = 0; i < opts.fkCount; i++)
                {
                    Config.FileKey.fileKey.Add(Guid.NewGuid().ToString());
                }
            }

            if(opts.outFile.Length != 0)
            {
                Config.FileKey.outFile = opts.outFile;
                try
                {
                    log.Debug($"opening {Config.FileKey.outFile}");
                    StreamWriter writer = new StreamWriter(Config.FileKey.outFile);
                    foreach (string fKey in Config.FileKey.fileKey)
                    {
                        writer.WriteLine(fKey); 
                    }
                    writer.Flush(); writer.Close();
                } catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                }
            }
            else
            {
                foreach (string fKey in Config.FileKey.fileKey)
                {
                    Console.WriteLine(fKey); 
                }
            }
        }
    }
}
