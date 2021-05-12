using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using FileConnectorCommon;
using FileConnector.Models;
using Newtonsoft.Json;
using System.IO;
using System.Security;
using System.Security.Principal;
using System.Diagnostics;

namespace FileConnector.Utils
{

    static class Util
    {
        private static Logger log;

        static Util()
        {
            log = Config.ConfigLog.getLog();
        }

        public static void banner(ConsoleColor cf, ConsoleColor cb)
        {

            ConsoleColor _cf = Console.ForegroundColor;
            ConsoleColor _cb = Console.BackgroundColor;

            Console.ForegroundColor = cf;
            Console.BackgroundColor = cb;
            Console.WriteLine("***************************************************");
            Console.WriteLine("*                                                 *");
            Console.WriteLine("*      RNS v0.1 (Full Metal Jacket)               *");
            Console.WriteLine("*                                                 *");
            Console.WriteLine("***************************************************");
            Console.ForegroundColor = _cf;
            Console.BackgroundColor = _cb;
        }


        public static string Base64Decode(string base64EncodedData)
        {

            byte[] base64EncodedBytes= null;
            string decoded = String.Empty;
            try
            {
                base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            }catch(FormatException fe)
            {
                log.Debug($"b64 convert: Format exception: {fe.Message}");
            }catch (Exception e)
            {
                log.Debug($"b64 convert: Exception: {e.Message}");
            }

            try
            {
                decoded = Encoding.UTF8.GetString(base64EncodedBytes);
            }catch ( Exception e)
            {
                log.Debug($"b64 byte to string: Exception: {e.Message}");
            }
            return decoded;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            var securePassword = new SecureString();

            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }

        public static string obj2JSON(bool b64, object o)
        {
            string response = "{}";
            string json = "{}";

            try
            {
                json = JsonConvert.SerializeObject(o);
                response = json;

                if (b64)
                {
                    try
                    {
                        response = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                    }
                    catch (Exception e)
                    {
                        log.Error($"Unable to convert to b64: {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                log.Error($"Unable to convert to b64: {e.Message}");
            }

            return response;
        }


        public static string JsonPrettify(string json)
        {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }
        public static string JsonObjPrettify(object o)
        {
            var jsonPretty = JsonConvert.SerializeObject(o, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            });

            return jsonPretty;
        }


        public static void dumpSummaryTxt(TaskSummary ts)
        {
            Console.WriteLine("\n\n\n/// RNS Summary ///");
            Console.WriteLine($"Site: {ts.SiteId}");

            foreach (var hts in ts.HostTaskSummaries)
            {
                Console.WriteLine($"{hts.HostIdent}");
                foreach (var fts in hts.FileTaskSummaries)
                {
                    Console.WriteLine($"\t[{fts.Operation}] {fts.ImageTime} | {fts.PreImageName} | {fts.PreImageSize} | {fts.PreImageHash}");
                    Console.WriteLine($"\t{fts.PostImageName} | {fts.PostImageSize} | {fts.PostImageHash}");
                    Console.WriteLine();
                }
            }
        }
        public static Dictionary<string, Models.AuthN> MapHosts2AuthNs(List<Models.Host> hosts, List<Models.AuthN> authns)
        {
            Dictionary<string, Models.AuthN> h2a = new Dictionary<string, Models.AuthN>();
            foreach (var host in hosts)
            {
                try
                {
                    Models.AuthN authn = authns.FirstOrDefault(s => s.Ident == host.As);
                    h2a.Add(host.Ident, authn);
                }
                catch (ArgumentException ae)
                {
                    log.Debug($"Issue mapping host to authentication: {ae.Message}");
                }

            }

            return h2a;
        }

    }
    static class CredMgmt
    {
        public static NetworkCredential GenNetworkCreds(string user, string password, string domain)
        {
            return new NetworkCredential(user, password, domain);
        }
        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                      .IsInRole(WindowsBuiltInRole.Administrator);
        }

    }

    static class ProcessMgmt
    {

        private static Logger log;

        static ProcessMgmt()
        {
            log = Config.ConfigLog.getLog();
        }


        public static bool TerminateProcess(Process process)
        {
            try
            {
                process.Kill();

            }catch(Exception e)
            {
                log.Error($"Unable to kill process: {e.Message} {e.InnerException?.Message}");
                return false;
            }
            return true;
        }
        public static string GetSelf()
        {
            return Process.GetCurrentProcess().ProcessName;
        }
        public static bool IsDuplicateProcessRunning(string name)
        {
            var count = 0;
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    count++;
                }
            }

            if (count > 1)
            {
                return true; 
            }
            return false;
        }
    }

}
