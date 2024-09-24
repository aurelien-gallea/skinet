using Azure.Core.Serialization;
using Core.Entities;
using System.Net;
using System.Text.Json;
using Xunit;

namespace API.Tests
{
    public class ProductsApiTests
    {
        private readonly HttpClient _client;

        public ProductsApiTests()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:5000/");
        }

        [Fact]
        public async Task Get_Products_ReturnsSuccess()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/products");

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_Products_FromJson()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/products");

            var response = await _client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            var arrayOfProducts = JsonSerializer.Deserialize<Product[]>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(arrayOfProducts);
            Assert.IsType<Product[]>(arrayOfProducts);
        }
        [Fact]
        public async Task Get_Product_ReturnSucces()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/products/1");

            var response =await _client.SendAsync(request);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode );    
        }

        [Fact]
        public async Task Get_Product_FromJson()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/products/1");

            var response = await _client.SendAsync(request);
            
            var responseBody = await response.Content.ReadAsStringAsync();

            var product = JsonSerializer.Deserialize<Product>(responseBody, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            
            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
        }
    }
}
