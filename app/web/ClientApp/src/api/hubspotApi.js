const defaultHeaders = { "Content-Type": "application/json" };
const headers = (token) =>
  !token
    ? defaultHeaders
    : { ...defaultHeaders, Authorization: `Bearer ${token}` };

export const fetchHubspotAppInformation = async ({ success, error, token }) => {
  const url = "api/hubspotappinfo";

  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(await response.json());
  }
};

export const fetchHubspotWebhookBehaviourAsync = async ({ token, id }) => {
  const url = `api/integratedsystems/${id}/hubspot/WebhookBehaviour`;
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const setHubspotWebhookBehaviourAsync = async ({
  token,
  id,
  behaviour,
}) => {
  const url = `api/integratedsystems/${id}/hubspot/WebhookBehaviour`;
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(behaviour),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotCrmCardBehaviourAsync = async ({ token, id }) => {
  const url = `api/integratedsystems/${id}/hubspot/CrmCardBehaviour`;
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const setHubspotCrmCardBehaviourAsync = async ({
  token,
  id,
  behaviour,
}) => {
  const url = `api/integratedsystems/${id}/hubspot/CrmCardBehaviour`;
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(behaviour),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotClientAllContactProperties = async ({
  success,
  error,
  token,
  id,
}) => {
  const url = `api/integratedsystems/${id}/hubspot/contactproperties`;

  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};

export const fetchHubspotClientContactEventsAsync = async ({ token, id }) => {
  const url = `api/integratedsystems/${id}/hubspot/contact-events`;

  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotAccount = async ({ success, error, token, id }) => {
  const url = `api/integratedsystems/${id}/hubspot/account`;

  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};

export const fetchHubspotContacts = async ({ success, error, token, id }) => {
  const url = `api/integratedsystems/${id}/hubspot/contacts`;

  const response = await fetch(url, {
    headers: headers(token),
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
  const url = `/api/integratedsystems/${integratedSystemId}/hubspot/HubspotCode`;
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify({ code, redirectUri }),
  });

  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};
