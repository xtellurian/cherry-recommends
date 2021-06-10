import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";

const defaultHeaders = { "Content-Type": "application/json" };

export const fetchSegments = async ({ success, error, token, page }) => {
  const url = getUrl("api/segments");
  const response = await fetch(`${url}?${pageQuery(page)}`, {
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

export const fetchSegment = async ({ success, error, token, id }) => {
  const url = getUrl(`api/segments/${id}`);
  const response = await fetch(url, {
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

export const createSegment = async ({ success, error, token, payload }) => {
  const url = getUrl("api/segments");
  const response = await fetch(url, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "post",
    body: JSON.stringify(payload),
  });

  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(await response.json());
  }
};
