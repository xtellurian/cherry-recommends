import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
import { headers } from "./headers";
import { searchEntities } from "../utilities/search";

export const fetchFeaturesAsync = async ({ token, page, searchTerm }) => {
  let url = getUrl("api/features");
  url = `${url}?${pageQuery(page)}`;
  if (searchTerm) {
    url = `${url}&${searchEntities(searchTerm)}`;
  }
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchFeatureAsync = async ({ token, id }) => {
  const url = getUrl(`api/features/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchFeatureTrackedUsersAsync = async ({ token, page, id }) => {
  const url = getUrl(`api/Features/${id}/TrackedUsers?${pageQuery(page)}`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const createFeatureAsync = async ({ token, feature }) => {
  const url = getUrl(`api/features/`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(feature),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const deleteFeatureAsync = async ({ token, id }) => {
  const url = getUrl(`api/features/${id}`);
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

export const fetchTrackedUserFeaturesAsync = async ({ token, id }) => {
  const url = getUrl(`api/TrackedUsers/${id}/features`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchTrackedUserFeatureValuesAsync = async ({
  token,
  id,
  feature,
  version,
}) => {
  const url = getUrl(
    `api/TrackedUsers/${id}/features/${feature}?version=${version || ""}`
  );
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
