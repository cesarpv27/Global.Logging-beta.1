# Global.Logging

The 'Global.Logging' NuGet package is intended to provide a logging service that allows storing the logging data on several cloud storage services. Currently, the integration with Azure Table Storage and Azure Blob Storage has been implemented, but the future plan is to add other storage services, like MongoDB. The 'Global.Logging' NuGet package has a modular and layered design that allows adding new storage integrations without much additional work.

The 'Global.Logging' NuGet package has several capabilities:

. Integration with .NET dependency injection.
. Optionally allows customization of the table name, blob container name, blob name, and the generation of partition key and row key names, depending on the target logging storage.
. Flexibility and adaptability to customize the logging service with filters, entity classes, factory classes, and more, including the logging service class itself.
. The labeled verbose capability allows for defining up to 10 additional fields in the stored entity, with the option to customize the source of the stored data based on the received 'verbose' parameter.
. Provides thread-safe capability to ensure reliable multithreading operations.
. Detailed responses.
. Retries options.

Let's talk through examples.

1. Integration with .NET dependency injection.

The 'Global.Logging' NuGet package includes a custom dependency injection implementation in the extension method 'AddDefaultGlobalLoggingServices' located in the 'Global.Logging.Extensions.ServiceCollectionExtensions' class. This method is configured with default values; however, you can create your own implementation if needed.

If you decide to use the default dependency injection implementation, just add something like this to your code:

```csharp
	var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((host, builder) =>
    {
        builder.AddJsonFile(SettingConstants.DefaultAppSettingsPath, optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .AddCommandLine(args);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddConfigurations(hostContext.Configuration)
        .AddCustomServices()
        .AddDefaultGlobalLoggingServices(
            sp =>
            {
                var azStorageSettings = sp.GetRequiredService<IOptions<AzureStorageSettings>>().Value; // Storage service access settings
                return azStorageSettings.ConnectionString!;
            });
    }).Build();
```

In the previous example, we call the 'AddDefaultGlobalLoggingServices' method, providing a lambda expression that includes the Azure storage access settings.

After that configuration, and possibly your custom settings, you can begin using the configured logging service.

- Log to Azure Table Storage.
When adding a new log, you can specify various information and options, including the 'SeverityLevel'. In the default behavior, this field is used to generate the log IDs and the name in the storage service. For example, using the default behavior in Azure Table Storage, if you add a log, the logger will create a table name using the predefined pattern:

"Logger" word appended with the current year, month, and the following suffix: "Low" if the Global.Logging.Domain.ILog.SeverityLevel is Global.Logging.Domain.SeverityLevel.Info or Global.Logging.Domain.SeverityLevel.Warning, otherwise "High". "Logger200101Low" "Logger200101High".

The following example show how to add a simple log:

```csharp
    public class Startup
    {
        private readonly IAzLogger _azLogger;
        public Startup(IAzLogger azLogger)
        {
            _azLogger = azLogger;
        }

        public async Task AddEntryAsync()
        {
            var response = await _azLogger.AddToTableAsync(
                this.GetType().Name,
                SeverityLevel.Info);
		}
	}
```

In the previous example, the logger creates a table with the name 'Logger[currentYearMonth]Low', and an entry with PartitionKey set to [today's date] and RowKey set to Info_Startup_[random Guid], where 'Low' indicates how the log entry was categorized, 'Info' represents the 'SeverityLevel', and 'Startup' is the name of the class that was included as the first parameter.

2. Customizing the table name, blob container name, blob name, and the generation of partition key and row key names, depending on the target logging storage.

It may be necessary to customize the table name, PartitionKey, and RowKey. In that case, it is possible to personalize them in several ways. One way is as follows:

```csharp
    public class Startup
    {
        private readonly IAzLogger _azLogger;
        public Startup(IAzLogger azLogger)
        {
            _azLogger = azLogger;
        }

        public async Task AddEntryAsync()
        {		
            await _azLogger.SetGenerateLogTableNameDelAsync(GenerateCustomTableName);
            await _azLogger.SetGeneratePartitionKeyDelAsync(GenerateCustomPartitionKey);
            await _azLogger.SetGenerateRowKeyDelAsync(GenerateCustomRowKey);

            var response = await _azLogger.AddToTableAsync(
                this.GetType().Name,
                SeverityLevel.Info);
		}
		
        public virtual string GenerateCustomTableName(
            ReadOnlyRawLog<Exception> readOnlyRawLog,
            string partitionKey,
            string rowKey)
        {
            return "MyCustomTable";
        }

        public virtual string GenerateCustomPartitionKey(ReadOnlyRawLog<Exception> readOnlyRawLog)
        {
            return "MyCustomPartitionKey";
        }

        public virtual string GenerateCustomRowKey(ReadOnlyRawLog<Exception> readOnlyRawLog)
        {
            return "MyCustomRowKey";
        }
	}
```

Other method overloads can be used to achieve the same behavior using lambda expressions. In the case of Azure Blob Storage, the customization can be done in a similar way.

3. Flexibility and adaptability to customize the logging service with filters, entity classes, factory classes, and more, including the logging service class itself.

The majority of classes in the 'Global.Logging' NuGet package are generic, allowing the use of custom implementations for entity classes, factory classes, and others. 

The package also has the capability to filter log operations depending on the needs of each circumstance. In some circumstances, it may be necessary to add or read only the entries with 'Exception' or 'FatalException' SeverityLevel. In that case or similar, a customization can be applied to achieve this goal.

The package already contains a predefined log filter based on the SeverityLevel, but feel free to implement your own filter. Implementing the 'Global.Logging.Models.IAzLogFilter' interface can result in a new log filter that can be used in the logger's behavior. This interface contains two properties (IsWritingAllowed, IsReadingAllowed) used in the logger to filter entries either add or get operations. One important thing to note is that the logger doesn't have the capability to filter the retrieved entities until they have been recovered from the target storage.

The 'Global.Logging.IAzSeverityLevelLogFilter' interface is a customization of the 'Global.Logging.Models.IAzLogFilter' interface, designed to filter entries based on the 'SeverityLevel'.
The 'Global.Logging.AzSeverityLevelLogFilter' class implements the 'Global.Logging.IAzSeverityLevelLogFilter' interface and can be used to modify the logger's behavior based on the 'SeverityLevel'. It also contains predefined methods that can be set for the 'IsWritingAllowed' and 'IsReadingAllowed' properties. To maintain thread-safe integrity in the logger, we recommend that any instance of 'AzSeverityLevelLogFilter' be created using the 'AddTransient' injection method in .Net.

The 'Global.Logging' NuGet package includes another dependency injection implementation in the extension method 'AddGlobalLoggingServicesWithLogFilter' located in the 'Global.Logging.Extensions.ServiceCollectionExtensions' class. This method is configured with default values, including the filter injection; however, you can create your own implementation if needed. Additionally, the method receives a generic delegate as the last parameter, which must return an instance of 'AzSeverityLevelLogFilter'.
The following code shows and example that how to use the extension method 'AddGlobalLoggingServicesWithLogFilter':

```csharp

	var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((host, builder) =>
    {
        builder.AddJsonFile(SettingConstants.DefaultAppSettingsPath, optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .AddCommandLine(args);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddConfigurations(hostContext.Configuration)
        .AddCustomServices()
        .AddGlobalLoggingServicesWithLogFilter(
            sp =>
            {
                var azStorageSettings = sp.GetRequiredService<IOptions<AzureStorageSettings>>().Value;
                return azStorageSettings.ConnectionString!;
            },
            () =>
            {
                var azSeverityLevelLogFilter = new AzSeverityLevelLogFilter<ReadOnlyRawLog<Exception>, ReadOnlyLog>();

                azSeverityLevelLogFilter.IsWritingAllowed = azSeverityLevelLogFilter.AllowHighSeverityLevelForWriting;
                azSeverityLevelLogFilter.IsReadingAllowed = azSeverityLevelLogFilter.AllowHighSeverityLevelForReading;

                return azSeverityLevelLogFilter;
            });
    }).Build();
```
In the previous example, we call the 'AddGlobalLoggingServicesWithLogFilter' method, providing a lambda expression in the first parameter that includes the Azure storage access settings, and a second parameter with a lambda expression for creating the 'AzSeverityLevelLogFilter' instance.

Having the corresponding configurations, it is possible to add logs as follows:

```csharp
    public class Startup
    {
        private readonly IAzLogger _azLogger;
        public Startup(IAzLogger azLogger)
        {
            _azLogger = azLogger;
        }

        public async Task AddEntryAsync()
        {
            var response1 = await _azLogger.AddToTableAsync(
                this.GetType().Name,
                SeverityLevel.Info);

            var response2 = await _azLogger.AddToTableAsync(
                this.GetType().Name,
                SeverityLevel.FatalException);
		}
	}
```

In that example, the first attempt to add should result in a response with 'Status' set to 'Failure', and the second should result in a response with 'Status' set to 'Success'. Only the second entry should be added.


*** (To be continue) ***
























