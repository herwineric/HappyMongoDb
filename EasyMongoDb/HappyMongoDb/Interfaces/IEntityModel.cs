using MongoDB.Bson;

namespace HappyMongoDb.Interfaces
{
    public interface IEntityModel
    {
        public ObjectId Id { get; set; }
    }
}