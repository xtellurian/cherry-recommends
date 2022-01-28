export function fetchModelRegistrationsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
export function fetchModelRegistrationAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function deleteModelRegistrationAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function createModelRegistrationAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
export function invokeModelAsync({ token, modelId, metrics }: {
    token: any;
    modelId: any;
    metrics: any;
}): Promise<any>;
