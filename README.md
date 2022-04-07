# MongoR
A EF Core like style to handle Mongo Db


# How to use

This is how you use this as of version 0.2.x

Define a MongoDatabaseContext as:

```c#

public class MyEntity
{
    public string A { get; set; }
}

public class MyDatabaseContext : MongoDatabaseContextRegistry
{
    private readonly string _connectionString;

    public ShopifyDatabaseContext2(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    protected override void ModelBuilder(IMongoModelBuilder mapper)
    {
        mapper.MapEntity<MyEntity>("MyEntity");
    }

    protected override void OnConfigure(MongoDatabaseConfiguration configuration)
    {
        configuration.DatabaseName = "MyDataBase";
        configuration.ConnectionString = _connectionString;

        base.OnConfigure(configuration);
    }
}

```

After you can get the mongo collection and access the data:

```c#

public class MyClass
{
    public static async Task MyMethod()
    {
        var test = new MyDatabaseContext();

        var collection = test.GetRegisteredCollection<MyEntity>(out string collectionName);

        // Query the collection by the property
        MyEntity? getBy = await collection.GetByAsync(p => p.MyProperty == "test");

        // Query all the items in the collection
        IEnumerable<MyEntity> getAll = await collection.GetAllAsync();

        // Insert an item in the collection
        MyEntity insert = await collection.InsertAsync(new MyEntity { MyProperty = "test" });

        // Delete an item in the collection that has MyProperty = "test2"
        MyEntity delete = await collection.DeleteAsync(c => c.MyProperty == "test2");

        // Replaces an item in the collection that has MyProperty = "test2" with "test23"
        MyEntity replace = await collection.ReplaceAsync(c => c.MyProperty == "test2", 
            new MyEntity { MyProperty = "test23" });

        // Performs an Upsert for an item in the collection that has MyProperty = "test2" with "test23"
        MyEntity upsert = await collection.InsertOrUpdateAsync(c => c.MyProperty == "test2",
            new MyEntity { MyProperty = "test23" });
    }
}

```

## With DI container

There is aslo support to auto-register all collections in ms- DI container.

```c#

public class MyClass
{
    public static async Task MyMethod()
    {
        var services = new ServiceCollection();

        // Register all the models that is registered in MyDatabaseContext -> ModelBuilder
        services.AddMongoRCollections<MyDatabaseContext>("<connection-string>");

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        
        // Get the repository context
        var repositoryContext = serviceProvider.GetRequiredService<IMongoRepositoryContext<MyEntity>>();

        // Use the already defined methods...
        await repositoryContext.InsertOrUpdateAsync(p => p.ProductId == request.ProductId, product);

        // Or use the native methods directly on the IMongoCollection<MyEntity>
        await _repositoryContext.Collection.InsertManyAsync(...);
    }
}

```