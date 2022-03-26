using MongoR.Utils;

namespace MongoR.Models;

public class MongoDatabaseConfiguration
{
    public DatabaseMode DatabaseMode { get; private set; } = default(DatabaseMode);
    
    public string? ConnectionString { get; set; } = null;

    public string? DatabaseName { get; set; } = null;

    public MongoDbCertificateSettings? CertificateSettings { get; init; } = null;

    public void NoDatabaseConnection()
    {
        DatabaseMode = DatabaseMode.NoDatabaseConnection;
    }
}