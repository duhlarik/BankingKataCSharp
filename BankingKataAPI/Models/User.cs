using System;

namespace BankingKataAPI.Models
{
    public class User : Identifiable
    {
        public Guid Id { get; private set; }
        public string EmailAddress { get; private set; }

        private User(Guid id, string emailAddress)
		{
			this.Id = id;
			this.EmailAddress = emailAddress;
		}

		public User(String emailAddress)
		{
            this.Id = Guid.NewGuid();
            this.EmailAddress = emailAddress;
		}

        public Guid GetId()
        {
            return Id;
        }

		public User cloneWithEmailAddress(string emailAddress)
		{
			return new User(this.Id, emailAddress);
		}
    }
}
