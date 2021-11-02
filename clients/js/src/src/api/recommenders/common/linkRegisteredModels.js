import { executeFetch } from "../../client/apiClient";

export const fetchLinkedRegisteredModelAsync = async ({
  recommenderApiName,
  token,
  id,
}) => {
  return await executeFetch({
    path: `api/recommenders/${recommenderApiName}/${id}/ModelRegistration`,
    token,
  });
};

export const createLinkedRegisteredModelAsync = async ({
  recommenderApiName,
  token,
  id,
  modelId,
}) => {
  return await executeFetch({
    path: `api/recommenders/${recommenderApiName}/${id}/ModelRegistration`,
    token,
    method: "post",
    body: { modelId },
  });
};
