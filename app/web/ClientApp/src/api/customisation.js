import { errorHandling } from "cherry.ai";

const customErrorResponseHandler = async (axiosResponse) => {
  console.warn(axiosResponse);
  console.warn(`Server responded: ${axiosResponse.statusText}`);

  throw axiosResponse.data;
};

export const configure = () => {
  errorHandling.setErrorResponseHandler(customErrorResponseHandler);
};
