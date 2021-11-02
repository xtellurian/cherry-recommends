import { executeFetch } from "./client/apiClient";

import * as pr from "./commonEntity/propertiesApiUtil";

export const fetchItemsAsync = async ({ token, page, searchTerm }) => {
  return await executeFetch({
    path: "api/RecommendableItems",
    token,
    page,
    query: {
      "q.term": searchTerm,
    },
  });
};

export const fetchItemAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/RecommendableItems/${id}`,
    token,
  });
};

export const createItemAsync = async ({ token, item }) => {
  return await executeFetch({
    path: "api/RecommendableItems",
    token,
    method: "post",
    body: item,
  });
};

export const updateItemAsync = async ({ token, id, item }) => {
  return await executeFetch({
    path: `api/RecommendableItems/${id}`,
    token,
    method: "post",
    body: item,
  });
};

export const deleteItemAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/RecommendableItems/${id}`,
    token,
    method: "delete",
  });
};

export const getPropertiesAsync = async ({ token, id }) => {
  return await pr.getPropertiesAsync({
    token,
    id,
    api: "RecommendableItems",
  });
};

export const setPropertiesAsync = async ({ token, id, properties }) => {
  return await pr.setPropertiesAsync({
    token,
    id,
    properties,
    api: "RecommendableItems",
  });
};
