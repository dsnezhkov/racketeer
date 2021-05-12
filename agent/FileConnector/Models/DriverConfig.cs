using System;
using System.Collections.Generic;

namespace FileConnector.Models
{

    #region ConfigFileJson
    public enum OpMode : ushort
    {
        None = 0,
        Interactive = 1,
        Distributed = 2,
    }
    public enum Operation : ushort
    {
        None = 0,
        Encrypt = 1,
        Decrypt = 2,
    }
    public enum AuthType : ushort
    {
        None = 0,
        Local = 1,
        NetworkImplicit = 2,
        NetworkExplicit = 3,
    }
    public enum HMethod : ushort
    {
        None = 0,
        Get = 1,
        Post = 2,
    }

    public class Profile
    {
        public ushort Interval { get; set; }
        public bool EncryptPay { get; set; }
    }

    public class ExtEndpoint
    {
        public string Ident { get; set; }
        public string Base { get; set; }
        public string Proto { get; set; }
        public bool TLS { get; set; }
        public Profile Profile { get; set; }
        public ushort Timeout { get; set; } 
    }
    public class Request
    {
        public HMethod Method  { get; set; }
    }
    public class Response
    {
        public HMethod Method  { get; set; }
    }

    public class Contract
    {
        public string Resource { get; set; }
        public Request Request { get; set; }
        public Request Response { get; set; }
    }
    public class ExtComm
    {
        public string As { get; set; }
        public Dictionary<string, Contract> Contracts { get; set; }
    }


    public class Keys
    {
        public String MasterKey { get; set; }
        public String SiteID { get; set; }
    }
    public class AuthN
    {
        public string Ident { get; set; }
        public AuthType Type { get; set; }
        public string Domain { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

    }
    public class File
    {
        public string FilePath { get; set; }
        public Operation Operation { get; set; }
        public string Key { get; set; }
    }
    public class Host
    {
        public string Ident { get; set; }
        public string Name { get; set; }
        public string As { get; set; }
        public Operation Operation { get; set; }
        public string Key { get; set; }
        public List<File> Files { get; set; }
    }
    public class DriverConfig
    {
        public OpMode OpMode { get; set; }
        public List<ExtEndpoint> ExtEndpoints { get; set; }
        public ExtComm ExtComm { get; set; }
        public Keys Keys { get; set; }
        public List<AuthN> AuthN { get; set; }
        public List<Host> Hosts { get; set; }
    }
    #endregion

}
