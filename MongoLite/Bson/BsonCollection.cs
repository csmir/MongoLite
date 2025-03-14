using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MongoLite.Bson
{
    /// <summary>
    ///     Represents a collection of documents in a MongoDB database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    public class BsonCollection<T>(string name) : IBsonCollection<T>
        where T : BsonEntity, new()
    {
        private readonly IMongoCollection<T> _collection = MongoHost.GetMongoCollection<T>(name);

        /// <inheritdoc />
        public virtual async Task InsertDocumentAsync(T document, CancellationToken cancellationToken = default)
            => await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);

        /// <inheritdoc />
        public virtual async Task InsertDocumentsAsync(IEnumerable<T> documents, CancellationToken cancellationToken = default)
            => await _collection.InsertManyAsync(documents, cancellationToken: cancellationToken);

        /// <inheritdoc />
        public virtual async Task<bool> InsertOrUpdateDocumentAsync(T document, CancellationToken cancellationToken = default)
        {
            if (document.ObjectId == ObjectId.Empty)
            {
                await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);
                return true;
            }

            var result = await _collection.ReplaceOneAsync(x => x.ObjectId == document.ObjectId, document, cancellationToken: cancellationToken);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
                return true;

            return false;
        }

        /// <inheritdoc />
        public virtual async Task<bool> UpdateDocumentAsync(T document, CancellationToken cancellationToken = default)
        {
            var result = await _collection.ReplaceOneAsync(x => x.ObjectId == document.ObjectId, document, cancellationToken: cancellationToken);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
                return true;

            return false;
        }

        /// <inheritdoc />
        public virtual async Task<bool> UpdateDocumentAsync(T document, Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            var result = await _collection.ReplaceOneAsync(expression, document, cancellationToken: cancellationToken);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
                return true;

            return false;
        }

        /// <inheritdoc />
        public virtual async Task<bool> ModifyDocumentAsync(T document, UpdateDefinition<T> update, CancellationToken cancellationToken = default)
            => (await _collection.UpdateOneAsync(x => x.ObjectId == document.ObjectId, update, cancellationToken: cancellationToken)).IsAcknowledged;

        /// <inheritdoc />
        public virtual async Task<bool> ModifyDocumentAsync(ObjectId objectId, UpdateDefinition<T> update, CancellationToken cancellationToken = default)
            => (await _collection.UpdateOneAsync(x => x.ObjectId == objectId, update, cancellationToken: cancellationToken)).IsAcknowledged;

        /// <inheritdoc />
        public virtual async Task<bool> DeleteDocumentAsync(T document, CancellationToken cancellationToken = default)
            => (await _collection.DeleteOneAsync(x => x.ObjectId == document.ObjectId, cancellationToken: cancellationToken)).IsAcknowledged;

        /// <inheritdoc />
        public virtual async Task<bool> DeleteDocumentAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            => (await _collection.DeleteOneAsync(filter, cancellationToken: cancellationToken)).IsAcknowledged;

        /// <inheritdoc />
        public virtual async Task<long> DeleteManyDocumentsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        {
            var result = await _collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);

            if (result.IsAcknowledged)
                return result.DeletedCount;

            return -1;
        }

        /// <inheritdoc />
        public virtual async Task<T?> FindDocumentAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            => await (await _collection.FindAsync(filter, cancellationToken: cancellationToken)).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        /// <inheritdoc />
        public virtual async Task<T?> FindDocumentAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
            => await (await _collection.FindAsync(filter, cancellationToken: cancellationToken)).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        /// <inheritdoc />
        public virtual async IAsyncEnumerable<T> FindManyDocumentsAsync(Expression<Func<T, bool>> filter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var collection = await _collection.FindAsync(filter, cancellationToken: cancellationToken);

            foreach (var entity in collection.ToEnumerable(cancellationToken: cancellationToken))
            {
                yield return entity;
            }
        }

        /// <inheritdoc />
        public virtual async IAsyncEnumerable<T> FindManyDocumentsAsync(FilterDefinition<T> filter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var collection = await _collection.FindAsync(filter, cancellationToken: cancellationToken);

            foreach (var entity in collection.ToEnumerable(cancellationToken: cancellationToken))
            {
                yield return entity;
            }
        }

        /// <inheritdoc />
        public virtual Task<long> CountDocumentsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
            return _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        }
    }
}
