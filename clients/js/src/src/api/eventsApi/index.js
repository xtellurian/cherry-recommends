import { getUrl } from "../../baseUrl";
import { headers } from "../headers";
import { internalId } from "../../utilities/index";
import * as kinds from "./eventKinds";

export const fetchEventAsync = async ({ id, token }) => {
  const url = getUrl(`api/events/${id}`);

  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const createEventsAsync = async ({ token, events }) => {
  const url = getUrl(`api/events`);

  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(events),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchTrackedUsersEventsAsync = async ({
  token,
  id,
  useInternalId,
}) => {
  let url = getUrl(`api/TrackedUsers/${id}/events`);
  url = `${url}?${internalId(useInternalId)}`;
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
export const fetchUserEvents = async ({
  success,
  error,
  token,
  commonUserId,
}) => {
  fetchTrackedUsersEventsAsync({ id: commonUserId, token })
    .then(success)
    .catch(error);
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

// useful extension methods to create certain event kinds
export const createRecommendationConsumedEventAsync = async ({
  token,
  commonUserId,
  correlatorId,
}) => {
  const payload = {
    commonUserId,
    eventId: `recommendation-${correlatorId}-${new Date().getTime()}`,
    recommendationCorrelatorId: correlatorId,
    kind: kinds.ConsumeRecommendation,
    eventType: "generated",
  };
  return await createEventsAsync({ token, events: [payload] });
};
