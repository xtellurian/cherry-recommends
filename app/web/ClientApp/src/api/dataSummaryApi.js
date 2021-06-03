const defaultHeaders = { "Content-Type": "application/json" };

export const fetchEventSummary = async ({ success, error, token }) => {
  let path = `api/datasummary/events`;

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

export const fetchEventTimeline = async ({
  success,
  error,
  token,
  kind,
  eventType,
}) => {
  let path = `api/datasummary/events/timeline/${kind}/${eventType}`;

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

export const fetchDashboard = async ({ success, error, token }) => {
  let path = `api/datasummary/dashboard`;

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
