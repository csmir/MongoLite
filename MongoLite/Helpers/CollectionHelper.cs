using MongoLite.Bson;
using MongoLite.Collections;

namespace MongoLite.Helpers
{
    /// <summary>
    ///     Represents a helper class for asynchronous cursor collections retrieved from the database.
    /// </summary>
    public static class CollectionHelper
    {
        /// <summary>
        ///     Creates a new instance of <see cref="List{T}"/> from an <see cref="IAsyncEnumerable{T}"/>. This method iterates over the async enumerable. 
        ///     It is not recommended to use this method if the enumerable has already been iterated over.
        /// </summary>
        /// <typeparam name="T">The object of <see cref="BsonEntity"/> that this model implements.</typeparam>
        /// <param name="asyncEnumerable">The asynchronous enumerable that this operation shifts.</param>
        /// <returns>A new instance of <see cref="List{T}"/> populated with the values passed in by the <paramref name="asyncEnumerable"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the enumerable has already been enumerated.</exception>
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> asyncEnumerable)
            where T : BsonEntity, new()
        {
            var list = new List<T>();

            try
            {
                await foreach (var item in asyncEnumerable)
                    list.Add(item);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("This enumerable has already been enumerated, and cannot be enumerated again.", ex);
            }

            return list;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="ModelDictionary{TKey, TValue}"/> from an <see cref="IAsyncEnumerable{T}"/>. This method iterates over the async enumerable.
        ///     It is not recommended to use this method if the enumerable has already been iterated over.
        /// </summary>
        /// <typeparam name="TKey">The key selected to be used to index the <see cref="ModelDictionary{TKey, TValue}"/> by.</typeparam>
        /// <typeparam name="TValue">The object of <see cref="BsonEntity"/> that this model implements.</typeparam>
        /// <param name="asyncEnumerable">The asynchronous enumerable that this operation shifts.</param>
        /// <param name="keySelector"></param>
        /// <returns>A new instance of <see cref="ModelDictionary{TKey, TValue}"/> populated with the values passed in by the <paramref name="asyncEnumerable"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the enumerable has already been enumerated.</exception>
        public static async Task<ModelDictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(this IAsyncEnumerable<TValue> asyncEnumerable, Func<TValue, TKey> keySelector)
            where TKey : notnull
            where TValue : BsonEntity, new()
        {
            var dictionary = new ModelDictionary<TKey, TValue>();

            try
            {
                await foreach (var item in asyncEnumerable)
                    dictionary[keySelector(item)] = item;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("This enumerable has already been enumerated, and cannot be enumerated again.", ex);
            }

            return dictionary;
        }
    }
}
