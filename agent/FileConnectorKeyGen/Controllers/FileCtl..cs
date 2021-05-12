using FileConnectorKeyGen.Utils;
using System;
using System.IO;
using System.Security;
using FileConnectorCommon;

namespace FileConnectorKeyGen
{
    public static class FileCtl
    {
        private static Logger log;
        static FileCtl()
        {
            log = new Logger(Logger.Target.Console);
        }
        public static void Run(FileOptions opts)
        {

            if (opts.Debug)
                log.SetLevel(Logger.Level.Debug);
            else
                log.SetLevel(Logger.Level.Info);

            if (opts.outFile.Length != 0)
                Config.File.outFile = opts.outFile;
            if (opts.inFile.Length != 0)
                Config.File.inFile = opts.inFile;

            if (opts.fileKey.Length != 0)
                Config.File.fileKey = opts.fileKey;

            if (opts.encrypt)
            {
                Console.WriteLine("Encrypting ... ");
                FileCryptors.SymmetricCryptor.FileEncrypt(log, Config.File.inFile, Config.File.outFile, Config.File.fileKey);
            }
            if (opts.decrypt)
            {
                Console.WriteLine("Decrypting ... ");
                FileCryptors.SymmetricCryptor.FileDecrypt(log, Config.File.inFile, Config.File.outFile, Config.File.fileKey);
            }

        }
    }
}
