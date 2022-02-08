export function fetchCustomersAsync({ token, page, searchTerm }: {
    token: any;
    page: any;
    searchTerm: any;
}): Promise<any>;
export function updateMergePropertiesAsync({ token, id, properties }: {
    token: any;
    id: any;
    properties: any;
}): Promise<any>;
export function fetchCustomerAsync({ token, id, useInternalId }: {
    token: any;
    id: any;
    useInternalId: any;
}): Promise<any>;
export function fetchUniqueCustomerActionGroupsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function fetchLatestRecommendationsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function fetchCustomerActionAsync({ token, id, category, actionName, }: {
    token: any;
    id: any;
    category: any;
    actionName: any;
}): Promise<any>;
export function uploadUserDataAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any[] | {
    error: any;
} | undefined>;
export function createOrUpdateCustomerAsync({ token, customer, user, }: {
    token: any;
    customer: any;
    user: any;
}): Promise<any>;
export function fetchCustomersActionsAsync({ token, page, id, revenueOnly, }: {
    token: any;
    page: any;
    id: any;
    revenueOnly: any;
}): Promise<any>;
export function setCustomerMetricAsync({ token, id, metricId, useInternalId, value, }: {
    token: any;
    id: any;
    metricId: any;
    useInternalId: any;
    value: any;
}): Promise<any>;
