using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankingKataAPI.Models;

namespace BankingKataAPI.Persistence
{
    public class NetworkRepository<T> : IRepository<T>
        where T : class, Identifiable
    {
        private readonly Dictionary<Guid, T> _items;
        private readonly Random _random;

        public NetworkRepository()
        {
            _random = new Random();
            _items = new Dictionary<Guid, T>();
        }

        public T FindOne(Guid id)
        {
            RandomlyExplode();
            return _items.ContainsKey(id)
                ? _items[id]
                : null;
        }

        public void Save(T item)
        {
            RandomlyExplode();
            _items[item.GetId()] = item;
        }

        private void RandomlyExplode()
        {
            
            try {
                var sleepTime = _random.Next(2000) + 1000;
                Thread.Sleep(sleepTime);
            } catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            var willExplode = _random.NextDouble() < 0.04;
            if (willExplode) {
                throw new Exception("The network repository has exploded.");
            }
        }
    }
}
