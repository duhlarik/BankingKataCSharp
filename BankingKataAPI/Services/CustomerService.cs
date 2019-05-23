using System;
using BankingKataAPI.Models;
using BankingKataAPI.Persistence;

namespace BankingKataAPI.Services
{
    public class CustomerService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IEmailService _emailService;

        public CustomerService(IRepository<User> userRepository, IEmailService emailService) {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public void UpdateEmailAddress(Guid userId, string emailAddress)
        {
            var user = _userRepository.FindOne(userId) ?? throw new Exception("Invalid userId");
            var updatedUser = user.cloneWithEmailAddress(emailAddress);
            _userRepository.Save(updatedUser);
        }

        public Guid SendFraudAlert(Guid userId) {
            // TODO implement this method!
            return Guid.Empty;
        }
    }
}
