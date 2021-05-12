using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static FileConnectorCommon.Interop;

namespace FileConnector
{
    public class ConnectToSharedFolder : IDisposable
    {
        readonly string _networkName;


        public ConnectToSharedFolder(string networkName)
        {
            _networkName = networkName;

            var netResource = new NetResource
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkName
            };

            var result = WNetAddConnection2(
                netResource,
                null,
                null,
                0);

            if (result != 0)
            {
                throw new Win32Exception(result, "Error connecting to remote share");
            }
        }


        public ConnectToSharedFolder(string networkName, NetworkCredential credentials)
        {
            _networkName = networkName;

            var netResource = new NetResource
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkName
            };

            var userName = string.IsNullOrEmpty(credentials.Domain)
                ? credentials.UserName
                : string.Format(@"{0}\{1}", credentials.Domain, credentials.UserName);

            var result = WNetAddConnection2(
                netResource,
                credentials.Password,
                userName,
                0);

            if (result != 0)
            {
                throw new Win32Exception(result, "Error connecting to remote share");
            }
        }

        public async void FileUpload(string LocalFile, string networkPath, NetworkCredential credentials)
        {
            string myNetworkPath;
            try
            {
                string UploadURL = Path.GetFileName(LocalFile);
                using (new ConnectToSharedFolder(networkPath, credentials))
                {
                    byte[] file = File.ReadAllBytes(LocalFile);
                    myNetworkPath = networkPath + "\\" + UploadURL;

                    using (FileStream fileStream = File.Create(myNetworkPath, file.Length))
                    {
                        await fileStream.WriteAsync(file, 0, file.Length);
                        fileStream.Close();
                    }
                }
            }
            catch (Exception)
            {
            }
        }



        ~ConnectToSharedFolder()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_networkName, 0, true);
        }
    }
}
