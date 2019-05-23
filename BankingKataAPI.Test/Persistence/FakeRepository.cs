using System;
using System.Collections.Generic;
using System.Text;
using BankingKataAPI.Models;
using BankingKataAPI.Persistence;

namespace BankingKataAPI.Test.Persistence
{
    public class FakeRepository<T> : IRepository<T> where T : class, Identifiable
    {
        private readonly Dictionary<Guid, T> _items;

        public FakeRepository()
        {
            _items = new Dictionary<Guid, T>();
        }

        public T FindOne(Guid id)
        {
            return _items.ContainsKey(id) ? _items[id] : null;
        }

        public void Save(T item)
        {
            _items[item.GetId()] = item;
        }
    }
}
