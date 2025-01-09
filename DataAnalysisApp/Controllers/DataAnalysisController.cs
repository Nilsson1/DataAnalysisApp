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

        public TimeSpan CalculateMedian(List<TimeSpan> values)
        {
            var sortedValues = values.OrderBy(n => n).ToList();
            int count = sortedValues.Count;
            if (count % 2 == 0)
            {
                return new TimeSpan((sortedValues[count / 2 - 1].Ticks + sortedValues[count / 2].Ticks) / 2);
            }
            else
            {
                return sortedValues[count / 2];
            }
        }

        public TimeSpan CalculateStandardDeviation(List<TimeSpan> values)
        {
            double meanTicks = values.Average(v => v.Ticks);
            double sumOfSquares = values.Select(v => Math.Pow(v.Ticks - meanTicks, 2)).Sum();
            double stdDevTicks = Math.Sqrt(sumOfSquares / (values.Count - 1));
            return new TimeSpan(Convert.ToInt64(stdDevTicks));
        }

        public Tuple<TimeSpan, TimeSpan> CalculateConfidenceInterval(List<TimeSpan> values, TimeSpan mean, TimeSpan stdDev, double confidenceLevel)
        {
            int count = values.Count;
            double criticalValue = 1.96; // Z-value for 95% confidence
            double marginOfErrorTicks = criticalValue * (stdDev.Ticks / Math.Sqrt(count));
            TimeSpan marginOfError = new TimeSpan(Convert.ToInt64(marginOfErrorTicks));
            return Tuple.Create(mean - marginOfError, mean + marginOfError);
        }

        public double CalculateZScore(TimeSpan value, TimeSpan mean, TimeSpan stdDev)
        {
            return (value - mean) / stdDev;
        }
    }
}
