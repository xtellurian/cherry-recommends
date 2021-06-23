import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";

const defaultHeaders = { "Content-Type": "application/json" };

export const fetchParameters = async ({ success, error, token, page }) => {
  const url = getUrl("api/parameters");
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
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
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};

export const createParameter = async ({ success, error, token, payload }) => {
  const url = getUrl("api/parameters");
  const response = await fetch(url, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "post",
    body: JSON.stringify(payload),
  });

  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};
