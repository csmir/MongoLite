using MongoDB.Driver;
using System.Linq.Expressions;

namespace MongoLite.Bson
{
    public interface IBsonCollection<T>
        where T : IBsonEntity, new()
    {
        public Task InsertDocumentAsync(T document, CancellationToken cancellationToken = default);

        public Task InsertDocumentsAsync(IEnumerable<T> documents, CancellationToken cancellationToken = default);

        public Task<bool> InsertOrUpdateDocumentAsync(T document, CancellationToken cancellationToken = default);

        public Task<bool> UpdateDocumentAsync(T document, CancellationToken cancellationToken = default);

        public Task<bool> UpdateDocumentAsync(T document, Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

        public Task<bool> ModifyDocumentAsync(T document, UpdateDefinition<T> update, CancellationToken cancellationToken = default);

        public Task<bool> DeleteDocumentAsync(T document, CancellationToken cancellationToken = default);

        public Task<bool> DeleteDocumentAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

        public Task<long> DeleteManyDocumentsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

        public Task<T?> FindDocumentAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

        public Task<T?> FindDocumentAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

        public IAsyncEnumerable<T> FindManyDocumentsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

        public IAsyncEnumerable<T> FindManyDocumentsAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

        public Task<long> CountDocumentsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    }
}
