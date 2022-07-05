using System;
using System.Collections.Generic;
using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

// mongoDBClient allows us to interact with MongoDB
//
// DockerImge -> standalone 

namespace Catalog.Repositories 
{
    public class MongoDbItemsRepository : IItemsRepository
    {

        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;

        private readonly FilterDefinitionBuilder<Item> filterbuilder = Builders<Item>.Filter;

        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<Item>(collectionName);
        }
        
        public void CreateItem(Item item)
        {
            itemsCollection.InsertOne(item);
        }

        public void DeleteItem(Guid id)
        {
            var filter = filterbuilder.Eq(item => item.Id, id);
            itemsCollection.DeleteOne(filter);
        }

        public Item GetItem(Guid id)
        {
            var filter = filterbuilder.Eq(item => item.Id, id);
            return itemsCollection.Find(filter).SingleOrDefault();
        }

        public IEnumerable<Item> GetItems()
        {
            return itemsCollection.Aggregate().ToList();
        }

        public void UpdateItem(Item item)
        {
             var filter = filterbuilder.Eq(existingItem => existingItem.Id, item.Id);
             itemsCollection.ReplaceOne(filter, item);
        }
    }
}