import { pageQuery } from "./paging";
const defaultHeaders = { "Content-Type": "application/json" };

export const fetchSegments = async ({ success, error, token, page }) => {
  const response = await fetch(`api/segments?${pageQuery(page)}`, {
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
  const response = await fetch(`api/segments/${id}`, {
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
  const response = await fetch("api/segments", {
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
