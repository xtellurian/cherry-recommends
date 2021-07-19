import { getUrl } from "../baseUrl";
import { headers } from "./headers";

export const fetchUserEvents = async ({
  success,
  error,
  token,
  commonUserId,
}) => {
  let url = getUrl("api/events");
  if (commonUserId) {
    url = `${url}?commonUserId=${commonUserId}`;
  }

  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    success(await response.json());
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
    headers: headers(token),
    method: "post",
    body: JSON.stringify(events),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};
