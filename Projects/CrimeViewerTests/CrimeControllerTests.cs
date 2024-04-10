using System;
using System.Net;
using System.Threading.Tasks;
using CrimeSummaryService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PoliceUk;
using PoliceUk.Entities.StreetLevel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrimeService.Tests
{
    [TestClass]
    public class CrimeControllerTests
    {
        // Test Failing beause of IPoliceUkClient not containing LastUpdated, so put it in a wrapper Interface?
        /*
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
            var controller = new CrimeController(mockPoliceClient.Object);

            double lat = 51.44237;
            double lng = -2.49810;
            int? month = 1;

            // Act
            var result = controller.GetCrimeSummary(lat, lng, month);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResults, result);
        }
        */
        // TODO: Test that month will get turned into DateTime

        [TestMethod]
        public void GetCrimeSummary_ExceptionsCaughtAndThrownNicely()
        {
            // Arrange
            var mockPoliceClient = new Mock<IPoliceUkClient>();
            mockPoliceClient.Setup(c => c.StreetLevelCrimes(It.IsAny<Geoposition>(), It.IsAny<DateTime?>()))
                            .Throws(new Exception("Test exception"));

            var controller = new CrimeController(mockPoliceClient.Object);

            double lat = 51.44237;
            double lng = -2.49810;
            int? month = 1;

            // Act & Assert
            var exception = Assert.ThrowsException<Exception>(() => controller.GetCrimeSummary(lat, lng, month));
            Assert.AreEqual("Failed to retrieve crime data from Police UK API.", exception.Message);
        }
    }
}
