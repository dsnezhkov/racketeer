using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConnector.Models
{
    #region TaskSummary
    public class TaskSummary
    {
        public string SiteId { get; set; }
        public List<HostTaskSummary> HostTaskSummaries { get; set; }
    }
    public class HostTaskSummary
    {
        public string HostIdent { get; set; }
        public List<FileTaskSummary> FileTaskSummaries { get; set; }
    }
    public class FileTaskSummary
    {
        public string PreImageName { get; set; }
        public string PostImageName { get; set; }
        public string PreImageHash { get; set; }
        public string PostImageHash { get; set; }
        public long PreImageSize { get; set; }
        public long PostImageSize { get; set; }
        public string ImageTime { get; set; }
        public string SymKey { get; set; }
        public Operation Operation { get; set; }
    }
    #endregion
}
