export function fetchFeaturesAsync({ token, page, searchTerm }: {
    token: any;
    page: any;
    searchTerm: any;
}): Promise<any>;
export function fetchFeatureAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function fetchFeatureTrackedUsersAsync({ token, page, id }: {
    token: any;
    page: any;
    id: any;
}): Promise<any>;
export function fetchFeatureTrackedUserFeaturesAsync({ token, page, id, }: {
    token: any;
    page: any;
    id: any;
}): Promise<any>;
export function createFeatureAsync({ token, feature }: {
    token: any;
    feature: any;
}): Promise<any>;
export function deleteFeatureAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function fetchTrackedUserFeaturesAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function fetchTrackedUserFeatureValuesAsync({ token, id, feature, version, }: {
    token: any;
    id: any;
    feature: any;
    version: any;
}): Promise<any>;
export function fetchDestinationsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function createDestinationAsync({ token, id, destination }: {
    token: any;
    id: any;
    destination: any;
}): Promise<any>;
export function deleteDestinationAsync({ token, id, destinationId }: {
    token: any;
    id: any;
    destinationId: any;
}): Promise<any>;
export function fetchGeneratorsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
