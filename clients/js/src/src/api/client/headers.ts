// tenant
let storedTenant: string | null = null;

export const setTenant = (tenant: string): void => {
  console.debug(`Setting tenant: ${tenant}`);
  storedTenant = tenant;
};
// environment
let defaltEnvironmentId: number | null = null;
export const setDefaultEnvironmentId = (e: number) => {
  defaltEnvironmentId = e;
};

let defaultApiKey: string | null = null;
export const setDefaultApiKey = (k: string) => {
  defaultApiKey = k;
};

export const defaultHeaders: { [key: string]: string | number } = {
  "Content-Type": "application/json",
};

export const headers = (
  token: string | null | undefined,
  apiKey: string | null | undefined
) => {
  let headers = { ...defaultHeaders };
  if (storedTenant) {
    headers["x-tenant"] = storedTenant;
  }
  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }
  if (apiKey) {
    headers["x-api-key"] = `${apiKey}`; // ensure its a string
  } else if (defaultApiKey) {
    headers["x-api-key"] = `${defaultApiKey}`; // ensure its a string
  }
  if (defaltEnvironmentId) {
    headers["x-environment"] = defaltEnvironmentId;
  }
  return headers;
};
