import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";

const defaultHeaders = { "Content-Type": "application/json" };

export const fetchIntegratedSystems = async ({
  success,
  error,
  token,
  page,
}) => {
  const url = getUrl("api/integratedSystems");
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
};

export const fetchIntegratedSystem = async ({ success, error, token, id }) => {
  const url = getUrl(`api/integratedSystems/${id}`);
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

export const createIntegratedSystem = async ({
  success,
  error,
  token,
  payload,
}) => {
  const url = getUrl("api/integratedSystems");
  const response = await fetch(url, {
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
    error(await response.json());
  }
};

export const fetchWebhookReceivers = async ({
  success,
  error,
  token,
  page,
  id,
}) => {
  const url = getUrl(`api/integratedSystems/${id}/webhookreceivers`);
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const data = await response.json();
    success(data);
  } else {
    error(await response.json());
  }
};

export const createWebhookReceiver = async ({
  success,
  error,
  token,
  id,
  useSharedSecret
}) => {
  const url = getUrl(`api/integratedSystems/${id}/webhookreceivers`);
  const response = await fetch(`${url}?useSharedSecret=${useSharedSecret}`, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
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
