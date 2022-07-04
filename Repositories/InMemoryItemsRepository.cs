using System.Collections.Generic;
using Catalog.Entities;
using System;
using System.Linq;

// NOTES

// Initially going with just a inmemory repo so as to keep things simple

namespace Catalog.Repositories 
{
    public class InMemoryItemsRepository : IItemsRepository
    {
        private readonly List<Item> items = new() {
            new Item {Id  = Guid.Parse("f493e169-e93e-4335-b547-12b845b5380f"), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow},
            new Item {Id  = Guid.NewGuid(), Name = "Iron Sword", Price = 29, CreatedDate = DateTimeOffset.UtcNow},
            new Item {Id  = Guid.NewGuid(), Name = "Bronze Shield", Price = 18, CreatedDate = DateTimeOffset.UtcNow}
        };

        public IEnumerable<Item> GetItems()
        {
            return items;
        }

        public Item GetItem(Guid id)
        {
            return items.Where(item => item.Id == id).SingleOrDefault();
        }

        public void CreateItem(Item item)
        {
            items.Add(item);
        }

        public void UpdateItem(Item item)
        {
            var idx = items.FindIndex(existingItem => item.Id == existingItem.Id);
            items[idx] = item;
        }

        public void DeleteItem(Guid id)
        {
            var idx = items.FindIndex(existingItem => id == existingItem.Id);
            items.RemoveAt(idx);
        }
    }
}