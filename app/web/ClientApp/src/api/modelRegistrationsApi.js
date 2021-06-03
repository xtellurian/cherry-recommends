import { pageQuery } from "./paging";
const defaultHeaders = { "Content-Type": "application/json" };
const basePath = "api/ModelRegistrations";

export const fetchModelRegistrations = async ({
  success,
  error,
  token,
  page,
}) => {
  const response = await fetch(`${basePath}?${pageQuery(page)}`, {
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

export const fetchModelRegistration = async ({ success, error, token, id }) => {
  const response = await fetch(`${basePath}/${id}`, {
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

export const createModelRegistration = async ({
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
    error(await response.json());
  }
};

export const invokeModel = async ({
  success,
  error,
  token,
  modelId,
  features,
}) => {
  const response = await fetch(`${basePath}/${modelId}/invoke`, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "post",
    body: JSON.stringify(features),
  });
  if (response.ok) {
    const data = await response.json();
    success(data);
  } else {
    error(await response.json());
  }
};
