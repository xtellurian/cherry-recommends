interface ExecuteFetchRequest {
    method?: "get" | "post" | "put" | "delete" | undefined;
    token?: string;
    apiKey?: string;
    path: string;
    page?: number | null;
    pageSize?: number | null;
    body?: object | string | null;
    query?: object;
    responseType?: "json" | "blob" | "arraybuffer" | "document" | "text" | "stream" | undefined;
}
export declare const executeFetch: ({ token, apiKey, path, page, pageSize, body, method, query, responseType, }?: ExecuteFetchRequest) => Promise<any>;
export {};
