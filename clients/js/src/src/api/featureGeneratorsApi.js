import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
import { headers } from "./headers";

export const fetchFeatureGeneratorsAsync = async ({ page, token }) => {
  const url = getUrl(`api/FeatureGenerators?${pageQuery(page)}`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const createFeatureGeneratorAsync = async ({ token, payload }) => {
  const url = getUrl(`api/FeatureGenerators`);
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

export const deleteFeatureGeneratorAsync = async ({ token, id }) => {
  const url = getUrl(`api/FeatureGenerators/${id}`);
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
