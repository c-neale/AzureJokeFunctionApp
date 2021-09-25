# AzureJokeFunctionApp

## Basic Design

The app consists of the following main components

| Resource Type | Purpose |
| --- | --- |
| Azure Function App | Main resource that will house our functions, see table below for function descriptons |
| Azure Storage Account | Required by the Azure Function app.  This will also hold out message queue |
| Azure CosmosDB | Database to store and serve the jokes |


| Function Name | Purpose |
| --- | --- |
| JokeScraperTimerFunction | Function running on a timer. Every X interval (to be determined) the function will grab an RSS feed and split up into individual items.  Items will be places on a storage queue for processing. |
| JokeProcessorQueueFunction | Function will monitor a storage queue for items.  When triggered, it will process the items and store in An Azure CosmosDB |
| JokeServerHttpFunction | When triggered this will grab a joke from the CosmosDB and return it.


## Tools

- Visual Studio 2022 (preview) can probably also use VS2019

- Azure Storage emulator		(for dev, you could also just spin up some Azure resources if you want.)
- Azure CosmosDB emulator		(for dev, you could also just spin up some Azure resources if you want.)

## Dev setup

Create a local.settings.json file in the function app project root, fill in with the following contents.  Make sure to set the Cosmos connection string to point to your CosmosDB instance.
```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "CosmosConnectionString": ""
    }
}
```

## References

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)
[CosmosDB SQL API](https://docs.microsoft.com/en-us/azure/cosmos-db/sql/sql-api-sdk-dotnet-standard)


## Notes

### Cosmos DB connection strings format

```
AccountEndpoint=<url>; AccountKey=<key>;
```
