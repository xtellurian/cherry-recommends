export interface AuthenticatedRequest {
    token: string;
    useInternalId?: boolean | undefined;
}
export interface PaginatedRequest extends AuthenticatedRequest {
    page: number;
}
export interface PaginateResponse<T> {
    items: T[];
    pagination: {
        pageCount: number;
        totalItemCount: number;
        pageNumber: number;
        hasPreviousPage: boolean;
        hasNextPage: boolean;
        isFirstPage: boolean;
        isLastPage: boolean;
    };
}
export interface EntityRequest extends AuthenticatedRequest {
    id: number | string;
    useInternalId: boolean | undefined;
}
export interface PaginatedEntityRequest extends EntityRequest {
    page?: number | undefined;
    pageSize?: number | undefined;
}
export interface EntitySearchRequest extends PaginatedEntityRequest {
    searchTerm: string | undefined;
}
export interface DeleteRequest extends AuthenticatedRequest {
    id: number | string;
}
export interface DeleteResponse {
    id: number;
    resourceUrl: string;
    success: boolean;
}
export interface SetpropertiesRequest extends EntityRequest {
    properties: any;
}
export interface ModelInput {
    commonUserId: string;
    arguments: any | undefined;
}
interface Entity {
    id: number;
}
interface CommonEntity extends Entity {
    commonId: string;
    properties: any;
}
export interface RecommendableItem extends CommonEntity {
}
export interface Promotion extends RecommendableItem {
}
export interface CustomerEvent {
    commonUserId?: string | undefined;
    customerId: string;
    eventId: string;
    timestamp?: string | undefined;
    recommendationCorrelatorId?: number | undefined | null;
    sourceSystemId?: number | null | undefined;
    kind: "Custom" | "Behaviour" | "ConsumeRecommendation";
    eventType: string;
    properties?: any | null | undefined;
}
interface Customer extends CommonEntity {
}
interface ScoredItem {
    itemId: number | undefined;
    itemCommonId: string | undefined;
    commonId: string | undefined;
    item: RecommendableItem;
    score: number;
}
export interface ItemsRecommendation {
    created: string;
    correlatorId: null;
    commonUserId: string;
    scoredItems: ScoredItem[];
    customer: Customer;
    trigger: string;
}
export interface PromotionsRecommendation extends ItemsRecommendation {
}
export interface Business extends CommonEntity {
}
export interface MetricBinRequest extends EntityRequest {
    binCount: number;
}
export interface PromotionsRequest extends EntitySearchRequest {
    promotionType: string | undefined;
    benefitType: string | undefined;
    weeksAgo: number | undefined;
}
export {};
