import * as w from 'winston';

const logger = w.createLogger({
    level: 'info',
    transports: [
        new w.transports.Console({format: w.format.simple()})
    ]
});

// Created types/custom.d.ts and updated package.json to include file
global.myConfig = {a: 1, b: 2 };

import { returnNumA } from './stuff';

logger.info(`Global a: ${returnNumA()}`);