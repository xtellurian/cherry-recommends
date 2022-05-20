import { EntityRequest, PaginatedEntityRequest } from "../interfaces";
import { executeFetch } from "./client/apiClient";

import { components } from "../model/api";

export const fetchEventAsync = async ({ id, token }: EntityRequest) => {
  return await executeFetch({
    token,
    path: `api/events/${id}`,
  });
};

export type EventKinds = components["schemas"]["EventKinds"];
interface EventKindConstants {
  custom: EventKinds;
  propertyUpdate: EventKinds;
  behaviour: EventKinds;
  pageView: EventKinds;
  identify: EventKinds;
  consumeRecommendation: EventKinds;
  addToBusiness: EventKinds;
  purchase: EventKinds;
  usePromotion: EventKinds;
}
export const eventKinds: EventKindConstants = {
  custom: "custom",
  propertyUpdate: "propertyUpdate",
  behaviour: "behaviour",
  pageView: "pageView",
  identify: "identify",
  consumeRecommendation: "consumeRecommendation",
  addToBusiness: "addToBusiness",
  purchase: "purchase",
  usePromotion: "usePromotion",
};
type EventDto = components["schemas"]["EventDto"];
type EventLoggingResponse = components["schemas"]["EventLoggingResponse"];
interface CreateEventRequest {
  apiKey?: string | undefined;
  token?: string | undefined;
  events: EventDto[];
}
export const createEventsAsync = async ({
  apiKey,
  token,
  events,
}: CreateEventRequest): Promise<EventLoggingResponse> => {
  return await executeFetch({
    path: "api/events",
    method: "post",
    token,
    apiKey,
    body: events,
  });
};

type CustomerEventPaginated = components["schemas"]["CustomerEventPaginated"];
export const fetchCustomersEventsAsync = async ({
  token,
  id,
  page,
  pageSize,
  useInternalId,
}: PaginatedEntityRequest): Promise<CustomerEventPaginated> => {
  return await executeFetch({
    path: `api/Customers/${id}/events`,
    token,
    page,
    pageSize,
    query: {
      useInternalId,
    },
  });
};
export const fetchTrackedUsersEventsAsync = fetchCustomersEventsAsync;

interface CreateRecommendationConsumedRequest {
  token: string;
  commonUserId?: string | undefined;
  customerId: string;
  correlatorId: number;
  properties:
    | {
        [key: string]: unknown;
      }
    | null
    | undefined;
}
// useful extension methods to create certain event kinds
export const createRecommendationConsumedEventAsync = async ({
  token,
  commonUserId,
  customerId,
  correlatorId,
  properties,
}: CreateRecommendationConsumedRequest) => {
  const payload: EventDto = {
    commonUserId,
    customerId,
    eventId: `recommendation-${correlatorId}-${new Date().getTime()}`,
    recommendationCorrelatorId: correlatorId,
    kind: "consumeRecommendation",
    eventType: "generated",
    properties,
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
