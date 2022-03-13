import { executeFetch } from "./client/apiClient";

export const fetchSegmentsAsync = async ({ token, page }) => {
  return await executeFetch({
    path: "api/Segments",
    token,
    page,
  });
};

export const fetchSegmentAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/segments/${id}`,
    token,
  });
};

export const createSegmentAsync = async ({ token, payload }) => {
  return await executeFetch({
    path: "api/Segments",
    token,
    method: "post",
    body: payload,
  });
};

export const deleteSegmentAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/Segments/${id}`,
    token,
    method: "delete",
  });
};

export const addCustomerAsync = async ({ token, id, customerId }) => {
  return await executeFetch({
    path: `api/Segments/${id}/Customers/${customerId}`,
    token,
    method: "post",
  });
};

export const removeCustomerAsync = async ({ token, id, customerId }) => {
  return await executeFetch({
    path: `api/Segments/${id}/Customers/${customerId}`,
    token,
    method: "delete",
  });
};

export const fetchSegmentCustomersAsync = async ({
  token,
  page,
  id,
  searchTerm,
  weeksAgo,
}) => {
  return await executeFetch({
    path: `api/Segments/${id}/Customers`,
    token,
    page,
    query: {
      "q.term": searchTerm,
      "q.weeksAgo": weeksAgo,
    },
  });
};
