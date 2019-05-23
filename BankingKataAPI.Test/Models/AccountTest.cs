using System.Globalization;
using BankingKataAPI.Models;
using Xunit;

namespace BankingKataAPI.Test.Models
{
    public class AccountTest
    {
        public AccountTest()
        {
            var cultureInfo = new CultureInfo("en-US") {NumberFormat = {CurrencySymbol = "$"}};

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        [Fact]
        public void CanSpecifyAStartingBalance()
        {
            var account = new Account(new Money(65.98m));

            Assert.Equal(account.Balance, new Money(65.98m));
        }

        [Fact]
        public void ShouldGenerateIdForAccount()
        {
            var account1 = new Account(new Money());
            var account2 = new Account(new Money());

            Assert.NotEqual(account1.Id, account2.Id);
        }
    }
}
