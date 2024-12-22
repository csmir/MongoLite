using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoLite.Bson
{
    /// <summary>
    ///     Represents a base class for BSON entities.
    /// </summary>
    public abstract class BsonEntityBase : IBsonEntity
    {
        /// <summary>
        ///     Gets or sets the object ID of the entity.
        /// </summary>
        [BsonId]
        public ObjectId ObjectId { get; set; }

        /// <summary>
        ///     Gets or sets the date and time when the entity was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        ///     Gets or sets the state of the entity. This property is ignored by the BSON serializer.
        /// </summary>
        [BsonIgnore]
        public EntityState State { get; set; }
    }
}
