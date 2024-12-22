namespace MongoLite
{
    /// <summary>
    ///     Represents the state of a model.
    /// </summary>
    public enum EntityState
    {
        /// <summary>
        ///     The model is being deserialized.
        /// </summary>
        Deserializing,

        /// <summary>
        ///     The model is ready to be used.
        /// </summary>
        Ready,

        /// <summary>
        ///     The model has been deleted.
        /// </summary>
        Deleted,

        /// <summary>
        ///     The model is in a stateless state.
        /// </summary>
        Stateless,
    }
}
