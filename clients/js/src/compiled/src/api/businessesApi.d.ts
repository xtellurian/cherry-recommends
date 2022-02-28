import { AuthenticatedRequest, EntitySearchRequest, PaginateResponse, Business } from "../interfaces";
import { components } from "../model/api";
export declare const fetchBusinessesAsync: ({ token, page, searchTerm, }: EntitySearchRequest) => Promise<PaginateResponse<Business>>;
interface CreateBusinessRequest extends AuthenticatedRequest {
    business: components["schemas"]["CreateBusiness"];
}
export declare const createBusinessAsync: ({ token, business, }: CreateBusinessRequest) => Promise<any>;
export {};
