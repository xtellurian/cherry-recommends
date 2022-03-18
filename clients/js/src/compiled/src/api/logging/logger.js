const info = (message) => {
    console.info(`${message}`);
};
const error = (error) => {
    console.error(`${error}`);
};
const debug = (message) => {
    console.debug(`${message}`);
};
export default {
    debug,
    info,
    error,
};
