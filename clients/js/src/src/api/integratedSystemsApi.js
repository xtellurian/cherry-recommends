import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
import { headers } from "./headers";

export const fetchIntegratedSystemsAsync = async ({ token, page }) => {
  const url = getUrl("api/integratedSystems");
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
export const fetchIntegratedSystems = async ({
  success,
  error,
  token,
  page,
}) => {
  fetchIntegratedSystemsAsync({ token, page }).then(success).error(error);
};

export const fetchIntegratedSystemAsync = async ({ token, id }) => {
  const url = getUrl(`api/integratedSystems/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
export const fetchIntegratedSystem = async ({ success, error, token, id }) => {
  fetchIntegratedSystemAsync({ id, token }).then(success).catch(error);
};

export const renameAsync = async ({ token, id, name }) => {
  const url = getUrl(`api/integratedSystems/${id}/name?name=${name}`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const createIntegratedSystemAsync = async ({ token, payload }) => {
  const url = getUrl("api/integratedSystems");
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(payload),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const deleteIntegratedSystemAsync = async ({ token, id }) => {
  const url = getUrl(`api/integratedSystems/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "delete",
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchWebhookReceivers = async ({ success, error, token, id }) => {
  const url = getUrl(`api/integratedSystems/${id}/webhookreceivers`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    const data = await response.json();
    success(data);
  } else {
    error(await response.json());
  }
};

export const createIntegratedSystem = ({ success, error, token, payload }) => {
  createIntegratedSystemAsync({ token, payload }).then(success).catch(error);
};

export const createWebhookReceiver = async ({
  success,
  error,
  token,
  id,
  useSharedSecret,
}) => {
  const url = getUrl(`api/integratedSystems/${id}/webhookreceivers`);
  const response = await fetch(`${url}?useSharedSecret=${useSharedSecret}`, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify({}), // body is just empty for this
  });
  if (response.ok) {
    const data = await response.json();
    success(data);
  } else {
    error(await response.json());
  }
};
