import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";

const defaultHeaders = { "Content-Type": "application/json" };

export const fetchTouchpoints = async ({
  success,
  error,
  token,
  page,
  onFinally,
}) => {
  try {
    const url = getUrl("api/touchpoints");
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
  } finally {
    if (onFinally) {
      onFinally();
    }
  }
};

export const fetchTouchpoint = async ({ success, error, token, id }) => {
  const url = getUrl(`api/touchpoints/${id}`);
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

export const createTouchpointMetadata = async ({
  success,
  error,
  token,
  payload,
  onFinally,
}) => {
  try {
    const url = getUrl("api/touchpoints");
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
  } finally {
    if (onFinally) {
      onFinally();
    }
  }
};

export const fetchTrackedUserTouchpoints = async ({
  success,
  error,
  token,
  id,
}) => {
  if (!id) {
    error({
      title: "Tracked User ID is required.",
    });
    return;
  }
  const url = getUrl(`api/trackedusers/${id}/touchpoints`);
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

export const fetchTrackedUsersInTouchpoint = async ({
  success,
  error,
  token,
  id,
}) => {
  if (!id) {
    error({
      title: "Touchpoint ID is required.",
    });
    return;
  }
  const url = getUrl(`api/touchpoints/${id}/trackedusers`);
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

export const createTrackedUserTouchpoint = async ({
  success,
  error,
  token,
  id,
  touchpointCommonId,
  payload,
}) => {
  if (!id || !touchpointCommonId) {
    error({
      title: "Tracked User ID and touchpoint ID are required.",
    });
    return;
  }
  const url = getUrl(
    `api/trackedusers/${id}/touchpoints/${touchpointCommonId}`
  );
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

export const fetchTrackedUserTouchpointValues = async ({
  success,
  error,
  token,
  id,
  touchpointCommonId,
  version,
}) => {
  if (!id || !touchpointCommonId) {
    error({
      title: "Tracked User ID and touchpoint ID are required.",
    });
    return;
  }

  const url = getUrl(
    `api/trackedusers/${id}/touchpoints/${touchpointCommonId}`
  );
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