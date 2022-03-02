import { Business, DeleteRequest, EntityRequest, AuthenticatedRequest, EntitySearchRequest, PaginateResponse } from "../interfaces";
import { components } from "../model/api";
export declare const fetchBusinessesAsync: ({ token, page, searchTerm, }: EntitySearchRequest) => Promise<PaginateResponse<Business>>;
export declare const fetchBusinessAsync: ({ token, id, }: EntityRequest) => Promise<components["schemas"]["Business"]>;
export declare const deleteBusinessAsync: ({ token, id }: DeleteRequest) => Promise<any>;
interface CreateBusinessRequest extends AuthenticatedRequest {
    business: components["schemas"]["CreateBusiness"];
}
export declare const createBusinessAsync: ({ token, business, }: CreateBusinessRequest) => Promise<any>;
interface UpdateBusinessPropertiesRequest extends EntityRequest {
    properties?: {
        [key: string]: unknown;
    } | null;
}
export declare const updateBusinessPropertiesAsync: ({ token, id, properties }: UpdateBusinessPropertiesRequest) => Promise<any>;
export {};
