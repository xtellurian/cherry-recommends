import { pageQuery } from "./paging";
import { chunkArray } from "../utilities/chunk";
import { getUrl } from "../baseUrl";
const MAX_ARRAY = 5000;
const defaultHeaders = { "Content-Type": "application/json" };

export const fetchTrackedUsers = async ({ success, error, token, page }) => {
  const url = getUrl("api/trackedUsers");
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: !token ? {} : { Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const trackedUsers = await response.json();
    success(trackedUsers);
  } else {
    error(await response.json());
  }
};

export const fetchTrackedUser = async ({ success, error, token, id }) => {
  const url = getUrl(`api/trackedUsers/${id}`);
  const response = await fetch(url, {
    headers: !token ? {} : { Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const trackedUser = await response.json();
    success(trackedUser);
  } else {
    error(await response.json());
  }
};

export const uploadUserData = async ({ success, error, token, payload }) => {
  const url = getUrl(`api/trackedUsers`);
  const payloads = chunkArray(payload.users, MAX_ARRAY).map((users) => ({
    users,
  }));
  const responses = [];
  for (const p of payloads) {
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders
        : { ...defaultHeaders, Authorization: `Bearer ${token}` },
      method: "put",
      body: JSON.stringify(p),
    });
    if (response.ok) {
      responses.push(await response.json());
    } else {
      error(await response.json());
    }
  }
  success(responses);
};

export const createOrUpdateTrackedUser = async ({
  success,
  error,
  token,
  user,
}) => {
  const url = getUrl(`api/trackedUsers/`);
  const response = await fetch(url, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "post",
    body: JSON.stringify(user),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};
