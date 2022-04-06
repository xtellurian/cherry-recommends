import { tenants } from "cherry.ai";
const defaultHeaders = { "Content-Type": "application/json" };
const headers = (token) =>
  !token
    ? defaultHeaders
    : { ...defaultHeaders, Authorization: `Bearer ${token}` };

export const fetchCurrentTenantAsync = tenants.fetchCurrentTenantAsync;
export const fetchHostingAsync = tenants.fetchHostingAsync;
export const fetchCurrentTenantMembershipsAsync =
  tenants.fetchCurrentTenantMembershipsAsync;
export const createTenantMembershipAsync = tenants.createTenantMembershipAsync;

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
