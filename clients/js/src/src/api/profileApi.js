import { executeFetch } from "./client/apiClient";

export const setMetadataAsync = async ({ token, metadata }) => {
  return await executeFetch({
    path: "api/profile/metadata",
    token,
    method: "post",
    body: metadata,
  });
};

export const getMetadataAsync = async ({ token }) => {
  return await executeFetch({
    path: "api/profile/metadata",
    token,
  });
};
