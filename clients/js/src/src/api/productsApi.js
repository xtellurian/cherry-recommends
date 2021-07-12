import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";

const defaultHeaders = { "Content-Type": "application/json" };

export const fetchProducts = async ({ success, error, token, page }) => {
  const url = getUrl("api/products");
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

export const fetchProduct = async ({ success, error, token, id }) => {
  const url = getUrl(`api/products/${id}`);
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

export const createProduct = async ({ success, error, token, product }) => {
  const url = getUrl(`api/products/`);
  const response = await fetch(url, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "post",
    body: JSON.stringify(product),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};

export const deleteProduct = async ({ success, error, token, id }) => {
  const url = getUrl(`api/products/${id}`);
  const response = await fetch(url, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "delete",
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};
