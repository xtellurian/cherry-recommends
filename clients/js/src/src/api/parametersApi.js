import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
import { headers } from "./headers";

export const fetchParameters = async ({ success, error, token, page }) => {
  const url = getUrl("api/parameters");
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: headers(token),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};

export const fetchParameter = async ({ success, error, token, id }) => {
  const url = getUrl(`api/parameters/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};

export const deleteParameterAsync = async ({ token, id }) => {
  const url = getUrl(`api/parameters/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "delete",
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const createParameterAsync = async ({ token, payload }) => {
  const url = getUrl("api/parameters");
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

export const createParameter = ({ success, error, token, payload }) => {
  createParameterAsync({ token, payload }).then(success).catch(error);
};
