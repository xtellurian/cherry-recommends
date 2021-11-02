import { executeFetch } from "../client/apiClient";
import * as kinds from "./eventKinds";

export const fetchEventAsync = async ({ id, token }) => {
  return await executeFetch({
    token,
    path: `api/events/${id}`,
  });
};

export const createEventsAsync = async ({ apiKey, token, events }) => {
  return await executeFetch({
    path: "api/events",
    method: "post",
    token,
    apiKey,
    body: events,
  });
};

export const fetchTrackedUsersEventsAsync = async ({
  token,
  id,
  useInternalId,
}) => {
  return await executeFetch({
    path: `api/TrackedUsers/${id}/events`,
    token,
    query: {
      useInternalId,
    },
  });
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
