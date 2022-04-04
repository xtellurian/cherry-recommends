interface ExecuteFetchRequest {
    method?: "get" | "post" | "put" | "delete" | undefined;
    token?: string;
    apiKey?: string;
    path: string;
    page?: number | null;
    pageSize?: number | null;
    body?: object | string | null;
    query?: object;
}
export declare const executeFetch: ({ token, apiKey, path, page, pageSize, body, method, query, }?: ExecuteFetchRequest) => Promise<any>;
export {};
