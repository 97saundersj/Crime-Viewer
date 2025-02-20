using Moq;
using PoliceUk;
using PoliceUk.Entities.StreetLevel;

namespace CrimeService.Tests
{
    [TestClass]
    public class CrimeServiceTests
    {
        [TestMethod]
        public void GetCrimeSummary_ReturnsStreetLevelCrimeResults()
        {
            // Arrange
            var mockPoliceClient = new Mock<IPoliceUkClient>();

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
            mockPoliceClient.Setup(c => c.StreetLevelCrimes(It.IsAny<Geoposition>(), It.IsAny<DateTime?>()))
                            .Returns(expectedResults);
            var crimeService = new CrimeViewerBackend.Services.CrimeService(mockPoliceClient.Object)
            {
                CrimesLastUpdated = new DateTime()
            };

            double lat = 51.44237;
            double lng = -2.49810;
            int? month = 1;

            // Act
            var result = crimeService.GetCrimes(lat, lng, month);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResults, result);
        }

        [TestMethod]
        public void ConvertMonthToDateTime_WithCrimesLastUpdatedInPast_ReturnsPreviousYear()
        {
            // Arrange
            var mockPoliceClient = new Mock<IPoliceUkClient>();
            var service = new CrimeViewerBackend.Services.CrimeService(mockPoliceClient.Object);
            service.CrimesLastUpdated = new DateTime(DateTime.Now.Year - 1, 7, 15); // Example: July Last Year
            int monthNumber = 6; // June

            // Act
            DateTime result = service.ConvertMonthToDateTime(monthNumber);

            // Assert
            Assert.AreEqual(DateTime.Now.Year - 1, result.Year);
            Assert.AreEqual(monthNumber, result.Month);
            Assert.AreEqual(1, result.Day);
        }

        // Tests are commented out because LastUpdated does not exsist in IPoliceUkClient, only PoliceUkClient
        // TODO: Figure out another way to mock e.g. put it in a wrapper Interface?
        /*
        [TestMethod]
        public void GetLastUpdated()
        {
            
        }
        
        [TestMethod]
        public void ConvertMonthToDateTime_WithNoCrimesLastUpdated_ReturnsCurrentYear()
        {
            // Arrange
            var mockPoliceClient = new Mock<IPoliceUkClient>();
            var service = new CrimeViewerBackend.Services.CrimeService(mockPoliceClient.Object);
            int monthNumber = 6; // June

            // Act
            DateTime result = service.ConvertMonthToDateTime(monthNumber);

            // Assert
            Assert.AreEqual(DateTime.Now.Year, result.Year);
            Assert.AreEqual(monthNumber, result.Month);
            Assert.AreEqual(1, result.Day);
        }

        [TestMethod]
        public void ConvertMonthToDateTime_WithCrimesLastUpdatedInCurrentYear_ReturnsCurrentYear()
        {
            // Arrange
            var mockPoliceClient = new Mock<IPoliceUkClient>();
            var service = new CrimeViewerBackend.Services.CrimeService(mockPoliceClient.Object);
            service.CrimesLastUpdated = new DateTime(DateTime.Now.Year, 7, 15); // Example: July Current Year
            int monthNumber = 6; // June

            // Act
            DateTime result = service.ConvertMonthToDateTime(monthNumber);

            // Assert
            Assert.AreEqual(DateTime.Now.Year, result.Year);
            Assert.AreEqual(monthNumber, result.Month);
            Assert.AreEqual(1, result.Day);
        }
        */
    }
}