import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
const defaultHeaders = { "Content-Type": "application/json" };

export const fetchExperiments = async ({ success, error, token, page }) => {
  const url = getUrl("api/experiments");
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: !token ? {} : { Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(response.json());
  }
};

export const createExperiment = async ({ success, error, token, payload }) => {
  const url = getUrl("api/experiments");
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

export const fetchExperimentResults = async ({ success, error, token, id }) => {
  const url = getUrl(`api/experiments/${id}/results`);
  const response = await fetch(url, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const data = await response.json();
    success(data);
  } else {
    error(response.json());
  }
};

export const fetchRecommendation = async ({
  success,
  error,
  token,
  experimentId,
  features,
  userId,
}) => {
  const url = getUrl(`api/experiments/${experimentId}/recommendation`);
  const response = await fetch(url, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "post",
    body: JSON.stringify({ commonUserId: userId, features }),
  });
  if (response.ok) {
    const data = await response.json();
    success(data);
  } else {
    error(await response.json());
  }
};
