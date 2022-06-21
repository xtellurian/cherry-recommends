const defaultHeaders = { "Content-Type": "application/json" };
const headers = (token, tenant, environment) => {
  let _headers = defaultHeaders;
  if (token) {
    _headers = { ..._headers, Authorization: `Bearer ${token}` };
  }
  if (tenant) {
    _headers = { ..._headers, "x-tenant": tenant };
  }
  if (environment) {
    _headers = { ..._headers, "x-environment": environment };
  }
  return _headers;
};

export const fetchHubspotAppInformationAsync = async ({ token, tenant }) => {
  const url = "api/hubspotappinfo";
  const response = await fetch(url, {
    headers: headers(token, tenant),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotWebhookBehaviourAsync = async ({
  token,
  tenant,
  id,
}) => {
  const url = `api/integratedsystems/${id}/hubspot/WebhookBehaviour`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const setHubspotWebhookBehaviourAsync = async ({
  token,
  tenant,
  id,
  behaviour,
}) => {
  const url = `api/integratedsystems/${id}/hubspot/WebhookBehaviour`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
    method: "post",
    body: JSON.stringify(behaviour),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotCrmCardBehaviourAsync = async ({
  token,
  tenant,
  id,
}) => {
  const url = `api/integratedsystems/${id}/hubspot/CrmCardBehaviour`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const setHubspotCrmCardBehaviourAsync = async ({
  token,
  tenant,
  id,
  behaviour,
}) => {
  const url = `api/integratedsystems/${id}/hubspot/CrmCardBehaviour`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
    method: "post",
    body: JSON.stringify(behaviour),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotPushBehaviourAsync = async ({ token, tenant, id }) => {
  const url = `api/IntegratedSystems/${id}/Hubspot/PushBehaviour`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const setHubspotPushBehaviourAsync = async ({
  token,
  tenant,
  id,
  behaviour,
}) => {
  const url = `api/IntegratedSystems/${id}/Hubspot/PushBehaviour`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
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
  tenant,
  id,
}) => {
  const url = `api/integratedsystems/${id}/Hubspot/ContactProperties`;

  const response = await fetch(url, {
    headers: headers(token, tenant),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotClientContactEventsAsync = async ({
  token,
  tenant,
  id,
}) => {
  const url = `api/integratedsystems/${id}/Hubspot/contact-events`;

  const response = await fetch(url, {
    headers: headers(token, tenant),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotAccountAsync = async ({ token, tenant, id }) => {
  const url = `api/integratedsystems/${id}/hubspot/account`;

  const response = await fetch(url, {
    headers: headers(token, tenant),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchHubspotContactsAsync = async ({ token, tenant, id }) => {
  const url = `api/integratedsystems/${id}/hubspot/contacts`;

  const response = await fetch(url, {
    headers: headers(token, tenant),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const saveHubspotCodeAsync = async ({
  token,
  tenant,
  integratedSystemId,
  code,
  redirectUri,
}) => {
  const url = `/api/integratedsystems/${integratedSystemId}/hubspot/HubspotCode`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
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
  tenant,
  id,
  behaviour,
}) => {
  const url = `api/integratedsystems/${id}/hubspot/ConnectedContactProperties`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
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
  tenant,
  id,
}) => {
  const url = `api/integratedsystems/${id}/hubspot/ConnectedContactProperties`;
  const response = await fetch(url, {
    headers: headers(token, tenant),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
