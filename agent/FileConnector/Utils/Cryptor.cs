using System;
using System.IO;
using System.Security.Cryptography;
using FileConnector.Models;
using FileConnectorCommon;

namespace FileConnector.Utils
{
    public static class Cryptor
    {
        private static Logger log;
        static Cryptor()
        {
            log = Config.ConfigLog.getLog();
        }

        private static bool fileExists(string filePath)
        {
            return System.IO.File.Exists(filePath);
        }
        public static long getFileSize(string filePath)
        {
            if ( fileExists(filePath))
            {
                return new FileInfo(filePath).Length;
            }

            return 0;
        }
        private static string SHA256CheckSum(string filePath, bool b64)
        {
            using (SHA256 SHA256 = SHA256Managed.Create())
            {
                using (FileStream fileStream = System.IO.File.OpenRead(filePath))
                {
                    byte[] checksum = SHA256.ComputeHash(fileStream);
                    if (b64)
                    {
                        return Convert.ToBase64String(checksum);
                    }
                    else
                    {
                        return BitConverter.ToString(checksum).Replace("-", String.Empty);
                    }
                }
            }
        }

        public static string hashFile(string fileIn)
        {
            if (Cryptor.fileExists(fileIn))
            {
                return SHA256CheckSum(fileIn, false);
            }

            return String.Empty;
        }

        public static bool decFile(string fileIn, string fileOut, string key, bool delSrc)
        {
            bool decSuccess = false;
            log.Debug($"Decrypting file {fileIn} -> {fileOut}");


            if (Cryptor.fileExists(fileIn))
            {
                log.Debug($"File {fileIn} found");
                log.Debug("Invoking cryptor");
                decSuccess = FileCryptors.SymmetricCryptor.FileDecrypt(log, fileIn, fileOut, key);

                if (decSuccess)
                {
                    log.Debug("Decryption routine success");
                    if (delSrc)
                    {
                        try
                        {
                            log.Debug($"Deleting original file {fileIn}");
                            System.IO.File.Delete(fileIn);
                        }
                        catch (Exception e)
                        {
                            log.Error($"Unable to delete file {fileIn}, Details: {e}");
                        }
                    }
                }
                else
                {
                    log.Error($"Deccryption routine failed");
                }
            }
            else
            {
                log.Error($"File {fileIn} not found");
            }

            return decSuccess;
        }
        public static bool encFile(string fileIn, string fileOut, string key, bool delSrc)
        {
            bool encSuccess = false;
            log.Debug($"Encrypting file {fileIn} -> {fileOut}");

            // connector.FileUpload(filePath, networkPath, credentials);

            if (Cryptor.fileExists(fileIn))
            {
                log.Debug($"File {fileIn} found");
                log.Debug("Invoking cryptor");

                encSuccess = FileCryptors.SymmetricCryptor.FileEncrypt(log, fileIn, fileOut, key);

                if (encSuccess)
                {
                    log.Debug("Encryption routine success");

                    if (delSrc)
                    {
                        try
                        {
                            log.Debug($"Deleting original file {fileIn}");
                            System.IO.File.Delete(fileIn);
                        }
                        catch (Exception e)
                        {
                            log.Error($"Unable to delete file {fileIn}, Details: {e}");
                        }
                    }
                }
                else
                {
                    log.Error($"Encryption routine failed");
                }
            }
            else
            {
                log.Error($"File {fileIn} not found");
            }

            return encSuccess;
        }


        public static bool cryptFileKickOff(string fileStem, string encKey, Operation op, ref FileTaskSummary fts, bool delSrcFile)
        {

            bool success = false;
            string fileSrc = "";
            string fileDst = "";

            fts.Operation = op;


            switch (op)
            {
                case Operation.None:
                    log.Warn($"NOP operation on {fileStem}");
                    break;
                case Operation.Encrypt:
                    fileSrc = fileStem;
                    fileDst = fileStem + ".enc";

                    fts.PreImageName = fileSrc;
                    fts.PreImageHash = Cryptor.hashFile(fileSrc);
                    fts.PreImageSize = Cryptor.getFileSize(fileSrc);

                    log.Debug($"Encrypting {fileSrc} => { fileDst} key: {encKey}");
                    success = Cryptor.encFile(fileSrc, fileDst, encKey, delSrcFile);

                    break;
                case Operation.Decrypt:
                    fileSrc = fileStem + ".enc";
                    fileDst = fileStem;

                    fts.PreImageName = fileSrc;
                    fts.PreImageHash = Cryptor.hashFile(fileSrc);
                    fts.PreImageSize = Cryptor.getFileSize(fileSrc);

                    log.Debug($"Decrypting {fileSrc} => { fileDst} key: {encKey}");
                    success = Cryptor.decFile(fileSrc, fileDst, encKey, delSrcFile);
                    break;
                default:
                    log.Error($"Unknown operation submitted");
                    break;
            }

            if (success)
            {
                fts.ImageTime = DateTime.Now.ToString();
                fts.PostImageName = fileDst;
                fts.PostImageHash = Cryptor.hashFile(fileDst);
                fts.PostImageSize = Cryptor.getFileSize(fileDst);
            }
            else
            {
                log.Debug($"Unable to properly encrypt file: {fileSrc}");
            }

            return success;
        }
    } 
}
