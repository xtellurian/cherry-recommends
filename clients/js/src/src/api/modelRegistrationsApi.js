import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
import { headers } from "./headers";

export const fetchModelRegistrations = async ({
  success,
  error,
  token,
  page,
}) => {
  const url = getUrl("api/ModelRegistrations");
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: headers(token),
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(await response.json());
  }
};

export const fetchModelRegistration = async ({ success, error, token, id }) => {
  const url = getUrl(`api/ModelRegistrations/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(await response.json());
  }
};

export const deleteModelRegistration = async ({
  success,
  error,
  token,
  id,
}) => {
  const url = getUrl(`api/ModelRegistrations/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "delete",
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(await response.json());
  }
};

export const createModelRegistrationAsync = async ({ token, payload }) => {
  const url = getUrl("api/ModelRegistrations");
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(payload),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const createModelRegistration = ({ success, error, token, payload }) => {
  createModelRegistrationAsync({ token, payload }).then(success).catch(error);
};

export const invokeModel = async ({
  success,
  error,
  token,
  modelId,
  features,
}) => {
  const url = getUrl(`api/ModelRegistrations/${modelId}/invoke`);
  const response = await fetch(url, {
    headers: headers(token),
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
