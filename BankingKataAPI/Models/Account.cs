using System;
using Newtonsoft.Json;

namespace BankingKataAPI.Models
{
    public class Account : Identifiable
    {
        public Guid Id { get; }
        public Money Balance { get; }

        [JsonConstructor]
        private Account(Guid id, Money balance)
		{
			Id = id == Guid.Empty ? Guid.NewGuid() : id;
			Balance = balance;
		}

		public Account(Money balance)
            : this(Guid.Empty, balance)
		{
            
		}

        public Guid GetId()
        {
            return Id;
        }

		public Account CloneWithBalance(Money balance)
		{
			return new Account(Id, balance);
		}
    }
}
