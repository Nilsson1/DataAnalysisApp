using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysisApp.DataModels
{
    public class LoginResult
    {
        public LoginData LoginAttempt { get; set; }
        public double Score { get; set; }
    }
}
