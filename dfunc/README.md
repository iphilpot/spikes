# Spike on Durable Functions
This spike is for using `durableClient` and `entityTriggers`. This use case is for when you don't want orchestration. It's a simple counter that is triggered from an `HTTPTrigger`. It adds one to the counter, resets to 0, or gets the state of the counter.

## Notes for setting up a new project
You only need to do this the first time when creating this type of project. At the time I did this [Extension Bundles](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-register#extension-bundles) do not support DurableTask v2.x. I had to run these commnads on **File > New Project**.

- Install update durable task ext `func extension install --package Microsoft.Azure.WebJobs.Extensions.DurableTask --version 2.1.0`
- Make sure you have dotnet version 2.x installed to compile (I had 3.x, they can be installed side by side) `sudo apt install dotnet-sdk-2.2`

## To run this project
I've not actually deployed this to azure. I created a Storage Account and updated local.settings.json with the connection string. I tried to use Azurite, but it doesn't support tables. Durable functions implement the event sourcing pattern using table storage, so I had to use a real SA not an emulated. (I'm on ubuntu and doesn't support the Azure Storage Emulator).

### Pre-reqs

- VScode + Azure Function Ext
- Azure Function Core Tools
- Create a storage account > connString in `local.settings.json`
- Run: `func extension sync`
- Run: `npm install`
- The folder: `messages/postman` contains three HTTP POSTs

## Architecture
This contains two functions:

- `evtCalc` Function is basically a message router. It will signal and operation on the entity or request state.
- `evtCounter` Function can only be called by "starter" function. It is an entity that maintains its own state and is a simple counter.

## Note to self
The fact that we are able to get state doesn't really support a messaging infrastructure like EventGrid. That said it could be reworked using different entityies etcs.

Would need to register EventGrid using:

```javascript
switch(evt.eventType) {
    case "Microsoft.EventGrid.SubscriptionValidationEvent":
        handleEventGridSubscriptionValidation(context, evt);
        break;

// ...

function handleEventGridSubscriptionValidation(context, evt) {
    var code = evt.data.validationCode;
    context.res = { status: 200, body: { "ValidationResponse": code } };
}
```