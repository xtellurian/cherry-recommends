const defaultHeaders = { "Content-Type": "application/json" };
const basePath = "api/integratedSystems";

export const fetchIntegratedSystems = async ({ success, error, token }) => {
  const response = await fetch(basePath, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(response.statusText);
  }
};

export const createIntegratedSystem = async ({
  success,
  error,
  token,
  payload,
}) => {
  const response = await fetch(basePath, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "post",
    body: JSON.stringify(payload),
  });
  if (response.ok) {
    const data = await response.json();
    success(data);
  } else {
    error(response.statusText);
  }
};
