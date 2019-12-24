const df = require("durable-functions");

module.exports = df.entity(async function (context) {
    let currentValue = context.df.getState(() => 0);

    switch (context.df.operationName) {
        case "add":
            const amount = context.df.getInput();
            currentValue += amount;
            break;
        case "reset":
            currentValue = 0;
            break;
    }

    context.df.setState(currentValue);
});