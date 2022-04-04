import {
  AuthenticatedRequest,
  CustomerEvent,
  EntityRequest,
} from "../interfaces";
import { executeFetch } from "./client/apiClient";

export const Custom = "Custom";
export const Behaviour = "Behaviour";
export const ConsumeRecommendation = "ConsumeRecommendation";

export const fetchEventAsync = async ({ id, token }: EntityRequest) => {
  return await executeFetch({
    token,
    path: `api/events/${id}`,
  });
};

interface CreateEventRequest {
  apiKey?: string | undefined;
  token?: string | undefined;
  events: CustomerEvent[];
}
export const createEventsAsync = async ({
  apiKey,
  token,
  events,
}: CreateEventRequest) => {
  return await executeFetch({
    path: "api/events",
    method: "post",
    token,
    apiKey,
    body: events,
  });
};

export const fetchCustomersEventsAsync = async ({
  token,
  id,
  useInternalId,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/Customers/${id}/events`,
    token,
    query: {
      useInternalId,
    },
  });
};
export const fetchTrackedUsersEventsAsync = fetchCustomersEventsAsync;

interface CreateRecommendationConsumedRequest {
  token: string;
  commonUserId: string | undefined;
  customerId: string;
  correlatorId: number;
}
// useful extension methods to create certain event kinds
export const createRecommendationConsumedEventAsync = async ({
  token,
  commonUserId,
  customerId,
  correlatorId,
}: CreateRecommendationConsumedRequest) => {
  const payload: CustomerEvent = {
    commonUserId,
    customerId,
    eventId: `recommendation-${correlatorId}-${new Date().getTime()}`,
    recommendationCorrelatorId: correlatorId,
    kind: ConsumeRecommendation,
    eventType: "generated",
  };
  return await createEventsAsync({ token, events: [payload] });
};

export const fetchBusinessEventsAsync = async ({
  token,
  id,  
}: EntityRequest) => {
  return await executeFetch({
    path: `api/Businesses/${id}/events`,
    token,
  });
};