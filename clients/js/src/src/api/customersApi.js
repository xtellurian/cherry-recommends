import { chunkArray } from "../utilities/chunk";
import { handleErrorResponse } from "../utilities/errorHandling";
import { executeFetch } from "./client/apiClient";

const MAX_ARRAY = 5000;
const basePath = "api/Customers";
export const fetchCustomersAsync = async ({ token, page, searchTerm }) => {
  return await executeFetch({
    path: basePath,
    token,
    page,
    query: {
      "q.term": searchTerm,
    },
  });
};

export const updateMergePropertiesAsync = async ({ token, id, properties }) => {
  return await executeFetch({
    token,
    path: `${basePath}/${id}/properties`,
    method: "post",
    body: properties,
  });
};

export const fetchCustomerAsync = async ({ token, id, useInternalId }) => {
  return await executeFetch({
    path: `${basePath}/${id}`,
    token,
    query: {
      useInternalId,
    },
  });
};

export const fetchUniqueCustomerActionGroupsAsync = async ({ token, id }) => {
  return await executeFetch({
    token,
    path: `${basePath}/${id}/action-groups`,
  });
};

export const fetchLatestRecommendationsAsync = async ({ token, id }) => {
  return await executeFetch({
    token,
    path: `${basePath}/${id}/latest-recommendations`,
  });
};

export const fetchCustomerActionAsync = async ({
  token,
  id,
  category,
  actionName,
}) => {
  return await executeFetch({
    path: `${basePath}/${id}/actions/${category}`,
    token,
    query: {
      actionName,
    },
  });
};

export const uploadUserDataAsync = async ({ token, payload }) => {
  const payloads = chunkArray(payload.users, MAX_ARRAY).map((users) => ({
    users,
  }));
  const responses = [];
  for (const p of payloads) {
    const response = await executeFetch({
      token,
      path: basePath,
      method: "put",
      body: p,
    });
    if (response.ok) {
      responses.push(await response.json());
    } else {
      return await handleErrorResponse(response);
    }
  }
  return responses;
};

export const createOrUpdateCustomerAsync = async ({
  token,
  customer,
  user,
}) => {
  if (user) {
    console.log(
      "user is a deprecated property in createOrUpdateCustomerAsync(). use 'customer'."
    );
  }
  return await executeFetch({
    path: basePath,
    method: "post",
    body: customer || user,
    token,
  });
};

export const fetchCustomersActionsAsync = async ({
  token,
  page,
  id,
  revenueOnly,
}) => {
  return await executeFetch({
    path: `${basePath}/${id}/Actions`,
    token,
    page,
    query: {
      revenueOnly: !!revenueOnly,
    },
  });
};

export const setCustomerMetricAsync = async ({
  token,
  id,
  metricId,
  useInternalId,
  value,
}) => {
  return await executeFetch({
    path: `${basePath}/${id}/Metrics/${metricId}`,
    method: "post",
    token,
    query: {
      useInternalId,
    },
    body: { value },
  });
};

export const deleteCustomerAsync = async ({ token, id, }) => {
  return await executeFetch({
      path: `${basePath}/${id}`,
      token,
      method: "delete",
  });
};