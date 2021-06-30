import { getUrl } from "../baseUrl";
const defaultHeaders = { "Content-Type": "application/json" };

export const fetchUserEvents = async ({
  success,
  error,
  token,
  commonUserId,
}) => {
  const url = getUrl("api/events");
  let path = `${url}?commonUserId=${commonUserId}`;

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

export const logUserEvents = async ({ success, error, token, events }) => {
  const url = getUrl("api/events");
  if (events.some((e) => !e.commonUserId)) {
    error({
      title: "Every Event requires a commonUserId",
    });
    return;
  }
  if (events.some((e) => !e.eventId)) {
    error({
      title: "Every Event requires a unique eventId",
    });
    return;
  }
  if (events.some((e) => !e.eventType)) {
    error({
      title: "Every Event requires an eventType",
    });
    return;
  }
  if (events.some((e) => !e.kind)) {
    error({
      title: "Every Event requires a kind",
    });
    return;
  }

  const response = await fetch(url, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    method: "post",
    body: JSON.stringify(events),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};
