const defaultHeaders = { "Content-Type": "application/json" };
const headers = (token) =>
  !token
    ? defaultHeaders
    : { ...defaultHeaders, Authorization: `Bearer ${token}` };

export const fetchCurrentTenantAsync = async ({ token }) => {
  const url = "api/tenants/current";
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHostingAsync = async ({ token }) => {
  const url = "api/tenants/hosting";
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchStatusAsync = async ({ token, name }) => {
  const url = `api/tenants/status/${name}`;
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const createTenantAsync = async ({
  token,
  name,
  termsOfServiceVersion,
  dryRun,
}) => {
  const url = `api/tenants?dryRun=${!!dryRun}`;
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify({ name, termsOfServiceVersion }),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchMembershipsAsync = async ({ token }) => {
  const url = "api/tenants/memberships";
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
