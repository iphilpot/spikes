import * as w from 'winston';
import * as path from 'path';
import * as fs from 'fs';


const filename = path.join(__dirname, 'created-logfile.log');

//
// Remove the file, ignoring any errors
//
try { fs.unlinkSync(filename); }
catch (ex) { }

const logger = w.createLogger({
    level: 'info',
    format: w.format.combine(
        w.format.timestamp({
            format: 'YYYY-MM-DD HH:mm:ss'
        }),
        w.format.json()
    ),
    transports: [
        new w.transports.Console({format: w.format.simple()}),
        new w.transports.File({ filename })
    ]
});

logger.info('Log this out');

logger.info({
    message: 'Use good message',
    additional: 'properties',
    are: 'passed along'
});
