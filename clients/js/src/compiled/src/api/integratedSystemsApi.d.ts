export function fetchIntegratedSystemsAsync({ token, page, systemType, }: {
    token: any;
    page: any;
    systemType: any;
}): Promise<any>;
export function fetchIntegratedSystemAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function renameAsync({ token, id, name }: {
    token: any;
    id: any;
    name: any;
}): Promise<any>;
export function createIntegratedSystemAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
export function deleteIntegratedSystemAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function fetchWebhookReceiversAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function createWebhookReceiverAsync({ token, id, useSharedSecret, }: {
    token: any;
    id: any;
    useSharedSecret: any;
}): Promise<any>;
export function setIsDCGeneratorAsync({ token, id, value }: {
    token: any;
    id: any;
    value: any;
}): Promise<any>;
