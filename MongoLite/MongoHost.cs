using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoLite.Bson;

namespace MongoLite
{
    /// <summary>
    ///     A hosted service that connects to a MongoDB database, allowing use of a statically available <see cref="IMongoDatabase"/> instance.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    public class MongoHost(ILogger<MongoHost> logger, IConfiguration configuration) : IHostedService
    {
        private static MongoClient? _client;
        private static IMongoDatabase? _database;

        private readonly MongoUrl _url = new(configuration.GetConnectionString("DefaultConnection"));
        private readonly string _databaseName = configuration.GetConnectionString("DefaultConnectionName")!;

        /// <summary>
        ///     Gets the connection status of the database. This status is statically accessible, but will only be set to <see langword="true"/> if the connection is successful through the <see cref="IHostedService"/>'s instance activation.
        /// </summary>
        public static bool IsConnected { get; private set; } = false;

        /// <summary>
        ///     Gets a <see cref="MongoCollectionBase{T}"/> for the specified <typeparamref name="T"/> model.
        /// </summary>
        /// <typeparam name="T">The implementation of <see cref="BsonEntity"/> to get a collection for.</typeparam>
        /// <param name="name">The name of the collection.</param>
        /// <returns></returns>
        public static IMongoCollection<T> GetMongoCollection<T>(string name)
            where T : BsonEntity, new()
        {
            if (WaitConnection())
            {
                var collection = _database?.GetCollection<T>(name);

                return collection is null
                    ? throw new ArgumentException("The collection could not be found, created, or is temporarily unavailable.", nameof(name))
                    : collection;
            }

            throw new MongoClientException("The database timed out.");
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_url is not null)
            {
                logger.LogInformation("Configuring Database connection...");

                if (string.IsNullOrEmpty(_databaseName))
                    throw new ArgumentNullException(_databaseName, "Database name is null.");

                _client = new MongoClient(_url);

                if (_client is not null)
                {
                    _database = _client.GetDatabase(_databaseName);

                    logger.LogInformation("Connecting to Database...");

                    if (!TryConnection())
                        throw new InvalidOperationException("Databases could not connect.");

                    logger.LogInformation("Database succesfully connected.");
                    IsConnected = true;
                }
                else
                    throw new InvalidOperationException("Database client is unavailable.");
            }
            else
                logger.LogError("Database lacks connection arguments.");

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogError("Database disconnecting...");

            IsConnected = false;

            _client = null;
            _database = null;

            logger.LogInformation("Database disconnected.");

            return Task.CompletedTask;
        }

        private static bool TryConnection()
        {
            try
            {
                _client?.ListDatabaseNames();
                return true;
            }
            catch (MongoException)
            {
                return false;
            }
        }

        private static bool WaitConnection()
        {
            var retryCounter = 0;
            while (!IsConnected)
            {
                if (retryCounter >= 30)
                    return false;

                Task.Delay(50);

                retryCounter++;
            }

            return true;
        }
    }
}
