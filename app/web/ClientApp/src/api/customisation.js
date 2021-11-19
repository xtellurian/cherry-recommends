import { errorHandling } from "cherry.ai";

const customErrorResponseHandler = async (response) => {
  const json = await response.json();
  console.log(`Server responded: ${response.statusText}`);
  console.log(json);

  throw json;
};

export const configure = () => {
  errorHandling.setErrorResponseHandler(customErrorResponseHandler);
};
