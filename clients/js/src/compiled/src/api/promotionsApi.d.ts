import { AuthenticatedRequest, DeleteRequest, DeleteResponse, EntityRequest, EntitySearchRequest, PaginateResponse, Promotion, SetpropertiesRequest } from "../interfaces";
import { components } from "../model/api";
export declare const fetchPromotionsAsync: ({ token, page, searchTerm, }: EntitySearchRequest) => Promise<PaginateResponse<Promotion>>;
export declare const fetchPromotionAsync: ({ token, id }: EntityRequest) => Promise<any>;
interface CreatePromotionRequest extends AuthenticatedRequest {
    promotion: components["schemas"]["CreatePromotionDto"];
}
export declare const createPromotionAsync: ({ token, promotion, }: CreatePromotionRequest) => Promise<Promotion>;
interface UpdatePromotionRequest extends EntityRequest {
    promotion: {
        name: string;
        description: string;
        properties: any;
    };
}
export declare const updatePromotionAsync: ({ token, id, promotion, }: UpdatePromotionRequest) => Promise<Promotion>;
export declare const deletePromotionAsync: ({ token, id, }: DeleteRequest) => Promise<DeleteResponse>;
export declare const getPropertiesAsync: ({ token, id }: EntityRequest) => Promise<any>;
export declare const setPropertiesAsync: ({ token, id, properties, }: SetpropertiesRequest) => Promise<any>;
export {};
