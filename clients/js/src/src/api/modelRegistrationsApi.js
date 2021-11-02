import { executeFetch } from "./client/apiClient";

export const fetchModelRegistrationsAsync = async ({ token, page }) => {
  return await executeFetch({
    path: "api/ModelRegistrations",
    token,
    page,
  });
};

export const fetchModelRegistrationAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/ModelRegistrations/${id}`,
    token,
  });
};

export const deleteModelRegistrationAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/ModelRegistrations/${id}`,
    token,
    method: "delete",
  });
};

export const createModelRegistrationAsync = async ({ token, payload }) => {
  return await executeFetch({
    path: "api/ModelRegistrations",
    token,
    method: "post",
    body: payload,
  });
};

export const invokeModelAsync = async ({ token, modelId, features }) => {
  return await executeFetch({
    path: `api/ModelRegistrations/${modelId}/invoke`,
    token,
    method: "post",
    body: features,
  });
};
