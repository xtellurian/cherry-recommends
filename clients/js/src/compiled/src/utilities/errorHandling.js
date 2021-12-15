// type ErrorHandler = (response: Response) => Promise<any>;
const defaultErrorResponseHandler = async (response) => {
    const json = await response.json();
    console.log(`Server responded: ${response.statusText}`);
    console.log(json);
    if (response.status >= 500) {
        return { error: json };
    }
    else if (response.status >= 400) {
        throw json;
    }
};
let errorResponseHandler = defaultErrorResponseHandler;
export const setErrorResponseHandler = (errorHandler) => {
    errorResponseHandler = errorHandler;
};
// this function is called in api.js functions.
export const handleErrorResponse = async (response) => {
    console.log("SDK is handling an error response");
    return await errorResponseHandler(response);
};
// the below all function as handlers of a fetch promise rejected
const defaultErrorFetchHandler = (ex) => {
    throw ex;
};
let errorFetchHandler = defaultErrorFetchHandler;
export const handleErrorFetch = (ex) => {
    errorFetchHandler(ex);
};
export const setErrorFetchHandler = (handler) => {
    errorFetchHandler = handler;
};
