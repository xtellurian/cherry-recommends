const defaultHeaders = { "Content-Type": "application/json" };
const headers = (token) =>
  !token
    ? defaultHeaders
    : { ...defaultHeaders, Authorization: `Bearer ${token}` };

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

export const fetchShopInformationAsync = async ({ token, id }) => {
  const url = `api/integratedsystems/${id}/shopify/ShopInformation`;
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchShopifyInstallUrlAsync = async ({
  token,
  id,
  shopifyUrl,
  redirectUrl,
}) => {
  const url = `api/integratedsystems/${id}/shopify/Install?shopifyUrl=${shopifyUrl}&redirectUrl=${redirectUrl}`;
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const shopifyConnectAsync = async ({ token, id, code }) => {
  const url = `api/integratedsystems/${id}/shopify/Connect`;
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(code),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const shopifyDisconnectAsync = async ({ token, id }) => {
  const url = `api/integratedsystems/${id}/shopify/Disconnect`;
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
