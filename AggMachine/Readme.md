[Project Setup](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test)

# CSharp Durable Functions Inventory Management

This is an implementation of the documentation for [.Net Durable Entities](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-dotnet-entities#accessing-entities-through-interfaces).

## Setup

**Dependencies**
 - VSCode
 - VSCode extensions: Azure Functions, CSharp
 - Azure Function Core Tools
 - Dotnet 3.1
 - Storage emulator (Windows) or Storage Account in Azure (Mac)
 - Postman

**Local Settings**

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "CosmosKey": "",
    "CosmosEndpoint": "",
    "CosmosDatabaseName": "inventory",
    "CosmosMDCName": "mdc",
    "CosmosDBConnection": ""
  }
}
```

## Test

Just run `dotnet test` from the `AggMachine` folder.

## Run

To run the project locally enter `func start` at the CLI, or in Visual Studio/VS Code, press `<F5>` in the `ch02` workspace. You should see the function url `http://localhost:7071/api/HttpStart`. Use Postman (or some other tool) to submit a `POST` request with the sample body structure below.

This will eventually be triggered directly via the change feed processor functions.

**For On Hand events**
```json
{
    "id": "div1:store005:0003400029005",
    "divisionId": "div1",
    "storeId": "store005",
    "upc": "0003400029005",
    "countAdjustment": 1000,
    "type": "onHandUpdate",
    "productName": "HERSHEY'S Milk Chocolate Bars",
    "description": "HERSHEY'S Milk Chocolate Bars are the classic full size chocolate candy bars you’ve always enjoyed!",
    "lastShipmentTimestamp": null,
    "lastUpdateTimestamp": "2020-05-13 9:00:00 AM"
}
```

**For Shipment events**
```json
{
    "id": "div1:store005:0003400029005",
    "divisionId": "div1",
    "storeId": "store005",
    "upc": "0003400029005",
    "countAdjustment": 3000,
    "type": "shipmentUpdate",
    "productName": null,
    "description": null,
    "lastShipmentTimestamp": "2020-05-13 9:00:00 AM",
    "lastUpdateTimestamp": "2020-05-13 9:00:00 AM"
}
```