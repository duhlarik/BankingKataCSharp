using System;
using System.Globalization;
using BankingKataAPI.Models;
using BankingKataAPI.Services;
using BankingKataAPI.Test.Persistence;
using Xunit;

namespace BankingKataAPI.Test.Services
{
    public class AccountsServiceTest
    {
        private readonly FakeRepository<Account> _repository;
        private readonly AccountsService _service;

        public AccountsServiceTest()
        {
            _repository = new FakeRepository<Account>();
            _service = new AccountsService(_repository);

            var cultureInfo = new CultureInfo("en-US") {NumberFormat = {CurrencySymbol = "$"}};

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        [Fact]
        public void OpeningAnAccountSavesItInTheAccountRepository()
        {
            var account = _service.OpenNewAccount(new Money());
            Assert.NotNull(_repository.FindOne(account.GetId()));
        }

        [Fact]
        public void NewAccountsStartWithCorrectBalance()
        {
            var expectedBalance = new Money(7.11m);

            var account = _service.OpenNewAccount(expectedBalance);
            var savedAccount = _repository.FindOne(account.GetId());

            Assert.Equal(expectedBalance, savedAccount.Balance);
        }

        [Fact]
        public void FindAccountFindsItFromTheRepository()
        {
            var expectedBalance = new Money(12.34m);
            var account = new Account(expectedBalance);
            _repository.Save(account);

            var savedAccount = _service.FindAccount(account.GetId());
            Assert.Equal(expectedBalance, savedAccount.Balance);
        }

        [Fact]
        public void FindAccountThrowsAnErrorIfAccountIsNotFound()
        {
            var nonExistantAccountId = Guid.NewGuid();

            var exception = Assert.Throws<Exception>(() => _service.FindAccount(nonExistantAccountId));
            Assert.Equal("Invalid account", exception.Message);
        }

        [Fact]
        public void DepositAddsFundsToSavedAccount()
        {
            var account = new Account(new Money(52.01m));
            _repository.Save(account);

            _service.Deposit(account.GetId(), new Money(12.34m));
            var savedAccount = _repository.FindOne(account.GetId());
            Assert.Equal(new Money(64.35m), savedAccount.Balance);
        }

        [Fact]
        public void DepositThrowsErrorIfAccountDoesNotExist()
        {
            var nonExistantAccountId = Guid.NewGuid();

            var exception = Assert.Throws<Exception>(() => _service.Deposit(nonExistantAccountId, new Money(12.34m)));
            Assert.Equal("Invalid account", exception.Message);
        }


        [Fact]
        public void WithdrawSubtractsFundsFromSavedAccount()
        {
            var account = new Account(new Money(82.00m));
            _repository.Save(account);

            _service.Withdraw(account.GetId(), new Money(56.30m));
            var savedAccount = _repository.FindOne(account.GetId());

            Assert.Equal(new Money(25.70m), savedAccount.Balance);
        }

        [Fact]
        public void WithdrawThrowsErrorIfAccountDoesNotExist()
        {
            var nonExistantAccountId = Guid.NewGuid();

            var exception = Assert.Throws<Exception>(() => _service.Withdraw(nonExistantAccountId, new Money(12.34m)));
            Assert.Equal("Invalid account", exception.Message);
        }
    }
}