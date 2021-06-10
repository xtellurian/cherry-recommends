import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
const defaultHeaders = { "Content-Type": "application/json" };

export const fetchApiKeys = async ({ success, error, token, page }) => {
  const url = getUrl("api/apiKeys");
  let path = `${url}?${pageQuery(page)}`;

  const response = await fetch(path, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(await response.json());
  }
};

export const createApiKey = async ({ success, error, token, name }) => {
  const url = getUrl("api/apiKeys/exchange");
  const response = await fetch(url, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
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
