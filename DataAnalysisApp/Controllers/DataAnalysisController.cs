using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysisApp.Controllers
{
    public class DataAnalysisController
    {
        public DataAnalysisController() 
        {

        }

        public double GetMean(List<TimeSpan> values)
        {
            return values.Average(v => v.Ticks);
        }

        public TimeSpan GetStandardDeviation(List<TimeSpan> values)
        {
            double sumOfSquares = values.Select(v => Math.Pow(v.Ticks - GetMean(values), 2)).Sum();
            double stdDevTicks = Math.Sqrt(sumOfSquares / (values.Count));
            return new TimeSpan(Convert.ToInt64(stdDevTicks));
        }

        public Tuple<TimeSpan, TimeSpan> GetConfidenceInterval(List<TimeSpan> values)
        {
            int count = values.Count;
            TimeSpan mean = new TimeSpan(Convert.ToInt64(GetMean(values)));
            TimeSpan stdDev = GetStandardDeviation(values);
            double criticalValue = 1.96;
            double marginOfErrorTicks = criticalValue * stdDev.Ticks / Math.Sqrt(count);
            TimeSpan marginOfError = new TimeSpan(Convert.ToInt64(marginOfErrorTicks));
            return Tuple.Create(mean - marginOfError, mean + marginOfError);
        }

        public double GetZScore(TimeSpan value, TimeSpan mean, TimeSpan stdDev)
        {
            return (value - mean) / stdDev;
        }
    }
}
