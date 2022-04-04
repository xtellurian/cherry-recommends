import axios from "axios";
import { getBaseUrl } from "./baseUrl";
let currentInstance = null;
let currentConfig = null;
const defaultConfig = {
    baseUrl: getBaseUrl(),
};
// sets the current config
const initialise = (config = defaultConfig) => {
    currentConfig = config;
    return axios.create({
        baseURL: config.baseUrl,
        timeout: config.timeout,
        headers: config.tenant ? { "x-tenant": config.tenant } : {},
        validateStatus: function (status) {
            // return status < 500; // Resolve only if the status code is less than 500
            return true; // always resolve the promise
        },
    });
};
export const current = (config) => {
    // if current instance is null or undefined
    if (!currentInstance) {
        currentInstance = initialise(config);
    }
    else if (!config) {
        currentInstance = initialise();
    }
    // if config isn't exactly the same object
    else if (config !== currentConfig) {
        if (config.baseUrl !== (currentConfig === null || currentConfig === void 0 ? void 0 : currentConfig.baseUrl) ||
            config.tenant !== currentConfig.tenant ||
            config.timeout !== currentConfig.timeout) {
            // then create a new instance and config
            currentInstance = initialise(config);
        }
    }
    return currentInstance;
};
