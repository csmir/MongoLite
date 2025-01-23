using MongoDB.Driver;
using MongoLite.Helpers;

namespace MongoLite.Bson
{
    /// <summary>
    ///     A set of extension methods for BSON entities.
    /// </summary>
    public static class BsonEntityExtensions
    {
        /// <summary>
        ///     Deletes the entity from the database.
        /// </summary>
        public static Task<bool> DeleteAsync<T>(this T model, CancellationToken cancellationToken = default)
            where T : BsonEntity, new()
            => BsonModelHelper<T>.DeleteAsync(model, cancellationToken);

        /// <summary>
        ///     Creates a new entity if it does not exist in the database.
        /// </summary>
        public static async Task<T> CreateIfNotExists<T>(this Task<T?> task, Action<T> creationAction, CancellationToken cancellationToken = default)
            where T : BsonEntity, new()
        {
            var result = await task;

            if (result is null)
                return await BsonModelHelper<T>.CreateAsync(creationAction, cancellationToken);

            return result;
        }

        /// <summary>
        ///     Throws an exception if the entity does not exist.
        /// </summary>
        /// <exception cref="MongoException"></exception>
        public static async Task<T> ThrowIfNotExists<T>(this Task<T?> task)
            where T : BsonEntity, new()
        {
            var result = await task;

            return result is null ? throw new MongoException($"The requested entity of type {typeof(T).Name} does not exist.") : result;
        }
    }
}