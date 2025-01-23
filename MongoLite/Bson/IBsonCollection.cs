using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace MongoLite.Bson
{
    /// <summary>
    ///     Represents a collection of documents in a MongoDB database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBsonCollection<T>
        where T : BsonEntity, new()
    {
        /// <summary>
        ///     Inserts a document into the collection.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task InsertDocumentAsync(T document, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Inserts multiple documents into the collection.
        /// </summary>
        /// <param name="documents"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task InsertDocumentsAsync(IEnumerable<T> documents, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Inserts a document into the collection, or updates it if it already exists.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> InsertOrUpdateDocumentAsync(T document, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates a document in the collection.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> UpdateDocumentAsync(T document, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates a document in the collection.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> UpdateDocumentAsync(T document, Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Modifies a document in the collection.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> ModifyDocumentAsync(ObjectId objectId, UpdateDefinition<T> update, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes a document from the collection.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> DeleteDocumentAsync(T document, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes a document from the collection.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> DeleteDocumentAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes multiple documents from the collection.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<long> DeleteManyDocumentsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Finds a document in the collection.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<T?> FindDocumentAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Finds a document in the collection.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<T?> FindDocumentAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Finds multiple documents in the collection.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IAsyncEnumerable<T> FindManyDocumentsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Finds multiple documents in the collection.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IAsyncEnumerable<T> FindManyDocumentsAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Counts the number of documents in the collection.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<long> CountDocumentsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    }
}
