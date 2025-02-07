using DataAnalysisApp.Controllers;

namespace DataAnalysisAppTest
{
    [TestClass]
    public class StatisticsTest
    {
        [TestMethod]
        public void GetMean_ValidInput_ReturnsCorrectMean()
        {
            // Arrange
            DataAnalysisController stats = new DataAnalysisController();
            List<TimeSpan> timeStamps = new List<TimeSpan>
            {
                TimeSpan.FromHours(1),
                TimeSpan.FromHours(2),
                TimeSpan.FromHours(3),
                TimeSpan.FromHours(4),
                TimeSpan.FromHours(5)
            };

            // Act
            double result = stats.GetMean(timeStamps);

            // Assert
            Assert.AreEqual(TimeSpan.FromHours(3).Ticks, result);
        }

        public void GetMean_SingleElementArray_ReturnsElementValue()
        {
            // Arrange
            DataAnalysisController stats = new DataAnalysisController();
            List<TimeSpan> timeStamps = new List<TimeSpan> { TimeSpan.FromHours(5) };

            // Act
            double result = stats.GetMean(timeStamps);

            // Assert
            Assert.AreEqual(TimeSpan.FromHours(5).Ticks, result);
        }

        [TestMethod]
        public void GetMean_EmptyArray_ThrowsInvalidOperationException()
        {
            // Arrange
            DataAnalysisController stats = new DataAnalysisController();
            List<TimeSpan> timeStamps = new List<TimeSpan> { };

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => stats.GetMean(timeStamps));
        }

        [TestMethod]
        public void GetMean_NullArray_ThrowsArgumentNullException()
        {
            // Arrange
            DataAnalysisController stats = new DataAnalysisController();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => stats.GetMean(null));
        }

        [TestMethod]
        public void GetStandardDeviation_ValidInput_ReturnsCorrectStandardDeviation()
        {
            // Arrange
            DataAnalysisController stats = new DataAnalysisController();
            List<TimeSpan> timeStamps = new List<TimeSpan>
            {
                TimeSpan.FromHours(1),
                TimeSpan.FromHours(2),
                TimeSpan.FromHours(3),
                TimeSpan.FromHours(4),
                TimeSpan.FromHours(5)
            };
            
            //Act
            TimeSpan result = stats.GetStandardDeviation(timeStamps);

            // Assert
            Assert.IsTrue(Math.Abs((result - TimeSpan.FromHours(1.4142135623730951)).TotalSeconds) < 1);
        }

        [TestMethod]
        public void GetConfidenceInterval_ValidInput_ReturnsCorrectInterval()
        {
            // Arrange
            DataAnalysisController stats = new DataAnalysisController();
            List<TimeSpan> timeSpans = new List<TimeSpan>
            {
                TimeSpan.FromHours(1),
                TimeSpan.FromHours(2),
                TimeSpan.FromHours(3),
                TimeSpan.FromHours(4),
                TimeSpan.FromHours(5)
            };

            // Act
            Tuple<TimeSpan, TimeSpan> result = stats.GetConfidenceInterval(timeSpans);

            // Expected values
            TimeSpan expectedLowerBound = TimeSpan.FromHours(1.760463931575295);
            TimeSpan expectedUpperBound = TimeSpan.FromHours(4.239536068424705);

            // Assert
            Assert.IsTrue(Math.Abs((result.Item1 - expectedLowerBound).TotalSeconds) < 1);
            Assert.IsTrue(Math.Abs((result.Item2 - expectedUpperBound).TotalSeconds) < 1);
        }

        [TestMethod]
        public void GetConfidenceInterval_SingleElementList_ReturnsZeroMargin()
        {
            // Arrange
            DataAnalysisController stats = new DataAnalysisController();
            List<TimeSpan> timeSpans = new List<TimeSpan> { TimeSpan.FromHours(5) };

            // Act
            Tuple<TimeSpan, TimeSpan> result = stats.GetConfidenceInterval(timeSpans);

            // Assert
            Assert.AreEqual(TimeSpan.FromHours(5), result.Item1);
            Assert.AreEqual(TimeSpan.FromHours(5), result.Item2);
        }

        [TestMethod]
        public void GetZScore_ValidInput_ReturnsCorrectZScore()
        {
            // Arrange
            DataAnalysisController stats = new DataAnalysisController();
            List<TimeSpan> timeStamps = new List<TimeSpan>
            {
                TimeSpan.FromHours(1),
                TimeSpan.FromHours(2),
                TimeSpan.FromHours(3),
                TimeSpan.FromHours(4),
                TimeSpan.FromHours(5)
            };

            TimeSpan mean = new TimeSpan(Convert.ToInt64(stats.GetMean(timeStamps)));

            // Act
            double result = stats.GetZScore(TimeSpan.FromHours(3), mean, stats.GetStandardDeviation(timeStamps));

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}