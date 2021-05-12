using System;
using System.Security.Cryptography;
using System.IO;
using FileConnectorCommon;

namespace FileCryptors
{

    public static class SymmetricCryptor
    {

        // private static Logger log;
        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(data);
                }
            }

            return data;
        }

        public static bool FileEncrypt(Logger log, string inputFile, string outputFile, string key)
        {

            bool state = false; 

            log.Debug($"File Cryptor {inputFile} => {outputFile}");

            FileStream fsCrypt = new FileStream(outputFile, FileMode.Create);

            //convert password string to byte arrray
            // byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(key);
            // Console.WriteLine("Password Bytes: {0}", BitConverter.ToString(passwordBytes));


            // Create enc key from key prime
            Guid? keyG = null;

            try
            {
                keyG = new Guid(key);
                log.Debug($"\nGenerated GUID : {keyG.ToString()}");
            }
            catch (Exception e)
            {
                log.Error($"\n Error in creating GUID from string: {e.Message}");
                return state;
            }
            byte[] salt = new byte[16];
            byte[] keyBytes = keyG?.ToByteArray();

            salt = GenerateRandomSalt();

            //Set Rijndael symmetric encryption algorithm
            //RijndaelManaged AES = new RijndaelManaged();
            AesManaged AES = new AesManaged();
            // AES.KeySize = 256;
            // AES.BlockSize = 128;
            // AES.Padding = PaddingMode.PKCS7;
            AES.Padding = PaddingMode.Zeros;

            //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
            var keyDer = new Rfc2898DeriveBytes(keyBytes, salt, 50000);
            log.Debug($"Derived Key: {BitConverter.ToString(keyDer.GetBytes(32))}" );
            AES.Key = keyDer.GetBytes(AES.KeySize / 8);
            AES.IV = keyDer.GetBytes(AES.BlockSize / 8);

            //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
            // AES.Mode = CipherMode.CFB;
            AES.Mode = CipherMode.ECB;

            // write salt to the begining of the output file, so in this case can be random every time
            log.Debug($"Wrote salt {BitConverter.ToString(salt)} of size: {salt.Length}");
            fsCrypt.Write(salt, 0, salt.Length);

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);
            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            //create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
            byte[] buffer = new byte[1048576];
            int read;

            try
            {
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cs.Write(buffer, 0, read);
                }

                log.Debug($"Flushed final block");
                cs.FlushFinalBlock();

                state = true;

                // Close up
                fsIn.Close();

            }
            catch (Exception ex)
            {
                log.Error($"File I/O Error: {ex.Message}");
                return state;
            }
            finally
            {
                cs.Close();
                fsCrypt.Close();
            }

            return state;
        }

        /// <summary>
        /// Decrypts an encrypted file with the FileEncrypt method through its path and the plain password.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="password"></param>
        public static bool FileDecrypt(Logger log, string inputFile, string outputFile, string key )
        {

            bool state = false; 

            //byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(pasword);
            // Console.WriteLine("Password Bytes: {0}", BitConverter.ToString(passwordBytes));


            // Create enc key from key prime
            Guid? keyG = null;

            try
            {
                keyG = new Guid(key);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n Error in creating GUID from string: {0}", e.Message);
            }

            log.Debug($"\nDec key: {keyG.ToString()}");
            byte[] salt = new byte[16];
            byte[] keyBytes = keyG?.ToByteArray();

            salt = GenerateRandomSalt();

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);
            Console.WriteLine($"Read salt {BitConverter.ToString(salt)} of size {salt.Length}");

            //RijndaelManaged AES = new RijndaelManaged();
            AesManaged AES = new AesManaged();
            // AES.KeySize = 256;
            // AES.BlockSize = 128;
            var keyDer = new Rfc2898DeriveBytes(keyBytes, salt, 50000);

            log.Debug($"Derived Key: {BitConverter.ToString(keyDer.GetBytes(32))}");
            AES.Key = keyDer.GetBytes(AES.KeySize / 8);
            AES.IV = keyDer.GetBytes(AES.BlockSize / 8);
            // AES.Padding = PaddingMode.PKCS7;
            AES.Padding = PaddingMode.Zeros;
            // AES.Mode = CipherMode.CFB;
            AES.Mode = CipherMode.ECB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int read;
            byte[] buffer = new byte[1048576];

            try
            {
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Application.DoEvents();
                    fsOut.Write(buffer, 0, read);
                }

                state = true;
            }
            catch (CryptographicException ex_CryptographicException)
            {
                Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error by closing CryptoStream: " + ex.Message);
            }
            finally
            {
                fsOut.Close();
                fsCrypt.Close();
            }

            return state;
        }
    }


}
