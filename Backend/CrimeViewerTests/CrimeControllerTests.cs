using CrimeSummaryService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using PoliceCrimeViewer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrimeSummaryService.Tests
{
    [TestClass]
    public class CrimeSummaryControllerTests
    {
        [TestMethod]
        public async Task GetCrimeSummary_Returns_Successful_Response()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("[{\"category\":\"Burglary\"},{\"category\":\"Assault\"}]")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var controller = new CrimeSummaryController(httpClient);

            // Act
            var crimeSummary = await controller.GetCrimeSummary(51.44237, -2.49810, 1);

            // Assert
            Assert.IsNotNull(crimeSummary);
            Assert.AreEqual(2, crimeSummary.Count);
        }

        [TestMethod]
        public async Task GetCrimeSummary_Returns_Error_When_Api_Fails()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var controller = new CrimeSummaryController(httpClient);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<Exception>(() => controller.GetCrimeSummary(51.44237, -2.49810, 1));
            Assert.AreEqual(HttpStatusCode.InternalServerError.ToString(), exception.Message);
        }
    }
}
