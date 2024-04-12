using CrimeSummaryService.Controllers;
using CrimeViewerBackend.Services;
using Moq;
using PoliceUk.Entities.StreetLevel;

namespace CrimeService.Tests
{
    [TestClass]
    public class CrimeControllerTests
    {
        // Test Failing beause of IPoliceUkClient not containing LastUpdated, so put it in a wrapper Interface?
        [TestMethod]
        public void GetCrimeSummary_ReturnsStreetLevelCrimeResults()
        {
            // Arrange
            var mockCrimeService = new Mock<ICrimeService>();

            // Setup the behavior of StreetLevelCrimes method
            var expectedResults = new StreetLevelCrimeResults()
            {
                Crimes = new List<Crime>
                {
                    new Crime()
                    {
                        Category = "Category1"
                    }
                }
            };
            mockCrimeService.Setup(c => c.GetCrimes(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int?>()))
                            .Returns(expectedResults);
            var controller = new CrimeController(mockCrimeService.Object);

            double lat = 51.44237;
            double lng = -2.49810;
            int? month = 1;

            // Act
            var result = controller.GetCrimes(lat, lng, month);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResults, result);
        }
        // TODO: Test that month will get turned into DateTime

        [TestMethod]
        public void GetCrimeSummary_ExceptionsCaughtAndThrownNicely()
        {
            // Arrange
            var mockCrimeService = new Mock<ICrimeService>();
            mockCrimeService.Setup(c => c.GetCrimes(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int?>()))
                            .Throws(new Exception("Test exception"));

            var controller = new CrimeController(mockCrimeService.Object);

            double lat = 51.44237;
            double lng = -2.49810;
            int? month = 1;

            // Act & Assert
            var exception = Assert.ThrowsException<Exception>(() => controller.GetCrimes(lat, lng, month));
            Assert.AreEqual("Failed to retrieve crime data from Police UK API.", exception.Message);
        }
    }
}
