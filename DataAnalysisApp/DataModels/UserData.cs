using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysisApp.DataModels
{
    public class UserData
    {
        public string UserID { get; set; }
        public DateTime Timestamp { get; set; }
        public string IPAddress { get; set; }
        public string Result { get; set; }
        public string Reason { get; set; }
        public string DeviceName { get; set; }
        public string OperatingSystem { get; set; }
        public string Location { get; set; }
    }
}
