using MongoDB.Driver;
using MongoLite.Helpers;
using System.Linq.Expressions;

namespace MongoLite.Bson
{
    /// <summary>
    ///     A set of extension methods for BSON entities.
    /// </summary>
    public static class BsonEntityExtensions
    {
        /// <summary>
        ///     Saves the entity to the database in an atomic update operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TField"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<bool> SaveAsync<T, TField>(this T model, Expression<Func<T, TField>> expression, TField value, CancellationToken cancellationToken = default)
            where T : BsonEntityBase, new()
            => BsonModelHelper<T>.SaveAsync(model, Builders<T>.Update.Set(expression, value), cancellationToken);

        /// <summary>
        ///     Saves the entity to the database in an atomic update operation. This method is fire-forget, and does not return a value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TField"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        public static void Save<T, TField>(this T model, Expression<Func<T, TField>> expression, TField value)
            where T : BsonEntityBase, new()
            => _ = BsonModelHelper<T>.SaveAsync(model, Builders<T>.Update.Set(expression, value));

        /// <summary>
        ///     Deletes the entity from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<bool> DeleteAsync<T>(this T model, CancellationToken cancellationToken = default)
            where T : BsonEntityBase, new()
            => BsonModelHelper<T>.DeleteAsync(model, cancellationToken);

        /// <summary>
        ///     Creates a new entity if it does not exist in the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="creationAction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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