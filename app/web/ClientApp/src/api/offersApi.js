import { pageQuery } from "./paging";
const defaultHeaders = { "Content-Type": "application/json" };

export const fetchOffers = async ({ success, error, token, page }) => {
  let path = `api/offers?${pageQuery(page)}`;
  const response = await fetch(path, {
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

export const fetchOffer = async ({ success, error, token, id }) => {
  const response = await fetch(`api/offers/${id}`, {
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

export const createOffer = async ({ success, error, token, payload }) => {
  const response = await fetch("api/offers", {
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
