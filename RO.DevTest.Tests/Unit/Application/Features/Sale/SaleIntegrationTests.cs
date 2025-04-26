using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using RO.DevTest.Application.Features.Auth.Commands.LoginCommand;

namespace RO.DevTest.Tests.Application.Features.Sales
{
    public class SalesIntegrationTests : IClassFixture<WebApplicationFactory<RO.DevTest.WebApi.Program>>
    {
        private readonly HttpClient _client;

        public SalesIntegrationTests(WebApplicationFactory<RO.DevTest.WebApi.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateSale_ShouldReturnCreated()
        {
            await AuthenticateAsync();

            var clienteResponse = await _client.PostAsJsonAsync("/api/clients", new
            {
                nome = "Cliente Teste",
                email = $"cliente{Guid.NewGuid()}@teste.com",
                telefone = "(11) 98765-4321"
            });
            clienteResponse.EnsureSuccessStatusCode();
            var clienteCreated = await clienteResponse.Content.ReadFromJsonAsync<ClientCreatedResponse>();

            var produtoResponse = await _client.PostAsJsonAsync("/api/products", new
            {
                nome = "Produto Teste",
                descricao = "Descrição do produto teste",
                preco = 100.0m,
                estoque = 10
            });
            produtoResponse.EnsureSuccessStatusCode();
            var produtoCreated = await produtoResponse.Content.ReadFromJsonAsync<ProductCreatedResponse>();

            var createRequest = new
            {
                clienteId = clienteCreated!.Id,
                itens = new[]
                {
                    new
                    {
                        produtoId = produtoCreated!.Id,
                        quantidade = 2
                    }
                }
            };

            var saleResponse = await _client.PostAsJsonAsync("/api/sales", createRequest);

            saleResponse.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, saleResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteSale_ShouldReturnNoContent()
        {
            await AuthenticateAsync();

            var clienteResponse = await _client.PostAsJsonAsync("/api/clients", new
            {
                nome = "Cliente Teste",
                email = $"cliente{Guid.NewGuid()}@teste.com",
                telefone = "(11) 98765-4321"
            });
            clienteResponse.EnsureSuccessStatusCode();
            var clienteCreated = await clienteResponse.Content.ReadFromJsonAsync<ClientCreatedResponse>();

            var produtoResponse = await _client.PostAsJsonAsync("/api/products", new
            {
                nome = "Produto Teste",
                descricao = "Descrição do produto teste",
                preco = 100.0m,
                estoque = 10
            });
            produtoResponse.EnsureSuccessStatusCode();
            var produtoCreated = await produtoResponse.Content.ReadFromJsonAsync<ProductCreatedResponse>();

            var createSaleRequest = new
            {
                clienteId = clienteCreated!.Id,
                itens = new[]
                {
                    new
                    {
                        produtoId = produtoCreated!.Id,
                        quantidade = 1
                    }
                }
            };

            var createSaleResponse = await _client.PostAsJsonAsync("/api/sales", createSaleRequest);
            createSaleResponse.EnsureSuccessStatusCode();

            var createdSale = await createSaleResponse.Content.ReadFromJsonAsync<SaleCreatedResponse>();

            var deleteResponse = await _client.DeleteAsync($"/api/sales/{createdSale!.Id}");

            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task GetSalesReport_ShouldReturnOk()
        {
            await AuthenticateAsync();

            var inicio = DateTime.UtcNow.AddDays(-1).ToString("o");
            var fim = DateTime.UtcNow.AddDays(1).ToString("o");

            var response = await _client.GetAsync($"/api/sales/relatorio?inicio={inicio}&fim={fim}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
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

        private class SaleCreatedResponse
        {
            public Guid Id { get; set; }
        }

        private class ClientCreatedResponse
        {
            public Guid Id { get; set; }
        }

        private class ProductCreatedResponse
        {
            public Guid Id { get; set; }
        }
    }
}
