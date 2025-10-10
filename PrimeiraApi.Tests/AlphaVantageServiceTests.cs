using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using PrimeiraApi.Models;
using PrimeiraApi.Services;
using System.Net;
using System.Text;
using System.Text.Json;

namespace PrimeiraApi.Tests
{
    [TestClass]
    public class AlphaVantageServiceTests
    {
        [TestMethod]
        public async Task GetLongestGrowthStreakAsync_WithValidData_ShouldReturnCorrectStreak()
        {

            var fakeJsonResponse = @"
            {
                ""Meta Data"": { ""2. Symbol"": ""FAKE"" },
                ""Time Series (Daily)"": {
                    ""2025-10-06"": { ""4. close"": ""110.00"" },
                    ""2025-10-05"": { ""4. close"": ""100.00"" },
                    ""2025-10-04"": { ""4. close"": ""95.00"" },
                    ""2025-10-03"": { ""4. close"": ""90.00"" },
                    ""2025-10-02"": { ""4. close"": ""102.00"" },
                    ""2025-10-01"": { ""4. close"": ""101.00"" },
                    ""2025-09-30"": { ""4. close"": ""100.00"" }
                }
            }";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fakeJsonResponse, Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["AlphaVantage:ApiKey"]).Returns("FAKE_KEY");

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();

            var alphaVantageService = new AlphaVantageService(httpClient, mockConfiguration.Object, mockHttpClientFactory.Object);

            var result = await alphaVantageService.GetLongestGrowthStreakAsync("FAKE");

            int expectedStreak = 3;
            Assert.AreEqual(expectedStreak, result.Streak, "O cálculo da sequência de altas não retornou o valor esperado.");
        }
    }
}