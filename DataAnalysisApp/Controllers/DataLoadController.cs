using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalysisApp.DataModels;

namespace DataAnalysisApp.Controllers
{
    public class DataLoadController
    {
        private string loginFP;
        private string historyFP;

        public DataLoadController(string _loginFP, string _historyFP) //Give data path here maybe
        {
            loginFP = _loginFP;
            historyFP = _historyFP;
        }

        public List<LoginData> GetLoginData()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                BadDataFound = null,
                MissingFieldFound = null,
                HeaderValidated = null
            };
   
            using (var reader = new StreamReader(loginFP))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<LoginData>().ToList();
            }
        }

        public List<LoginData> GetUserHistoryData(string user)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                BadDataFound = null,
                MissingFieldFound = null,
                HeaderValidated = null
            };

            using (var reader = new StreamReader(historyFP))
            using (var csv = new CsvReader(reader, config))
            {
                List<LoginData> userHistory = new List<LoginData>();
                foreach (var row in csv.GetRecords<LoginData>().ToList())
                {
                    if (row.UserID == user)
                        userHistory.Add(row);
                }
                return userHistory;
            }
        }
    }
}