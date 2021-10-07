import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
import { headers } from "./headers";

export const fetchApiKeysAsync = async ({ token, page }) => {
  const url = getUrl("api/apiKeys");
  let path = `${url}?${pageQuery(page)}`;
  const response = await fetch(path, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchApiKeys = async ({ success, error, token, page }) => {
  fetchApiKeysAsync({ token, page }).then(success).catch(error);
};

export const createApiKeyAsync = async ({ token, payload }) => {
  const url = getUrl("api/apiKeys");
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(payload),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const createApiKey = async ({ success, error, token, payload }) => {
  createApiKeyAsync({ token, payload }).then(success).catch(error);
};

export const exchangeApiKeyAsync = async ({ apiKey }) => {
  const url = getUrl("api/apiKeys/exchange");
  const response = await fetch(url, {
    headers: headers(),
    method: "post",
    body: JSON.stringify({ apiKey }),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const exchangeApiKey = ({ success, error, apiKey }) => {
  const url = getUrl("api/apiKeys/exchange");
  exchangeApiKeyAsync({ apiKey }).then(success).catch(error);
};

export const deleteApiKeyAsync = async ({ token, id }) => {
  const url = getUrl(`api/apiKeys/${id}`);
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
