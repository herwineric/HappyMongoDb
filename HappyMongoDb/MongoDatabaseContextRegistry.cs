using System.Security.Cryptography.X509Certificates;
using HappyMongoDb.Interfaces;
using HappyMongoDb.Models;
using HappyMongoDb.Utils;
using MongoDB.Driver;

namespace HappyMongoDb;

public abstract class MongoDatabaseContextRegistry : IMongoDbContext
{
    protected MongoDatabaseContextRegistry()
    {
        ManageSettings();
    }

    private MongoDbCertificateSettings? CertificateSettings => DatabaseConfiguration.CertificateSettings;

    private string ConnectionString => DatabaseConfiguration.ConnectionString!;

    private IMongoClient Mongo { get; set; }

    private IMongoModelBuilder Builder { get; set; }

    public MongoDatabaseConfiguration DatabaseConfiguration { get; private set; }

    public string DatabaseName => DatabaseConfiguration.DatabaseName!;

    public IMongoDatabase Database { get; private set; }

    public IMongoCollection<TEntity> GetRegisteredCollection<TEntity>(out string collectionName)
        where TEntity : IEntityModel, new() =>
        Builder.MongoCollection<TEntity>(out collectionName);

    private void ManageSettings()
    {
        var config = new MongoDatabaseConfiguration();
        OnConfigure(config);

        if (config.DatabaseMode == DatabaseMode.UseDatabaseConnection)
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(ConnectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            if (CertificateSettings is not null)
            {
                var cert = new X509Certificate2(CertificateSettings.Path, CertificateSettings.Passphrase);
                settings.SslSettings = new SslSettings
                {
                    ClientCertificates = new List<X509Certificate> { cert }
                };
            }

            Mongo = new MongoClient(settings);

            Database = Mongo.GetDatabase(DatabaseName);
        }

        Builder = new MongoModelBuilder(Database);
        ModelBuilder(Builder);
    }

    protected abstract void ModelBuilder(IMongoModelBuilder mapper);

    protected virtual void OnConfigure(MongoDatabaseConfiguration configuration)
    {
        DatabaseConfiguration = configuration;

        if (configuration.DatabaseMode == DatabaseMode.NoDatabaseConnection)
            return;

        var bonusString =
            "\nHave you forgotten to call 'base.OnConfigure(configuration)' at the end of the OnConfigure?";

        if (string.IsNullOrEmpty(configuration.ConnectionString))
            throw new Exception(
                $"Please provide a connectionstring. Connectionstring is empty. {bonusString}");

        if (string.IsNullOrEmpty(configuration.DatabaseName))
            throw new Exception($"Please provide a databasename. Databasename is empty. {bonusString}");
    }
}