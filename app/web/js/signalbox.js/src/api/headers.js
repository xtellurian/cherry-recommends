export const defaultHeaders = { "Content-Type": "application/json" };

export const headers = (token) =>
  !token
    ? defaultHeaders
    : { ...defaultHeaders, Authorization: `Bearer ${token}` };
