using System.Collections.Generic;
using Catalog.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

// NOTES

// Initially going with just a inmemory repo so as to keep things simple

// Added async await as part of 3rd iteration, we could have removed this file as we have a mongo DB but keeping it as learning

namespace Catalog.Repositories 
{
    public class InMemoryItemsRepository : IItemsRepository
    {
        private readonly List<Item> items = new() {
            new Item {Id  = Guid.Parse("f493e169-e93e-4335-b547-12b845b5380f"), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow},
            new Item {Id  = Guid.NewGuid(), Name = "Iron Sword", Price = 29, CreatedDate = DateTimeOffset.UtcNow},
            new Item {Id  = Guid.NewGuid(), Name = "Bronze Shield", Price = 18, CreatedDate = DateTimeOffset.UtcNow}
        };

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await Task.FromResult(items);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            return await Task.FromResult(item);
        }

        public async Task CreateItemAsync(Item item)
        {
            items.Add(item);
            await Task.CompletedTask;
        }

        public async Task UpdateItemAsync(Item item)
        {
            var idx = items.FindIndex(existingItem => item.Id == existingItem.Id);
            items[idx] = item;
            await Task.CompletedTask;
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var idx = items.FindIndex(existingItem => id == existingItem.Id);
            items.RemoveAt(idx);
            await Task.CompletedTask;
        }
    }
}