using FileConnectorKeyGen.Utils;
using System;
using System.IO;
using System.Security;
using FileConnectorCommon;

namespace FileConnectorKeyGen
{
    public static class CredKeyCtl
    {
        private static Logger log;
        static CredKeyCtl()
        {
            log = new Logger(Logger.Target.Console);
        }
        public static void Run(CredKeyOptions opts)
        {

            if (opts.Debug)
                log.SetLevel(Logger.Level.Debug);
            else
                log.SetLevel(Logger.Level.Info);

            if (opts.siteId.Length != 0)
            {
                Config.CredKey.siteId = opts.siteId;
            }

            if (opts.masterKeyHash.Length != 0)
            {
                Config.CredKey.masterKeyHash = opts.masterKeyHash;
            }

            SecureString masterKey = KeyMgmt.GetSafeConsolePassword("Enter master key: ");
            Config.CredKey.masterKeyHash = SecurePasswordHasher.Hash(KeyMgmt.SecStr2Str(masterKey), Config.CredKey.siteId);
            log.Debug($"Provided MK hash: {opts.masterKeyHash}");
            log.Debug($"Computed MK hash: {Config.CredKey.masterKeyHash}");

            if (Config.CredKey.masterKeyHash == opts.masterKeyHash)
            {
                // Console.WriteLine("Hashes match.Can continue.");
                if (opts.encrypt)
                {
                    SecureString credKey = KeyMgmt.GetSafeConsolePassword("Enter cred key to encrypt: ");
                    Config.CredKey.credKeyEnc = SecurePwEncryptor.EncryptString(
                        KeyMgmt.SecStr2Str(masterKey), KeyMgmt.SecStr2Str(credKey), Config.CredKey.siteId);
                }
                if (opts.decrypt)
                {
                    Config.CredKey.credKeyDec = SecurePwEncryptor.DecryptString(
                        KeyMgmt.SecStr2Str(masterKey), opts.enCred, Config.CredKey.siteId);
                }
            } else
            {
                Console.WriteLine("Error: Hashes do NOT match. Check your master key or master key hash");
                return;
            }


            if (opts.outFile.Length != 0)
            {
                Config.CredKey.outFile = opts.outFile;
                try
                {
                    // Console.WriteLine("opening {0}", Config.CredKey.outFile);
                    StreamWriter writer = new StreamWriter(Config.CredKey.outFile);
                    writer.WriteLine(Config.CredKey.credKeyEnc.Length == 0 ? Config.CredKey.credKeyEnc : Config.CredKey.credKeyDec); 
                    writer.Flush(); writer.Close();
                } catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                }
            }
            else
            {
                Console.WriteLine("\nAnswer: {0}", Config.CredKey.credKeyEnc.Length == 0 ? Config.CredKey.credKeyDec : Config.CredKey.credKeyEnc); 
            }
        }
    }
}
