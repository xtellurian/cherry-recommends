import { executeFetch } from "./client/apiClient";

export const fetchIntegratedSystemsAsync = async ({ token, page }) => {
  return await executeFetch({
    path: "api/IntegratedSystems",
    token,
    page,
  });
};

export const fetchIntegratedSystemAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/IntegratedSystems/${id}`,
    token,
  });
};

export const renameAsync = async ({ token, id, name }) => {
  return await executeFetch({
    path: `api/integratedSystems/${id}/name`,
    token,
    method: "post",
    query: {
      name,
    },
  });
};

export const createIntegratedSystemAsync = async ({ token, payload }) => {
  return await executeFetch({
    path: "api/IntegratedSystems",
    token,
    method: "post",
    body: payload,
  });
};

export const deleteIntegratedSystemAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/integratedSystems/${id}`,
    token,
    method: "delete",
  });
};

export const fetchWebhookReceiversAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/integratedSystems/${id}/webhookreceivers`,
    token,
  });
};

export const createWebhookReceiverAsync = async ({
  token,
  id,
  useSharedSecret,
}) => {
  return await executeFetch({
    path: `api/integratedSystems/${id}/webhookreceivers`,
    token,
    method: "post",
    body: {}, // empty, no args
    query: {
      useSharedSecret,
    },
  });
};
