import { getUrl } from "../baseUrl";
import { headers } from "./headers";

export const fetchEventSummary = async ({ success, error, token }) => {
  const url = getUrl("api/datasummary/events");
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

export const fetchEventTimeline = async ({
  success,
  error,
  token,
  kind,
  eventType,
}) => {
  const url = getUrl(`api/datasummary/events/timeline/${kind}/${eventType}`);

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

export const fetchDashboard = async ({ success, error, token, scope }) => {
  const url = getUrl(`api/datasummary/dashboard`);

  const response = await fetch(`${url}?scope=${scope}`, {
    headers: headers(token),
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(await response.json());
  }
};
