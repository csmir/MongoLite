using Microsoft.Extensions.Hosting;
using MongoLite.Bson;
using System.Diagnostics.CodeAnalysis;

namespace MongoLite.Sample
{
    public class ExampleModelService : BackgroundService
    {
        [SuppressMessage("Reliability", "CA2016:Forward the 'CancellationToken' parameter to methods", Justification = "<Pending>")]
        protected override async Task ExecuteAsync(CancellationToken _)
        {
            ExecuteSync();

            var model = await BsonEntity
                .GetAsync<ExampleModel>(x => x.Name == "Example")
                .CreateIfNotExists(x => x.Name = "Example");

            model.Name = "Example2";

            var modelShouldExist = await BsonEntity
                .GetAsync<ExampleModel>(x => x.Name == "Example2")
                .ThrowIfNotExists();

            Console.WriteLine(model.Name);
        }

        private void ExecuteSync()
        {
            var model = BsonEntity
                .Get<ExampleModel>(x => x.Name == "Example")
                .CreateIfNotExists(x => x.Name = "Example");

            model.Name = "Example2";

            var modelShouldExist = BsonEntity
                .Get<ExampleModel>(x => x.Name == "Example2")
                .ThrowIfNotExists();

            Console.WriteLine(model.Name);
        }
    }
}
