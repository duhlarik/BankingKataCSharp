using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingKataAPI.Services
{
    public interface IEmailService
    {
        Guid SendMessage(string emailAddress, string message);
    }
}
