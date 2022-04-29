const defaultHeaders = { "Content-Type": "application/json" };
const headers = (token, tenant, environment) => {
  let _headers = defaultHeaders;
  if (token) {
    _headers = { ..._headers, Authorization: `Bearer ${token}` };
  }
  if (tenant) {
    _headers = { ..._headers, "x-tenant": tenant };
  }
  if (environment) {
    _headers = { ..._headers, "x-environment": environment };
  }
  return _headers;
};

export const setKlaviyoApiKeysAsync = async ({
  token,
  tenant,
  id,
  apiKeys,
}) => {
  const url = `api/integratedsystems/${id}/klaviyo/ApiKeys`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
    method: "post",
    body: JSON.stringify(apiKeys),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchKlaviyoListsAsync = async ({ token, tenant, id }) => {
  const url = `api/integratedsystems/${id}/klaviyo/Lists`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
