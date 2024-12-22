using CsvHelper;
using CsvHelper.Configuration;
using DataAnalysisApp.DataModels;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DataAnalysisApp.Controllers
{
    public class MainController
    {
        private readonly DataLoadController dlc;
        private readonly FilePathController fpc;
        public MainController(string[] args)
        {
            fpc = new FilePathController(args);
            dlc = new DataLoadController(fpc.LOGIN_DATA_FILEPATH, fpc.HISTORICAL_DATA_FILEPATH);
        }

        public void Run() //REMOVE AND JUST USE THE CONTROLLER ?
        {
            CalculateFalsePositivesIncorrectPasswords(dlc.GetLoginData());
            //CalculateIncorrectPasswords(dlc.GetLoginData());
        }

        private void CalculateFalsePositivesIncorrectPasswords(List<LoginData> loginData)
        {
            List<LoginResult> results = new List<LoginResult>();
            var attempts = loginData.Where(r => r.Result == "Fail" && r.Reason == "Incorrect Password").ToList();

            // Output false positives to console
            foreach (var attempt in attempts)
            {
                var userHistory = dlc.GetUserHistoryData(attempt.UserID); //Get history for false-positive analysis
                var result = new LoginResult
                {
                    LoginAttempt = attempt,
                    Score = FalsePositiveCalculator(attempt, userHistory)
                };
                results.Add(result);
                //Console.WriteLine($"{attempt.Timestamp}: {attempt.UserID} - {attempt.IPAddress} ({attempt.DeviceName}, {attempt.OperatingSystem}, {attempt.Location})");
            }
            foreach (var result in results)
            {
                Console.WriteLine(result.Score);
            }
        }

        private double FalsePositiveCalculator(LoginData loginData, List<UserData> userHistory)
        {
            double result = 0.0;
            var osMatches = userHistory.Where(user => user.OperatingSystem == loginData.OperatingSystem).ToList();
            foreach (var attempt in osMatches)
            {
                Console.WriteLine($"{attempt.Timestamp}: {attempt.UserID} - {attempt.IPAddress} ({attempt.DeviceName}, {attempt.OperatingSystem}, {attempt.Location})");
            }
            var osPercent = Math.Round((double)osMatches.Count / userHistory.Count, 2);
            Console.WriteLine(osPercent + " : " + osMatches.Count + "/" + userHistory.Count);
            if (osPercent > 0.5) 
            {
                return result = 0.5;
            }
            return result;
        }




        private void CalculateIncorrectPasswords(List<LoginData> loginData) //OLD
        {
            var falsePositives = loginData.Where(r => r.Result == "Fail" && r.Reason == "Incorrect Password").ToList();

            // Output false positives to console
            foreach (var attempt in falsePositives)
            {
                Console.WriteLine($"{attempt.Timestamp}: {attempt.UserID} - {attempt.IPAddress} ({attempt.DeviceName}, {attempt.OperatingSystem}, {attempt.Location})");
            }
        }
    }
}