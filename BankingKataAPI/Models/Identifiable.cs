using System;

namespace BankingKataAPI.Models
{
    public interface Identifiable
    {
        Guid GetId();
    }
}
