using MongoDB.Driver;
using MongoLite.Helpers;
using System.Linq.Expressions;

namespace MongoLite.Bson
{
    public static class BsonEntityExtensions
    {
        public static Task<bool> SaveAsync<T, TField>(this T model, Expression<Func<T, TField>> expression, TField value, CancellationToken cancellationToken = default)
            where T : BsonEntityBase, new()
            => BsonModelHelper<T>.SaveAsync(model, Builders<T>.Update.Set(expression, value), cancellationToken);

        public static Task<bool> DeleteAsync<T>(this T model, CancellationToken cancellationToken = default)
            where T : BsonEntityBase, new()
            => BsonModelHelper<T>.DeleteAsync(model, cancellationToken);

        public static async Task<T> CreateIfNotExists<T>(this Task<T?> task, Action<T> creationAction, CancellationToken cancellationToken = default)
            where T : BsonEntityBase, new()
        {
            var result = await task;

            if (result is null)
                return await BsonModelHelper<T>.CreateAsync(creationAction, cancellationToken);

            return result;
        }
    }
}