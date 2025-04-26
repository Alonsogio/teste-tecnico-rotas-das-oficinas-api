using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Application.Features.Auth.Commands.LoginCommand;

namespace RO.DevTest.Tests.Application.Features.Clients
{
    public class ClientIntegrationTests : IClassFixture<WebApplicationFactory<RO.DevTest.WebApi.Program>>
    {
        private readonly HttpClient _client;

        public ClientIntegrationTests(WebApplicationFactory<RO.DevTest.WebApi.Program> factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task GetClients_ReturnsOkResponse()
        {
            await AuthenticateAsync();
            var response = await _client.GetAsync("/api/clients");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("client", responseString);
        }

        [Fact]
        public async Task CreateClient_ReturnsCreatedResponse()
        {
            var newClient = new Client
            {
                Nome = "Novo Cliente",
                Email = "novo@cliente.com",
                Telefone = "(11) 98765-4321"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(newClient),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("/api/clients", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
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
            Console.WriteLine(responseString);

            var authResponse = JsonSerializer.Deserialize<LoginResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (authResponse?.AccessToken == null)
            {
                throw new Exception("Token not found in the response.");
            }

            Console.WriteLine(authResponse);
            Console.WriteLine("Access Token: " + authResponse.AccessToken);
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResponse.AccessToken);
        }


        [Fact]
        public async Task UpdateClient_ReturnsOkOrNoContentResponse()
        {
            await AuthenticateAsync();
            var newClient = new Client
            {
                Nome = "Cliente Para Atualizar",
                Email = "atualizar@cliente.com",
                Telefone = "(11) 99999-9999"
            };

            var createContent = new StringContent(
                JsonSerializer.Serialize(newClient),
                Encoding.UTF8,
                "application/json"
            );

            var createResponse = await _client.PostAsync("/api/clients", createContent);
            createResponse.EnsureSuccessStatusCode();

            var createdClientJson = await createResponse.Content.ReadAsStringAsync();
            var createdClient = JsonSerializer.Deserialize<Client>(createdClientJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var updatedClient = new
            {
                id = createdClient.Id,
                Nome = "Cliente Atualizado",
                Email = "atualizado@cliente.com",
                Telefone = "(99) 98765-4321"
            };

            var updateContent = new StringContent(
                JsonSerializer.Serialize(updatedClient),
                Encoding.UTF8,
                "application/json"
            );

            var updateResponse = await _client.PutAsync($"/api/clients/{createdClient.Id}", updateContent);
            updateResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync($"/api/clients/{createdClient.Id}");
            getResponse.EnsureSuccessStatusCode();
            var getContent = await getResponse.Content.ReadAsStringAsync();
            Assert.Contains("Cliente Atualizado", getContent);
        }


        [Fact]
        public async Task DeleteClient_ReturnsNoContentResponse()
        {
            await AuthenticateAsync();
            var newClient = new Client
            {
                Nome = "Cliente Para Deletar",
                Email = "deletar@cliente.com",
                Telefone = "(11) 98888-8888"
            };

            var createContent = new StringContent(
                JsonSerializer.Serialize(newClient),
                Encoding.UTF8,
                "application/json"
            );

            var createResponse = await _client.PostAsync("/api/clients", createContent);
            createResponse.EnsureSuccessStatusCode();

            var createdClientJson = await createResponse.Content.ReadAsStringAsync();
            var createdClient = JsonSerializer.Deserialize<Client>(createdClientJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var deleteResponse = await _client.DeleteAsync($"/api/clients/{createdClient.Id}");
            deleteResponse.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}