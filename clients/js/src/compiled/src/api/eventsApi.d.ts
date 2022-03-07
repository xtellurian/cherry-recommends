import { CustomerEvent, EntityRequest } from "../interfaces";
export declare const Custom = "Custom";
export declare const Behaviour = "Behaviour";
export declare const ConsumeRecommendation = "ConsumeRecommendation";
export declare const fetchEventAsync: ({ id, token }: EntityRequest) => Promise<any>;
interface CreateEventRequest {
    apiKey?: string | undefined;
    token?: string | undefined;
    events: CustomerEvent[];
}
export declare const createEventsAsync: ({ apiKey, token, events, }: CreateEventRequest) => Promise<any>;
export declare const fetchCustomersEventsAsync: ({ token, id, useInternalId, }: EntityRequest) => Promise<any>;
export declare const fetchTrackedUsersEventsAsync: ({ token, id, useInternalId, }: EntityRequest) => Promise<any>;
interface CreateRecommendationConsumedRequest {
    token: string;
    commonUserId: string | undefined;
    customerId: string;
    correlatorId: number;
}
export declare const createRecommendationConsumedEventAsync: ({ token, commonUserId, customerId, correlatorId, }: CreateRecommendationConsumedRequest) => Promise<any>;
export declare const fetchBusinessEventsAsync: ({ token, id, }: EntityRequest) => Promise<any>;
export {};
