let defaltEnvironmentId = null;

export const setDefaultEnvironmentId = (e) => {
  defaltEnvironmentId = e;
};

export const defaultHeaders = { "Content-Type": "application/json" };

export const headers = (token) => {
  let headers = { ...defaultHeaders };
  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }
  if (defaltEnvironmentId) {
    headers["x-environment"] = defaltEnvironmentId;
  }
  return headers;
};
