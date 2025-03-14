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
        ///     Creates a new entity in the database.
        /// </summary>
        /// <remarks>Returns <see langword="false"/> if the state of the model is <see cref="EntityState.Deleted"/>, <see cref="EntityState.Stateless"/>, or if the model could not be replaced nor inserted; Otherwise, <see langword="true"/>.</remarks>
        public static Task UpdateAsync<T>(this T model, CancellationToken cancellationToken = default)
            where T : BsonEntity, new()
            => BsonModelHelper<T>.UpdateAsync(model, cancellationToken);

        /// <summary>
        ///     Creates a new entity in the database synchronously.
        /// </summary>
        /// <remarks>Returns <see langword="false"/> if the state of the model is <see cref="EntityState.Deleted"/>, <see cref="EntityState.Stateless"/>, or if the model could not be replaced nor inserted; Otherwise, <see langword="true"/>.</remarks>
        public static void Update<T>(this T model)
            where T : BsonEntity, new()
            => BsonModelHelper<T>.UpdateAsync(model).GetAwaiter().GetResult();

        /// <summary>
        ///     Deletes the entity from the database.
        /// </summary>
        /// <remarks>Returns <see langword="false"/> if the state of the model is <see cref="EntityState.Deleted"/>, <see cref="EntityState.Stateless"/>, or if the model could not be deleted; Otherwise, <see langword="true"/>.</remarks>
        public static Task<bool> DeleteAsync<T>(this T model, CancellationToken cancellationToken = default)
            where T : BsonEntity, new()
            => BsonModelHelper<T>.DeleteAsync(model, cancellationToken);

        /// <summary>
        ///     Deletes the entity from the database synchronously.
        /// </summary>
        /// <remarks>Returns <see langword="false"/> if the state of the model is <see cref="EntityState.Deleted"/>, <see cref="EntityState.Stateless"/>, or if the model could not be deleted; Otherwise, <see langword="true"/>.</remarks>
        public static bool Delete<T>(this T model)
            where T : BsonEntity, new()
            => BsonModelHelper<T>.DeleteAsync(model).GetAwaiter().GetResult();

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
        ///     Creates a new entity if it does not exist in the database synchronously.
        /// </summary>
        public static T CreateIfNotExists<T>(this T? entity, Action<T> creationAction, CancellationToken cancellationToken = default)
            where T : BsonEntity, new()
        {
            if (entity is null)
                return BsonModelHelper<T>.CreateAsync(creationAction, cancellationToken).GetAwaiter().GetResult();

            return entity;
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

        /// <summary>
        ///    Throws an exception if the entity does not exist synchronously.
        /// </summary>
        /// <exception cref="MongoException"></exception>
        public static T ThrowIfNotExists<T>(this T? entity)
            where T : BsonEntity, new()
        {
            if (entity is null)
                throw new MongoException($"The requested entity of type {typeof(T).Name} does not exist.");
            return entity;
        }
    }
}