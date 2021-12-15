export function fetchEventAsync({ id, token }: {
    id: any;
    token: any;
}): Promise<any>;
export function createEventsAsync({ apiKey, token, events }: {
    apiKey: any;
    token: any;
    events: any;
}): Promise<any>;
export function fetchTrackedUsersEventsAsync({ token, id, useInternalId, }: {
    token: any;
    id: any;
    useInternalId: any;
}): Promise<any>;
export function createRecommendationConsumedEventAsync({ token, commonUserId, correlatorId, }: {
    token: any;
    commonUserId: any;
    correlatorId: any;
}): Promise<any>;
