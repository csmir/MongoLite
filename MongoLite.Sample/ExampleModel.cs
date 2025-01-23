using MongoLite.Bson;

namespace MongoLite.Sample
{
    public class ExampleModel : AtomicEntity<ExampleModel>
    {
        /// <summary>
        ///     Gets or sets the name of the model.
        /// </summary>
        public string Name { get; set => Save(field = value); } = string.Empty; // This property is saved atomically when written to.
    }
}
