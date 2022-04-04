import { AxiosResponse } from "axios";
type ErrorHandler = (response: AxiosResponse) => Promise<any>;

const defaultErrorResponseHandler = async (response: AxiosResponse) => {
  console.debug(response);

  // is not a promise
  if (response.status >= 500) {
    console.error(`Server responded: ${response.statusText}`);
    console.error(response.data);
    return { error: response.data };
  } else if (response.status >= 400) {
    console.warn(`Server responded: ${response.statusText}`);
    console.warn(response.data);
    throw response.data;
  }
};

let errorResponseHandler = defaultErrorResponseHandler;

export const setErrorResponseHandler = (errorHandler: ErrorHandler) => {
  errorResponseHandler = errorHandler;
};

// this function is called in api.js functions.
export const handleErrorResponse: ErrorHandler = async (
  response: AxiosResponse
) => {
  console.debug("SDK is handling an error response");
  return await errorResponseHandler(response);
};
