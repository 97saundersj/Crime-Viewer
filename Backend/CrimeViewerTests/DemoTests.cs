using Microsoft.Extensions.Logging;
using Moq;
using PoliceCrimeViewer.Controllers;
using PoliceCrimeViewer;

namespace CrimeViewerTests
{
    [TestClass]
    public class WeatherForecastControllerTests
    {
        [TestMethod]
        public void Get_ReturnsWeatherForecasts()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<WeatherForecast>));

            var forecasts = result.ToList();
            Assert.AreEqual(5, forecasts.Count);

            foreach (var forecast in forecasts)
            {
                Assert.IsNotNull(forecast.Date);
                Assert.IsNotNull(forecast.Summary);
                Assert.IsTrue(forecast.TemperatureC >= -20 && forecast.TemperatureC <= 55);
            }
        }
    }
}
