import { errorHandling } from "cherry.ai";

const customErrorResponseHandler = async (response) => {
  const json = await response.json();
  console.error(`Server responded: ${response.statusText}`);
  console.error(json);

  throw json;
};

export const configure = () => {
  errorHandling.setErrorResponseHandler(customErrorResponseHandler);
};
