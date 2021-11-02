import { executeFetch } from "../../client/apiClient";

export const fetchRecommenderTargetVariableValuesAsync = async ({
  recommenderApiName,
  name,
  token,
  id,
}) => {
  return await executeFetch({
    path: `api/recommenders/${recommenderApiName}/${id}/TargetVariableValues`,
    token,
    query: {
      name,
    },
  });
};

export const createRecommenderTargetVariableValueAsync = async ({
  recommenderApiName,
  targetVariableValue,
  token,
  id,
}) => {
  return await executeFetch({
    path: `api/recommenders/${recommenderApiName}/${id}/TargetVariableValues`,
    token,
    method: "post",
    body: targetVariableValue,
  });
};
