using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Application.Features.Auth.Commands.LoginCommand;
using RO.DevTest.Application.Features.Products.Commands.CreateProductCommand;

namespace RO.DevTest.Tests.Application.Features.Products
{
    public class ProductIntegrationTests : IClassFixture<WebApplicationFactory<RO.DevTest.WebApi.Program>>
    {
        private readonly HttpClient _client;

        public ProductIntegrationTests(WebApplicationFactory<RO.DevTest.WebApi.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResponse()
        {
            await AuthenticateAsync();

            var newProduct = new CreateProductCommand
            {
                Nome = "Produto Teste",
                Descricao = "Descrição do Produto Teste",
                Preco = 100.00m,
                Estoque = 10
            };

            var content = new StringContent(
                JsonSerializer.Serialize(newProduct),
                Encoding.UTF8,
                "application/json"
            );
            var createResponse = await _client.PostAsync("/api/products", content);
            createResponse.EnsureSuccessStatusCode();


            var response = await _client.GetAsync("/api/products");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Produto Teste", responseString);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedResponse()
        {
            await AuthenticateAsync();
            var newProduct = new Product
            {
                Nome = "Novo Produto",
                Descricao = "Descrição do Produto",
                Preco = 99.99m,
                Estoque = 10
            };

            var content = new StringContent(
                JsonSerializer.Serialize(newProduct),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("/api/products", content);
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var createdProduct = JsonSerializer.Deserialize<Product>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(createdProduct);
            Assert.Equal(newProduct.Nome, createdProduct?.Nome);
        }

        private async Task AuthenticateAsync()
        {
            var loginData = new LoginCommand
            {
                Email = "admin@email.com",
                Password = "123456Adm@"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(loginData),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("/api/auth/login", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<LoginResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (authResponse?.AccessToken == null)
                throw new Exception("Token not found in the response.");

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResponse.AccessToken);
        }
    }
}
