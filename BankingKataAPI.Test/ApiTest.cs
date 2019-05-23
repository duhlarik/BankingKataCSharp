using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BankingKataAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace BankingKataAPI.Test
{
    public class ApiTest : IDisposable
    {
        private readonly TestServer _server;

        public ApiTest()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }

        [Fact]
        public async Task ShouldReturnAccount()
        {
            var money = new[] {new Money(12m), new Money(13m)};
            var accounts = await CreateAccounts(money);
           
            var account = await GetAccount(accounts[0].Id);
            Assert.Equal(accounts[0].Id, account.Id);
        }

        public void Dispose()
        {
            _server?.Dispose();
        }

        private async Task<Account[]> CreateAccounts(IEnumerable money)
        {
            var client = _server.CreateClient();
            var moneyJson = JsonConvert.SerializeObject(money);
            var content = new StringContent(moneyJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/accounts", content);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Account[]>(json);
        }

        private async Task<Account> GetAccount(Guid accountId)
        {
            var client = _server.CreateClient();
            var response = await client.GetAsync($"api/accounts/{accountId}");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Account>(json);
        } 
    }
}
