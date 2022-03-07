import { executeFetch } from "./client/apiClientTs";
export const Custom = "Custom";
export const Behaviour = "Behaviour";
export const ConsumeRecommendation = "ConsumeRecommendation";
export const fetchEventAsync = async ({ id, token }) => {
    return await executeFetch({
        token,
        path: `api/events/${id}`,
    });
};
export const createEventsAsync = async ({ apiKey, token, events, }) => {
    return await executeFetch({
        path: "api/events",
        method: "post",
        token,
        apiKey,
        body: events,
    });
};
export const fetchCustomersEventsAsync = async ({ token, id, useInternalId, }) => {
    return await executeFetch({
        path: `api/Customers/${id}/events`,
        token,
        query: {
            useInternalId,
        },
    });
};
export const fetchTrackedUsersEventsAsync = fetchCustomersEventsAsync;
// useful extension methods to create certain event kinds
export const createRecommendationConsumedEventAsync = async ({ token, commonUserId, customerId, correlatorId, }) => {
    const payload = {
        commonUserId,
        customerId,
        eventId: `recommendation-${correlatorId}-${new Date().getTime()}`,
        recommendationCorrelatorId: correlatorId,
        kind: ConsumeRecommendation,
        eventType: "generated",
    };
    return await createEventsAsync({ token, events: [payload] });
};
export const fetchBusinessEventsAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/events`,
        token,
    });
};
