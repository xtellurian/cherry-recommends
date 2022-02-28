import { EntitySearchRequest, PaginateResponse, Business } from "../interfaces";
export declare const fetchBusinessesAsync: ({ token, page, searchTerm, }: EntitySearchRequest) => Promise<PaginateResponse<Business>>;
