using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoLite.Helpers;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MongoLite.Bson
{
    /// <summary>
    ///     Represents a base class for BSON entities, having a strongly-typed collection implementation.
    /// </summary>
    public abstract class AtomicEntity<T> : BsonEntity
        where T : AtomicEntity<T>, new()
    {
        /// <summary>
        ///     Saves a field in the entity atomically.
        /// </summary>
        protected void Save<TField>(TField value, [CallerMemberName] string? fieldName = null)
        {
            if (State is EntityState.Stateless or EntityState.Deleted or EntityState.Deserializing)
                return;

            BsonModelHelper<T>.Collection.ModifyDocumentAsync(ObjectId, Builders<T>.Update.Set(fieldName!, value)).Wait();
        }
    }

    /// <summary>
    ///     Represents a base class for BSON entities.
    /// </summary>
    public abstract class BsonEntity
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

        /// <summary>
        ///     Gets a stateless instance of the entity, without database bindings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetStateless<T>()
            where T : BsonEntity, new() => new()
            {
                State = EntityState.Stateless
            };

        /// <summary>
        ///     Creates a new instance of the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creationAction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T> CreateAsync<T>(Action<T>? creationAction = null, CancellationToken cancellationToken = default)
            where T : BsonEntity, new()
            => BsonModelHelper<T>.CreateAsync(creationAction, cancellationToken);

        /// <summary>
        ///     Gets the first entity in the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T?> GetFirstAsync<T>(CancellationToken cancellationToken = default)
            where T : BsonEntity, new()
            => BsonModelHelper<T>.GetAsync(x => true, cancellationToken);

        /// <summary>
        ///     Gets the first entity that matches the filter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T?> GetAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            where T : BsonEntity, new()
            => BsonModelHelper<T>.GetAsync(filter, cancellationToken);

        /// <summary>
        ///     Gets all entities that match the filter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static IAsyncEnumerable<T> GetManyAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            where T : BsonEntity, new()
            => BsonModelHelper<T>.GetManyAsync(filter, cancellationToken);

        /// <summary>
        ///     Gets all entities in the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static IAsyncEnumerable<T> GetAllAsync<T>(CancellationToken cancellationToken = default)
            where T : BsonEntity, new()
            => BsonModelHelper<T>.GetManyAsync(x => true, cancellationToken);

        /// <summary>
        ///     Deletes all entities that match the filter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<long> DeleteManyAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            where T : BsonEntity, new()
            => BsonModelHelper<T>.DeleteManyAsync(filter, cancellationToken);

        /// <summary>
        ///     Gets the count of entities that match the filter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<long> GetCountAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            where T : BsonEntity, new()
            => BsonModelHelper<T>.GetCountAsync(filter, cancellationToken);
    }
}
