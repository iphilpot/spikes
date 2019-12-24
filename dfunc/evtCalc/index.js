const df = require("durable-functions");

module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    if (req.body && req.body.messageType) {
        const client = df.getClient(context);
        
        const id = req.body.entityKey;
        const entityName = req.body.entityName;
        const operationName = req.body.operationName;
        const operationContent = parseInt(req.body.operationContent);
        const entityId = new df.EntityId(entityName, id); // myCount

        switch (req.body.messageType) {
            case "calc.add":
            case "calc.reset":
                await performOperation(entityId, client, operationName, operationContent);
                context.done()
                break;
            case "calc.get":
                await retrunStatus(entityId, client, context);
                break;
        }

    } else {
        context.res = {
            status: 400,
            body: "Please pass a calc.* message in the request body."
        };
    }
};

async function retrunStatus(entityId, client, context) {
    const stateResponse = await client.readEntityState(entityId);
    context.res = { body: `State: ${stateResponse.entityState}` };
}

async function performOperation(entityId, client, operationName, operationContent) {
    await client.signalEntity(entityId, operationName, operationContent);
}