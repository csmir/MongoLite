using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoLite.Bson
{
    public abstract class BsonEntityBase : IBsonEntity
    {
        [BsonId]
        public ObjectId ObjectId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        [BsonIgnore]
        public EntityState State { get; set; }
    }
}
