export declare const setTenant: (tenant: string) => void;
export declare const setDefaultEnvironmentId: (e: number) => void;
export declare const setDefaultApiKey: (k: string) => void;
export declare const defaultHeaders: {
    [key: string]: string | number;
};
export declare const headers: (token: string | null | undefined, apiKey: string | null | undefined) => {
    [x: string]: string | number;
};
