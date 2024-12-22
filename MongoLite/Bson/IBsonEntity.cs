using MongoDB.Bson.Serialization.Attributes;
using MongoLite.Helpers;
using System.Linq.Expressions;

namespace MongoLite.Bson
{
    public interface IBsonEntity
    {
        public DateTimeOffset CreatedAt { get; set; }

        [BsonIgnore]
        public abstract EntityState State { get; set; }

        public static T GetStateless<T>()
            where T : IBsonEntity, new() => new()
            {
                State = EntityState.Stateless
            };

        public static Task<T> CreateAsync<T>(Action<T>? creationAction = null, CancellationToken cancellationToken = default)
            where T : BsonEntityBase, new()
            => BsonModelHelper<T>.CreateAsync(creationAction, cancellationToken);

        public static Task<T?> GetFirstAsync<T>(CancellationToken cancellationToken = default)
            where T : BsonEntityBase, new()
            => BsonModelHelper<T>.GetAsync(x => true, cancellationToken);

        public static Task<T?> GetAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            where T : BsonEntityBase, new()
            => BsonModelHelper<T>.GetAsync(filter, cancellationToken);

        public static IAsyncEnumerable<T> GetManyAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            where T : BsonEntityBase, new()
            => BsonModelHelper<T>.GetManyAsync(filter, cancellationToken);

        public static IAsyncEnumerable<T> GetAllAsync<T>(CancellationToken cancellationToken = default)
            where T : BsonEntityBase, new()
            => BsonModelHelper<T>.GetManyAsync(x => true, cancellationToken);

        public static Task<long> DeleteManyAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            where T : BsonEntityBase, new()
            => BsonModelHelper<T>.DeleteManyAsync(filter, cancellationToken);

        public static Task<long> GetCountAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            where T : BsonEntityBase, new()
            => BsonModelHelper<T>.GetCountAsync(filter, cancellationToken);
    }
}
