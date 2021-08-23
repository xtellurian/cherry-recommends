import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
import { headers } from "./headers";

export const fetchApiKeys = async ({ success, error, token, page }) => {
  const url = getUrl("api/apiKeys");
  let path = `${url}?${pageQuery(page)}`;

  const response = await fetch(path, {
    headers: headers(token),
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(await response.json());
  }
};

export const createApiKey = async ({ success, error, token, name }) => {
  const url = getUrl("api/apiKeys/create");
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify({ name }),
  });
  if (response.ok) {
    const data = await response.json();
    success(data);
  } else {
    error(await response.json());
  }
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
