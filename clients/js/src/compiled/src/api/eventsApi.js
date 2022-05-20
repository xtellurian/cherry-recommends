import { executeFetch } from "./client/apiClient";
export const fetchEventAsync = async ({ id, token }) => {
    return await executeFetch({
        token,
        path: `api/events/${id}`,
    });
};
export const eventKinds = {
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
export const createEventsAsync = async ({ apiKey, token, events, }) => {
    return await executeFetch({
        path: "api/events",
        method: "post",
        token,
        apiKey,
        body: events,
    });
};
export const fetchCustomersEventsAsync = async ({ token, id, page, pageSize, useInternalId, }) => {
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
// useful extension methods to create certain event kinds
export const createRecommendationConsumedEventAsync = async ({ token, commonUserId, customerId, correlatorId, properties, }) => {
    const payload = {
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
export const fetchBusinessEventsAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/events`,
        token,
    });
};
