const defaultHeaders = { "Content-Type": "application/json" };

export const fetchExperiments = async ({ success, error, token }) => {
  const response = await fetch("api/experiments", {
    headers: !token ? {} : { Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(response.statusText);
  }
};

export const createExperiment = async ({ success, error, token, payload }) => {
  const response = await fetch("api/experiments", {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "post",
    body: JSON.stringify(payload),
  });
  if (response.ok) {
    const data = await response.json();
    success(data);
  } else {
    error(response.statusText);
  }
};

export const fetchExperimentResults = async ({
  success,
  error,
  token,
  id,
}) => {
  const response = await fetch(`api/experiments/${id}/results`, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const data = await response.json();
    success(data);
  } else {
    error(response.statusText);
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
  const response = await fetch(
    `api/experiments/${experimentId}/recommendation`,
    {
      headers: !token
        ? defaultHeaders
        : { ...defaultHeaders, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify({ commonUserId: userId, features }),
    }
  );
  if (response.ok) {
    const data = await response.json();
    success(data);
  } else {
    error(response.statusText);
  }
};
