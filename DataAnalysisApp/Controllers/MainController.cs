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
        private readonly DataAnalysisController dac;
        public MainController(string[] args)
        {
            fpc = new FilePathController(args);
            dlc = new DataLoadController(fpc.LOGIN_DATA_FILEPATH, fpc.HISTORICAL_DATA_FILEPATH);
            dac = new DataAnalysisController();
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
                Console.WriteLine($"***** Analyzing: {attempt.Timestamp}, {attempt.UserID}, {attempt.IPAddress}, {attempt.DeviceName}, {attempt.OperatingSystem}, {attempt.Location} *****");
                var userHistory = dlc.GetUserHistoryData(attempt.UserID); //Get history for false-positive analysis
                double timestampScore = Calc_TimeStamp(attempt, userHistory);
                double osScore = Calc_OS(attempt, userHistory);
                var result = new LoginResult
                {
                    LoginAttempt = attempt,
                    Score = ScoreCalculator(osScore, timestampScore, 0.0, 0.0, 0.0)
                };
                results.Add(result);
                Console.WriteLine($"*** Score *** \n {result.Score}");
                Console.WriteLine("---------------------------------------------------------------------------------------------------");
                //Console.WriteLine($"{attempt.Timestamp}: {attempt.UserID} - {attempt.IPAddress} ({attempt.DeviceName}, {attempt.OperatingSystem}, {attempt.Location})");
            }

            Console.WriteLine("***** Scores ******");
            var sortedResultsByScore = results.OrderByDescending(r => r.Score).ToList();
            foreach (var result in sortedResultsByScore)
            {
                Console.WriteLine($"{String.Format("{0:0.00}", result.Score)} : {result.LoginAttempt.Timestamp}, {result.LoginAttempt.UserID}, {result.LoginAttempt.IPAddress}, {result.LoginAttempt.DeviceName}, {result.LoginAttempt.OperatingSystem}, {result.LoginAttempt.Location}");
            }
        }

        private double ScoreCalculator(double os, double timeStamp, double ipAddress, double deviceName, double location)
        {
            double score = os + timeStamp + ipAddress + deviceName + location;
            return score;
        }

        private double Calc_OS(LoginData loginAttempt,  List<UserData> userHistory)
        {
            Console.WriteLine("** Analyzing OS **");

            double result = 0.0;
            var osMatches = userHistory.Where(user => user.OperatingSystem == loginAttempt.OperatingSystem).ToList();
            foreach (var attempt in osMatches)
            {
                //Console.WriteLine($"{attempt.Timestamp}: {attempt.UserID} - {attempt.IPAddress} ({attempt.DeviceName}, {attempt.OperatingSystem}, {attempt.Location})");
            }
            var osPercent = Math.Round((double)osMatches.Count / userHistory.Count, 2);
            Console.WriteLine(osPercent + " : " + osMatches.Count + "/" + userHistory.Count);
            if (osPercent > 0.5)
            {
                return result = 0.5;
            }
            return result;
        }

        private double Calc_TimeStamp(LoginData loginAttempt, List<UserData> userHistory)
        {
            Console.WriteLine("** Analyzing TimeStamp **");

            //Convert timestamp to TimeOfDay for analysis, don't care about which day login attempt occurred only the time of day.
            List<TimeSpan> loginTimesOfDay = userHistory.Select(login => login.Timestamp.TimeOfDay).ToList();

            // Calculate mean
            TimeSpan mean = new TimeSpan(Convert.ToInt64(loginTimesOfDay.Average(ts => ts.Ticks)));
            Console.WriteLine($"Mean: {mean}");

            // Calculate median
            TimeSpan median = dac.CalculateMedian(loginTimesOfDay);
            Console.WriteLine($"Median: {median}");

            // Calculate standard deviation
            TimeSpan stdDev = dac.CalculateStandardDeviation(loginTimesOfDay);
            Console.WriteLine($"Standard Deviation: {stdDev}");

            // Calculate 95% confidence interval
            var confidenceInterval = dac.CalculateConfidenceInterval(loginTimesOfDay, mean, stdDev, 0.95);
            Console.WriteLine($"95% Confidence Interval: ({confidenceInterval.Item1}, {confidenceInterval.Item2})");

            var zScore = dac.CalculateZScore(loginAttempt.Timestamp.TimeOfDay, mean, stdDev);
            Console.WriteLine($"Login Time: {loginAttempt.Timestamp.TimeOfDay}, Z-Score: {zScore:F2}");
            return Math.Abs(zScore); //Positive number for the further calculation of scores, higher score = bad, and it doesn't matter whether you're are way off LOWER or UPPER bound.
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