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


