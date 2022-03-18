import { executeFetch } from "./client/apiClient";

console.warn("Deprecation Notice: Features are replaced by Metrics.")
export const fetchFeatureGeneratorsAsync = async ({ page, token }) => {
  return await executeFetch({
    path: "api/FeatureGenerators",
    token,
    page,
  });
};

export const createFeatureGeneratorAsync = async ({ token, payload }) => {
  return await executeFetch({
    path: "api/FeatureGenerators",
    token,
    method: "post",
    body: payload,
  });
};

export const deleteFeatureGeneratorAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/FeatureGenerators/${id}`,
    token,
    method: "delete",
  });
};

export const manualTriggerFeatureGeneratorsAsync = async ({ token, id }) => {
  return await executeFetch({
    token,
    path: `api/FeatureGenerators/${id}/Trigger`,
    method: "post",
    body: {},
  });
};
