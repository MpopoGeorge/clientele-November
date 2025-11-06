using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Text;
using System.Text.Json;
using WeatherAPI.Application.Interfaces;
using WeatherAPI.Application.Services;
using WeatherAPI.Domain.Entities;

namespace WeatherAPI.Tests
{
    [TestFixture]
    public class WeatherServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private WeatherService _weatherService;
        private const string TestApiKey = "test_api_key";

        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _weatherService = new WeatherService(_httpClient, TestApiKey);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        public async Task GetWeatherByCityAsync_ValidCity_ReturnsWeatherInfo()
        {
            // Arrange
            var responseJson = JsonSerializer.Serialize(new
            {
                name = "London",
                sys = new { country = "GB" },
                main = new { temp = 15.5, humidity = 65 },
                weather = new[] { new { description = "clear sky" } },
                wind = new { speed = 3.2 }
            });

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _weatherService.GetWeatherByCityAsync("London");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.City, Is.EqualTo("London"));
            Assert.That(result.Country, Is.EqualTo("GB"));
            Assert.That(result.Temperature, Is.EqualTo(15.5));
            Assert.That(result.Description, Is.EqualTo("clear sky"));
        }

        [Test]
        public void GetWeatherByCityAsync_EmptyCityName_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _weatherService.GetWeatherByCityAsync(""));
        }

        [Test]
        public void GetWeatherByCityAsync_WhitespaceCityName_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _weatherService.GetWeatherByCityAsync("   "));
        }

        [Test]
        public void GetWeatherByCityAsync_CityNotFound_ThrowsException()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _weatherService.GetWeatherByCityAsync("InvalidCity"));
        }

        [Test]
        public void GetWeatherByCityAsync_ServerError_ThrowsException()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _weatherService.GetWeatherByCityAsync("London"));
        }
    }
}

