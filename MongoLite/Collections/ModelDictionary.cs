using MongoLite.Bson;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace MongoLite.Collections
{
    /// <summary>
    ///     A dictionary for <see cref="BsonEntity"/> elements. This dictionary type is designed for non-nullability, and it will return a default value if the key is not found.
    /// </summary>
    /// <remarks> 
    ///     The default value is a stateless instance of <typeparamref name="TValue"/>, retrieved from <see cref="BsonEntity.GetStateless{T}"/>. This object is populated with values set at <see langword="new"/> and is not saved to the database.
    /// </remarks>
    /// <typeparam name="TKey">The key from which database entries are indexed.</typeparam>
    /// <typeparam name="TValue">The <see cref="BsonEntity"/> element for which <typeparamref name="TKey"/> stands as an indexer.</typeparam>
    public sealed class ModelDictionary<TKey, TValue>(Action<TKey, TValue>? valueTransform = null) : IDictionary<TKey, TValue>
        where TKey : notnull
        where TValue : BsonEntity, new()
    {
        private readonly Dictionary<TKey, TValue?> _impl = [];

        /// <summary>
        ///     Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key to use to get or set a new element in the key value collection.</param>
        /// <returns>The <typeparamref name="TValue"/> this key has a reference to. If no element was found by the key, <see cref="Default"/> will be returned instead.</returns>
        [NotNull]
        public TValue this[TKey key]
        {
            get
            {
                TryGetValue(key, out var value);

                return value;
            }
            set
            {
                _impl[key] = value;
            }
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys 
            => _impl.Keys;

        /// <inheritdoc />
        public ICollection<TValue> Values 
            => _impl.Values!;

        /// <inheritdoc />
        public int Count 
            => _impl.Count;

        /// <inheritdoc />
        public bool IsReadOnly { get; } = false;

        /// <summary>
        ///     = <see cref="BsonEntity.GetStateless{T}"/>.
        /// </summary>
        /// <remarks>
        ///     Gets the default value for the dictionary. This value is a stateless instance of <typeparamref name="TValue"/>, retrieved from <see cref="BsonEntity.GetStateless{T}"/>.
        ///     This object is populated with values set at <see langword="new"/> and is not saved to the database.
        /// </remarks>
        public TValue Default { get; } = BsonEntity.GetStateless<TValue>();

        /// <summary>
        ///     Gets or sets an action that transforms the <typeparamref name="TValue"/> entry returned by the dictionary, in case of failure.
        /// </summary>
        public Action<TKey, TValue>? ValueTransform { get; set; } = valueTransform;

        /// <summary>
        ///     Creates a new instance of <see cref="ModelDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="implementationObject"></param>
        /// <param name="valueTransform"></param>
        public ModelDictionary(Dictionary<TKey, TValue?> implementationObject, Action<TKey, TValue>? valueTransform = null)
            : this(valueTransform)
        {
            _impl = implementationObject;
        }

        /// <inheritdoc />
        public void Add(TKey key, TValue value)
        {
            _impl.Add(key, value);
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This method will not return the default value for <typeparamref name="TValue"/> if the key is not found.
        /// </remarks>
        public bool ContainsKey(TKey key) 
            => _impl.ContainsKey(key);

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() 
            => _impl.GetEnumerator();

        /// <inheritdoc />
        public bool Remove(TKey key) 
            => _impl.Remove(key);

        /// <inheritdoc />
        /// <remarks>
        ///     <paramref name="value"/> will be set to <see cref="Default"/> if the key is not found or if the value is null.
        /// </remarks>
        public bool TryGetValue(TKey key, [NotNull] out TValue value)
        {
            if (_impl.TryGetValue(key, out var innerValue))
            {
                value = innerValue ?? Default;

                return true;
            }
            value = Default;

            ValueTransform?.Invoke(key, value);

            return false;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new NotImplementedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }
    }
}
