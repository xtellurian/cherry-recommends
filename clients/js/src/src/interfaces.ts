export interface AuthenticatedRequest {
  token: string;
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

export interface EntitySearchRequest extends PaginatedRequest {
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

interface CommonEntity {
  id: number;
  commonId: string;
  properties: any;
}

export interface RecommendableItem extends CommonEntity {}
interface Customer extends CommonEntity {}
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
