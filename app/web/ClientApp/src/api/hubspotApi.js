const defaultHeaders = { "Content-Type": "application/json" };

export const fetchHubspotAppInformation = async ({ success, error, token }) => {
  const url = "api/hubspotintegratedsystems";

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

export const fetchHubspotClientAllContactProperties = async ({
  success,
  error,
  token,
  id,
}) => {
  const url = `api/hubspotintegratedsystems/${id}/contactproperties`;

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

export const fetchHubspotClientContactEventsAsync = async ({ token, id }) => {
  const url = `api/hubspotintegratedsystems/${id}/contact-events`;

  const response = await fetch(url, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotAccount = async ({ success, error, token, id }) => {
  const url = `api/hubspotintegratedsystems/${id}/account`;

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

export const fetchHubspotContacts = async ({ success, error, token, id }) => {
  const url = `api/hubspotintegratedsystems/${id}/contacts`;

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

export const saveHubspotCode = async ({
  success,
  error,
  token,
  integratedSystemId,
  code,
  redirectUri,
}) => {
  const url = `/api/hubspotintegratedsystems/${integratedSystemId}/HubspotCode`;
  const response = await fetch(url, {
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    method: "post",
    body: JSON.stringify({ code, redirectUri }),
  });

  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};
