using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoR.Interfaces;

namespace MongoR.Models
{
    public record BaseEntityModel 
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}