using System.Security.Cryptography.X509Certificates;
using HappyMongoDb.Interfaces;
using HappyMongoDb.Models;
using MongoDB.Driver;

namespace HappyMongoDb;

public abstract class MongoDatabaseContextRegistry : IMongoDbContext
{
    protected MongoDatabaseContextRegistry()
    {
        var config = new MongoDatabaseConfiguration();
        OnConfigure(config);

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

        Builder = new MongoModelBuilder(this);
        ModelBuilder(Builder);
    }

    private MongoDbCertificateSettings? CertificateSettings { get; set; }

    private string ConnectionString { get; set; }

    private MongoClient Mongo { get; }

    private IMongoModelBuilder Builder { get; }

    public string DatabaseName { get; private set; }

    public IMongoDatabase Database { get; }

    public IMongoCollection<TEntity> GetRegisteredCollection<TEntity>(out string collectionName)
        where TEntity : IEntityModel, new() =>
        Builder.MongoCollection<TEntity>(out collectionName);

    protected abstract void ModelBuilder(IMongoModelBuilder mapper);

    protected virtual void OnConfigure(MongoDatabaseConfiguration configuration)
    {
        string bonusString = "\nHave you forgotten to call 'base.OnConfigure(configuration)' at the end of the OnConfigure?";
        
        if (string.IsNullOrEmpty(configuration.ConnectionString))
            throw new Exception(
                $"Please provide a connectionstring. Connectionstring is empty. {bonusString}");

        if (string.IsNullOrEmpty(configuration.DatabaseName))
            throw new Exception($"Please provide a databasename. Databasename is empty. {bonusString}");

        ConnectionString = configuration.ConnectionString;
        DatabaseName = configuration.DatabaseName;

        CertificateSettings = configuration.CertificateSettings;
    }
}