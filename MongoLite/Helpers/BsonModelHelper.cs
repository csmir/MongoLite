using MongoDB.Driver;
using MongoLite.Bson;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MongoLite.Helpers
{
    internal static class BsonModelHelper<T>
        where T : BsonEntity, new()
    {
        public static readonly BsonCollection<T> Collection = new(typeof(T).Name[..^7]);

        public static async Task SaveAsync(T model, CancellationToken cancellationToken = default)
        {
            if (model.State is EntityState.Stateless)
                return;

            await Collection.InsertOrUpdateDocumentAsync(model, cancellationToken);
        }

        public static async Task<T> CreateAsync(Action<T>? creationAction = null, CancellationToken cancellationToken = default)
        {
            var value = new T();

            creationAction?.Invoke(value);

            await Collection.InsertOrUpdateDocumentAsync(value, cancellationToken);

            value.State = EntityState.Ready;

            return value;
        }

        public static async Task<T?> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        {
            var value = await Collection.FindDocumentAsync(filter, cancellationToken);

            if (value is not null)
            {
                value.State = EntityState.Ready;
            }

            return value;
        }

        public static async IAsyncEnumerable<T> GetManyAsync(Expression<Func<T, bool>> filter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var value = Collection.FindManyDocumentsAsync(filter, cancellationToken);

            await foreach (var v in value)
            {
                v.State = EntityState.Ready;
                yield return v;
            }
        }

        public static async Task<bool> DeleteAsync(T model, CancellationToken cancellationToken = default)
        {
            if (model.State is EntityState.Stateless or EntityState.Deleted)
                return false;

            model.State = EntityState.Deleted;

            return await Collection.DeleteDocumentAsync(model, cancellationToken);
        }

        public static Task<long> DeleteManyAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        {
            return Collection.DeleteManyDocumentsAsync(filter, cancellationToken);
        }

        public static Task<long> GetCountAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        {
            return Collection.CountDocumentsAsync(filter, cancellationToken);
        }
    }
}
