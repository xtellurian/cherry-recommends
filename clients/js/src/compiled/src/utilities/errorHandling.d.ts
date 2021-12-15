export function setErrorResponseHandler(errorHandler: any): void;
export function handleErrorResponse(response: any): Promise<{
    error: any;
} | undefined>;
export function handleErrorFetch(ex: any): void;
export function setErrorFetchHandler(handler: any): void;
