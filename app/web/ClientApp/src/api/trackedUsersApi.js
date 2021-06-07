import { pageQuery } from "./paging";
import { chunkArray } from "../utility/chunk";
const MAX_ARRAY = 5000;
const defaultHeaders = { "Content-Type": "application/json" };
const baseUrl = "api/trackedUsers";

export const fetchTrackedUsers = async ({ success, error, token, page }) => {
  const response = await fetch(`${baseUrl}?${pageQuery(page)}`, {
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
  const response = await fetch(`${baseUrl}/${id}`, {
    headers: !token ? {} : { Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const trackedUser = await response.json();
    success(trackedUser);
  } else {
    error(await response.json());
  }
};

// todo
export const fetchTrackedUserEvents = async ({ success, error, token }) => {
  const response = await fetch("api/trackedUsers/events", {
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

export const uploadUserData = async ({ success, error, token, payload }) => {
  const payloads = chunkArray(payload.users, MAX_ARRAY).map((users) => ({
    users,
  }));
  const responses = [];
  for (const p of payloads) {
    const response = await fetch("api/trackedUsers", {
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

export const createSingleUser = async ({ success, error, token, user }) => {
  const response = await fetch("api/trackedUsers", {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "post",
    body: JSON.stringify(user),
  });
  if (response.ok) {
    var data = await response.json();
    success(data);
  } else {
    error(await response.json());
  }
};
