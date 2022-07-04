using Microsoft.AspNetCore.Mvc;
using Catalog.Repositories;
using Catalog.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Catalog.Dtos;

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
        public IEnumerable<ItemDto> GetItems()
        {
            var items = repository.GetItems().Select( item => item.AsDto());
            return items;
        }

        // GET /items/{id}
        [HttpGet("{Id}")]
        public ActionResult<ItemDto> GetItem(Guid Id)
        {
            var item = repository.GetItem(Id);

            if(item == null) {
                return NotFound();
            }
            return item.AsDto();
        }
        
        // POST /items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto) 
        {
            Item item = new() {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateItem(item);
            
            return CreatedAtAction(nameof(GetItem), new { id = item.Id}, item.AsDto());
        } 

        // PUT /item
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto) {
            var existingItem = repository.GetItem(id);
            if(existingItem == null ) {
                return NotFound();
            }

            //Update items with the new values
            Item updateItem = existingItem with {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            
            repository.UpdateItem(updateItem);
            return NoContent();
        }
        
        // DELETE /item/Id
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id) {
            var existingItem = repository.GetItem(id);
            if(existingItem == null ) {
                return NotFound();
            }

            repository.DeleteItem(id);
            return NoContent();
        }
    }
}

// NOTES

// InMemoryItemsRepository -> is tightly injected to keep things simple