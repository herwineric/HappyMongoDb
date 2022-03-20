using HappyMongoDb.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HappyMongoDb.Models
{
    public record BaseEntityModel : IEntityModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}