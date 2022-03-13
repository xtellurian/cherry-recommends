export function fetchSegmentsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
export function fetchSegmentAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function createSegmentAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
export function deleteSegmentAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function addCustomerAsync({ token, id, customerId }: {
    token: any;
    id: any;
    customerId: any;
}): Promise<any>;
export function removeCustomerAsync({ token, id, customerId }: {
    token: any;
    id: any;
    customerId: any;
}): Promise<any>;
export function fetchSegmentCustomersAsync({ token, page, id, searchTerm, weeksAgo, }: {
    token: any;
    page: any;
    id: any;
    searchTerm: any;
    weeksAgo: any;
}): Promise<any>;
