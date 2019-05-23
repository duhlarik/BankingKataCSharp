using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingKataAPI.Models;

namespace BankingKataAPI.Persistence
{
    public interface IRepository<T>
        where T : class, Identifiable
    {
        T FindOne(Guid id);

        void Save(T item);
    }
}
