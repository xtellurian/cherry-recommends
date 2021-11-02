const defaultHeaders = { "Content-Type": "application/json" };
const headers = (token) =>
  !token
    ? defaultHeaders
    : { ...defaultHeaders, Authorization: `Bearer ${token}` };

export const fetchHubspotAppInformationAsync = async ({ token }) => {
  const url = "api/hubspotappinfo";
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
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

export const fetchHubspotPushBehaviourAsync = async ({ token, id }) => {
  const url = `api/IntegratedSystems/${id}/Hubspot/PushBehaviour`;
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const setHubspotPushBehaviourAsync = async ({
  token,
  id,
  behaviour,
}) => {
  const url = `api/IntegratedSystems/${id}/Hubspot/PushBehaviour`;
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

export const fetchHubspotClientAllContactPropertiesAsync = async ({
  token,
  id,
}) => {
  const url = `api/integratedsystems/${id}/Hubspot/ContactProperties`;

  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotClientContactEventsAsync = async ({ token, id }) => {
  const url = `api/integratedsystems/${id}/Hubspot/contact-events`;

  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotAccountAsync = async ({ token, id }) => {
  const url = `api/integratedsystems/${id}/hubspot/account`;

  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotContactsAsync = async ({ token, id }) => {
  const url = `api/integratedsystems/${id}/hubspot/contacts`;

  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const saveHubspotCodeAsync = async ({
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
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const setHubspotConnectedContactPropertiesAsync = async ({
  token,
  id,
  behaviour,
}) => {
  const url = `api/integratedsystems/${id}/hubspot/ConnectedContactProperties`;
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

export const fetchHubspotConnectedContactPropertiesAsync = async ({
  token,
  id,
}) => {
  const url = `api/integratedsystems/${id}/hubspot/ConnectedContactProperties`;
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
