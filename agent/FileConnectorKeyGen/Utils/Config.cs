using FileConnectorCommon;
using System;
using System.Collections.Generic;

namespace FileConnectorKeyGen.Utils
{
    static class Config
    {
        public static class SiteId
        {
            public static string outFile = string.Empty;
            public static string siteId = string.Empty;
        }
        public static class MasterKey
        {
            public static string outFile = string.Empty;
            public static string siteId = string.Empty;
            public static string masterKeyHash = string.Empty;
        }
        public static class FileKey
        {
            public static string outFile = string.Empty;
            public static List<string> fileKey = new List<string> { };
        }
        public static class CredKey
        {
            public static string outFile = string.Empty;
            public static string siteId = string.Empty;
            public static string masterKeyHash = string.Empty;
            public static string credKeyEnc = string.Empty;
            public static string credKeyDec = string.Empty;
        }
        public static class File
        {
            public static string inFile = string.Empty;
            public static string outFile = string.Empty;
            public static string fileKey = string.Empty;
        }
    }
}
