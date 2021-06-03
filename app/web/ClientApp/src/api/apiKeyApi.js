import { pageQuery } from "./paging";
const defaultHeaders = { "Content-Type": "application/json" };

export const fetchApiKeys = async ({ success, error, token, page }) => {
  let path = `api/apiKeys?${pageQuery(page)}`;

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
  const path = "api/apiKeys/create";
  const response = await fetch(path, {
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
