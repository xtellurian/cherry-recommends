import { AuthenticatedRequest, DeleteRequest, DeleteResponse, EntityRequest, EntitySearchRequest, PaginateResponse, RecommendableItem, SetpropertiesRequest } from "../interfaces";
export declare const fetchItemsAsync: ({ token, page, searchTerm, }: EntitySearchRequest) => Promise<PaginateResponse<RecommendableItem>>;
export declare const fetchItemAsync: ({ token, id }: EntityRequest) => Promise<any>;
interface CreateItemRequest extends AuthenticatedRequest {
    item: {
        commonId: string;
        name: string;
        listPrice: number;
        firectCost: number;
        description: number;
        properties: any;
    };
}
export declare const createItemAsync: ({ token, item, }: CreateItemRequest) => Promise<RecommendableItem>;
interface UpdateItemRequest extends EntityRequest {
    item: {
        name: string;
        description: string;
        properties: any;
    };
}
export declare const updateItemAsync: ({ token, id, item, }: UpdateItemRequest) => Promise<RecommendableItem>;
export declare const deleteItemAsync: ({ token, id, }: DeleteRequest) => Promise<DeleteResponse>;
export declare const getPropertiesAsync: ({ token, id }: EntityRequest) => Promise<any>;
export declare const setPropertiesAsync: ({ token, id, properties, }: SetpropertiesRequest) => Promise<any>;
export {};
