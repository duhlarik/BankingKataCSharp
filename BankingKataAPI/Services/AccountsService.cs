using System;
using BankingKataAPI.Models;
using BankingKataAPI.Persistence;

namespace BankingKataAPI.Services
{
    public class AccountsService
    {
        private readonly IRepository<Account> _repository;

        public AccountsService(IRepository<Account> repository)
        {
            _repository = repository;
        }

        public Account FindAccount(Guid id)
        {
            return _repository.FindOne(id) ?? throw new Exception("Invalid account");
        }

        public Account OpenNewAccount(Money startingBalance)
        {
            var account = new Account(startingBalance);
            _repository.Save(account);
            return account;
        }

		public void Deposit(Guid accountId, Money amount)
		{
			var account = FindAccount(accountId);
            var updatedAccount = account.CloneWithBalance(account.Balance + amount);
		    _repository.Save(updatedAccount);
		}

		public void Withdraw(Guid accountId, Money amount)
		{
			var account = FindAccount(accountId);
			var updatedAccount = account.CloneWithBalance(account.Balance - amount);
		    _repository.Save(updatedAccount);
		}
    }
}
