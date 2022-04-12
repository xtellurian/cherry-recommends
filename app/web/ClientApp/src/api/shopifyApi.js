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

export const fetchShopifyAppInformationAsync = async ({ token }) => {
  const url = "/api/shopifyappinfo";
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchShopInformationAsync = async ({
  token,
  tenant,
  id,
  environment,
}) => {
  const url = `api/integratedsystems/${id}/shopify/ShopInformation`;
  const response = await fetch(url, {
    headers: headers(token, tenant, environment),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchShopifyInstallUrlAsync = async ({
  token,
  tenant,
  id,
  shopifyUrl,
  redirectUrl,
}) => {
  const url = `api/integratedsystems/${id}/shopify/Install?shopifyUrl=${shopifyUrl}&redirectUrl=${redirectUrl}`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchShopifyAuthorizeUrlAsync = async ({ qs }) => {
  const url = `/api/authorizeurl?${qs}`;
  const response = await fetch(url, {});
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const shopifyConnectAsync = async ({ token, tenant, data, qs }) => {
  const url = `api/shopify/connect?${qs}`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
    method: "post",
    body: JSON.stringify(data),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const shopifyDisconnectAsync = async ({
  token,
  tenant,
  id,
  environment,
}) => {
  const url = `api/integratedsystems/${id}/shopify/Disconnect`;
  const response = await fetch(url, {
    headers: headers(token, tenant, environment),
    method: "post",
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
