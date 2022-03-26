using MongoDB.Bson;

namespace MongoR.Interfaces
{
    public interface IEntityModel
    {
        public ObjectId Id { get; set; }
    }
}