namespace HappyMongoDb.Models;

public class MongoDatabaseConfiguration
{
    public string ConnectionString { get; init; }

    public string DatabaseName { get; init; }

    public MongoDbCertificateSettings? CertificateSettings { get; init; } = null;
}