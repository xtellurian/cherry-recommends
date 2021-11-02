import { executeFetch } from "./client/apiClient";

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
