using FileConnector.Models;
using FileConnector.Utils;
using FileConnectorCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Security;

namespace FileConnector.ServerTasks.Impl
{
    class TaskRunPolicy
    {
        private Logger log;
        private DriverConfig cfTree;

        private string masterPassHash;
        private string masterPassHashInput;
        private string siteId;

        private string domain;
        private string user;
        private string password;
        private string passwordDec;
        private string networkPath = "";
        private string encKey;
        private bool delSrcFile = true;
        private string OSErrMsg;
        private int OSErrno = 0;

        private Operation operation;
        private AuthType authType;
        private NetworkCredential credentials;

        private static TaskSummary taskSummary;

        public TaskRunPolicy()
        {
            log = Config.ConfigLog.getLog();
            Config.TaskSummaries.setTaskSummary(new TaskSummary());
            taskSummary = Config.TaskSummaries.getTaskSummary();
        }
        public void Run()
        {
            log.Debug("Loading configuration tree");
            cfTree = Config.ConfigFile.getCfg();
            if (cfTree == null)
            {
                log.Debug("Unable to parse config. Check format of the config file.");
                return;
            }

            masterPassHash = cfTree.Keys.MasterKey;
            siteId = cfTree.Keys.SiteID;
            taskSummary.SiteId = siteId;

            log.Debug($"MasterKey: {masterPassHash}, SiteID: {siteId}");

            // Useful mappings
            List<Models.Host> hosts = cfTree.Hosts;
            List<Models.AuthN> authns = cfTree.AuthN;

            log.Debug("Mapping hosts to authentication");
            Dictionary<string, Models.AuthN> hosts2authns = Util.MapHosts2AuthNs(hosts, authns);

            // Interactive master password
            // TODO: This may need to be sources in from the network

            /*
            log.Stop();
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("\n");
            SecureString masterPassInput = KeyMgmt.GetSafeConsolePassword("Enter master password: ");
            log.Start();
            */
            SecureString masterPassInput = Config.MasterKey.getMK();
            if (masterPassInput == null)
            {
                log.Debug($"MasterKey is not set. Not continuing");
                return;
            }

            masterPassHashInput = SecurePasswordHasher.Hash(KeyMgmt.SecStr2Str(masterPassInput), siteId);
            log.Debug($"MasterKeyHash: {masterPassHashInput}");

            if (masterPassHashInput != masterPassHash)
            {
                log.Error("Master key hashes do not match. Unauthorized.");
                return;
            }

            // Iterate over hosts and execute work on files
            log.Debug("Iterating over hosts");

            taskSummary.HostTaskSummaries = new List<HostTaskSummary>();
            foreach (var host in hosts)
            {
                log.Debug($"\n [[ Entering host : {host.Ident} ({host.Name}) ]]");

                // For additional security Pin the password of your files
                // GCHandle gchDec = GCHandle.Alloc(passwordDec, GCHandleType.Pinned);

                // To increase the security of the decryption, delete the used password from the memory !
                // Util.Interop.ZeroMemory(gchDec.AddrOfPinnedObject(), passwordDec.Length * 2);
                // gchDec.Free();

                HostTaskSummary hts = new HostTaskSummary();
                hts.HostIdent = host.Ident;

                bool localhost = false;
                password = hosts2authns[host.Ident].Password;
                user = hosts2authns[host.Ident].User;
                domain = hosts2authns[host.Ident].Domain;
                authType = hosts2authns[host.Ident].Type;
                ConnectToSharedFolder connector;

                switch (authType)
                {
                    case AuthType.None:
                        log.Debug($"Auth: None");
                        break;
                    case AuthType.NetworkExplicit:
                        log.Debug($"Auth: Explicit");
                        log.Debug($"Parsed creds: {domain}\\{user}:{password}");

                        networkPath = @"\\" + host.Name;
                        log.Debug($"Built networkPath as {networkPath}");

                        // Decrypt stored password
                        passwordDec = SecurePwEncryptor.DecryptString(KeyMgmt.SecStr2Str(masterPassInput), password, siteId);
                        log.Debug("Decrypted credentials");

                        // Specified user token
                        log.Debug("Building supplied user impersonation NetworkCredential");
                        credentials = CredMgmt.GenNetworkCreds(user, passwordDec, domain);

                        // Connect to host
                        try
                        {
                            log.Info($"Connecting to {networkPath}");
                            connector = new ConnectToSharedFolder(networkPath, credentials);
                            log.Debug($"Connected as: {credentials.Domain}\\{credentials.UserName}");

                        }
                        catch (Win32Exception we)
                        {

                            OSErrMsg = Interop.GetLastErrorString();
                            OSErrno = Interop.GetLastError();

                            // Multiple connections to a server or shared resource by the same user
                            // Still possible the connection is alive. 
                            if (OSErrno == 1219)
                            {
                                log.Warn($"Will attempt to proceed after: {we.Message}: {OSErrMsg}. " +
                                    $"\n For the future do not mix authentication principals on the same network resource");
                            }
                            else
                            {
                                log.Error($"Win32Exception: ({OSErrno}): Cannot connect to {we}: {OSErrMsg}");
                                continue;
                            }
                        }
                        catch (Exception e)
                        {
                            log.Error($"Exception: {e.Message}");
                            continue;
                        }
                        break;
                    case AuthType.NetworkImplicit:

                        log.Debug($"Auth: Implicit");
                        networkPath = @"\\" + host.Name;
                        log.Debug($"Built networkPath as {networkPath}");


                        // Connect to host
                        try
                        {
                            log.Info($"Connecting to {networkPath}");
                            connector = new ConnectToSharedFolder(networkPath);
                            log.Debug($"Connected as: self");

                        }
                        catch (Win32Exception we)
                        {
                            OSErrMsg = Interop.GetLastErrorString();
                            OSErrno = Interop.GetLastError();

                            // Multiple connections to a server or shared resource by the same user
                            // Still possible the connection is alive. 
                            if (OSErrno == 1219)
                            {
                                log.Warn($"Will attempt to proceed after: {we.Message}: {OSErrMsg}. " +
                                    $"\n For the future do not mix authentication principals on the same network resource");
                            }
                            else
                            {
                                log.Error($"Win32Exception: ({OSErrno}): Cannot connect to {we}: {OSErrMsg}");
                                continue;
                            }
                        }
                        catch (Exception e)
                        {
                            log.Error($"Exception: {e.Message}");
                            continue;
                        }
                        break;
                    case AuthType.Local:

                        log.Debug($"Auth: Local");
                        // Local machine impersonation
                        log.Debug("Building local user impersonation NetworkCredential");
                        localhost = true;
                        networkPath = String.Empty;
                        break;
                    default:
                        break;
                }

                log.Debug("Iterating over files in host");
                hts.FileTaskSummaries = new List<FileTaskSummary>();
                FileTaskSummary fts;

                foreach (var file in host.Files)
                {

                    fts = new FileTaskSummary();

                    // JSON may have forward slashes. Canonicalize.
                    string fileToEncrypt = file.FilePath.Replace("/", @"\");

                    if (!localhost)
                    {
                        fileToEncrypt = networkPath + fileToEncrypt;
                    }

                    // Supercede host directives with file directives if available
                    encKey = (file.Key != string.Empty) ? file.Key : host.Key;
                    operation = (file.Operation != Operation.None) ? file.Operation : host.Operation;
                    Cryptor.cryptFileKickOff(fileToEncrypt, encKey, operation, ref fts, delSrcFile);

                    hts.FileTaskSummaries.Add(fts);
                }

                taskSummary.HostTaskSummaries.Add(hts);
            }
        }
    }
}
