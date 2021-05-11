const defaultHeaders = { "Content-Type": "application/json" };

export const fetchTrackedUsers = async ({ success, error, token }) => {
  const response = await fetch("api/trackedUsers", {
    headers: !token ? {} : { Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const trackedUsers = await response.json();
    success(trackedUsers);
  } else {
    error(response.statusText);
  }
};

export const fetchTrackedUserEvents = async ({ success, error, token }) => {
  const response = await fetch("api/trackedUsers/events", {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(response.statusText);
  }
};

export const uploadUserData = async ({ success, error, token, payload }) => {
  const response = await fetch("api/trackedUsers/batch", {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "post",
    body: JSON.stringify(payload),
  });
  var data = await response.json();
  if (response.ok) {
    success(data);
  } else {
    error(data);
  }
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
    error(response.statusText);
  }
};

export const fetchSelectedTrackedUsers = async ({
  success,
  error,
  token,
  ids,
}) => {
  if (ids.length === 0) {
    success([]);
  }
  const response = await fetch("api/trackedUsers/query", {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "post",
    body: JSON.stringify({ externalIds: ids }),
  });
  if (response.ok) {
    var data = await response.json();
    success(data);
  } else {
    error(response);
  }
};
