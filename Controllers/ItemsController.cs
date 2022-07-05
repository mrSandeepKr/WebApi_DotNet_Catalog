using Microsoft.AspNetCore.Mvc;
using Catalog.Repositories;
using Catalog.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Catalog.Dtos;
using System.Threading.Tasks;

namespace Catalog.Controllers
{
    // Get /items

    [ApiController]
    [Route("items")]
    public class ItemsController: ControllerBase 
    {
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }

        // If GET /items is done then we come here
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await repository.GetItemsAsync())
                        .Select( item => item.AsDto());
            return items;
        }

        // GET /items/{id}
        [HttpGet("{Id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid Id)
        {
            var item = await repository.GetItemAsync(Id);

            if(item == null) {
                return NotFound();
            }
            return item.AsDto();
        }
        
        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto) 
        {
            Item item = new() {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateItemAsync(item);
            
            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id}, item.AsDto());
        } 

        // PUT /item
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto) {
            var existingItem = await repository.GetItemAsync(id);
            if(existingItem == null ) {
                return NotFound();
            }

            //Update items with the new values
            Item updateItem = existingItem with {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            
            await repository.UpdateItemAsync(updateItem);
            return NoContent();
        }
        
        // DELETE /item/Id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id) {
            var existingItem = await repository.GetItemAsync(id);
            if(existingItem == null ) {
                return NotFound();
            }

            await repository.DeleteItemAsync(id);
            return NoContent();
        }
    }
}

// NOTES

// InMemoryItemsRepository -> is tightly injected to keep things simple