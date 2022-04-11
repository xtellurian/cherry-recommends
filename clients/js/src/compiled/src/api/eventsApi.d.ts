import { EntityRequest, PaginatedEntityRequest } from "../interfaces";
import { components } from "../model/api";
export declare const fetchEventAsync: ({ id, token }: EntityRequest) => Promise<any>;
declare type EventDto = components["schemas"]["EventDto"];
declare type EventLoggingResponse = components["schemas"]["EventLoggingResponse"];
interface CreateEventRequest {
    apiKey?: string | undefined;
    token?: string | undefined;
    events: EventDto[];
}
export declare const createEventsAsync: ({ apiKey, token, events, }: CreateEventRequest) => Promise<EventLoggingResponse>;
declare type CustomerEventPaginated = components["schemas"]["CustomerEventPaginated"];
export declare const fetchCustomersEventsAsync: ({ token, id, page, pageSize, useInternalId, }: PaginatedEntityRequest) => Promise<CustomerEventPaginated>;
export declare const fetchTrackedUsersEventsAsync: ({ token, id, page, pageSize, useInternalId, }: PaginatedEntityRequest) => Promise<CustomerEventPaginated>;
interface CreateRecommendationConsumedRequest {
    token: string;
    commonUserId?: string | undefined;
    customerId: string;
    correlatorId: number;
    properties: {
        [key: string]: unknown;
    } | null | undefined;
}
export declare const createRecommendationConsumedEventAsync: ({ token, commonUserId, customerId, correlatorId, properties, }: CreateRecommendationConsumedRequest) => Promise<{
    eventsProcessed?: number | undefined;
    eventsEnqueued?: number | undefined;
}>;
export declare const fetchBusinessEventsAsync: ({ token, id, }: EntityRequest) => Promise<any>;
export {};
