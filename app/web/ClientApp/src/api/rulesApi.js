const defaultHeaders = { "Content-Type": "application/json" };

export const fetchRules = async ({ success, error, token, segmentId }) => {
  let path = "api/rules";
  if (segmentId) {
    path = `${path}?segmentId=${segmentId}`;
  }
  const response = await fetch(path, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(response.statusText);
  }
};

export const fetchRule = async ({ success, error, token, ruleId }) => {
  const response = await fetch(`api/rules/${ruleId}`, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(response.status);
  }
};

export const createRule = async ({ success, error, token, payload }) => {
  const response = await fetch("api/rules", {
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
    error(response);
  }
};
