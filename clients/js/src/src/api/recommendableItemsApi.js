import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
import { headers } from "./headers";

import * as pr from "./commonEntity/propertiesApiUtil";

export const fetchItemsAsync = async ({ token, page }) => {
  const url = getUrl("api/RecommendableItems");
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchItemAsync = async ({ token, id }) => {
  const url = getUrl(`api/RecommendableItems/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const createItemAsync = async ({ token, item }) => {
  const url = getUrl(`api/RecommendableItems/`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(item),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const updateItemAsync = async ({ token, id, item }) => {
  const url = getUrl(`api/RecommendableItems/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(item),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const deleteItemAsync = async ({ token, id }) => {
  const url = getUrl(`api/RecommendableItems/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "delete",
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
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
