export function fetchTrackedUsersAsync({ token, page, searchTerm }: {
    token: any;
    page: any;
    searchTerm: any;
}): Promise<any>;
export function updateMergePropertiesAsync({ token, id, properties }: {
    token: any;
    id: any;
    properties: any;
}): Promise<any>;
export function fetchTrackedUserAsync({ token, id, useInternalId }: {
    token: any;
    id: any;
    useInternalId: any;
}): Promise<any>;
export function fetchUniqueTrackedUserActionGroupsAsync({ token, id, }: {
    token: any;
    id: any;
}): Promise<any>;
export function fetchLatestRecommendationsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function fetchTrackedUserActionAsync({ token, id, category, actionName, }: {
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
export function createOrUpdateTrackedUserAsync({ token, user }: {
    token: any;
    user: any;
}): Promise<any>;
export function fetchTrackedUsersActionsAsync({ token, page, id, revenueOnly, }: {
    token: any;
    page: any;
    id: any;
    revenueOnly: any;
}): Promise<any>;
