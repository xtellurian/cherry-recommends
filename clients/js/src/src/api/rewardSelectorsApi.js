import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
import { headers } from "./headers";

export const fetchRewardSelectorsAsync = async ({ token, page }) => {
  const url = getUrl("api/RewardSelectors");
  let path = `${url}?${pageQuery(page)}`;

  const response = await fetch(path, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchRewardSelectorAsync = async ({ token, page, id }) => {
  const url = getUrl(`api/RewardSelectors/${id}`);
  let path = `${url}?${pageQuery(page)}`;

  const response = await fetch(path, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const deleteRewardSelectorAsync = async ({ token, page, id }) => {
  const url = getUrl(`api/RewardSelectors/${id}`);
  let path = `${url}?${pageQuery(page)}`;

  const response = await fetch(path, {
    headers: headers(token),
    method: "delete",
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const createRewardSelectorAsync = async ({ token, entity }) => {
  const url = getUrl("api/RewardSelectors");

  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(entity),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
