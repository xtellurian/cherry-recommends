import {
  AuthenticatedRequest,
  DeleteRequest,
  DeleteResponse,
  EntityRequest,
  EntitySearchRequest,
  PaginateResponse,
  RecommendableItem,
  SetpropertiesRequest,
} from "../interfaces";
import { executeFetch } from "./client/apiClient";
import { components } from "../model/api";

import * as pr from "./commonEntity/propertiesApiUtil";

console.warn("Deprecation Notice: Recommendable Items are replaced by Promotions.")
export const fetchItemsAsync = async ({
  token,
  page,
  searchTerm,
}: EntitySearchRequest): Promise<PaginateResponse<RecommendableItem>> => {
  return await executeFetch({
    path: "api/RecommendableItems",
    token,
    page,
    query: {
      "q.term": searchTerm,
    },
  });
};

export const fetchItemAsync = async ({ token, id }: EntityRequest) => {
  return await executeFetch({
    path: `api/RecommendableItems/${id}`,
    token,
  });
};

interface CreateItemRequest extends AuthenticatedRequest {
  item: components["schemas"]["CreatePromotionDto"];
}
export const createItemAsync = async ({
  token,
  item,
}: CreateItemRequest): Promise<RecommendableItem> => {
  return await executeFetch({
    path: "api/RecommendableItems",
    token,
    method: "post",
    body: item,
  });
};

interface UpdateItemRequest extends EntityRequest {
  item: {
    name: string;
    description: string;
    properties: any;
  };
}
export const updateItemAsync = async ({
  token,
  id,
  item,
}: UpdateItemRequest): Promise<RecommendableItem> => {
  return await executeFetch({
    path: `api/RecommendableItems/${id}`,
    token,
    method: "post",
    body: item,
  });
};

export const deleteItemAsync = async ({
  token,
  id,
}: DeleteRequest): Promise<DeleteResponse> => {
  return await executeFetch({
    path: `api/RecommendableItems/${id}`,
    token,
    method: "delete",
  });
};

export const getPropertiesAsync = async ({ token, id }: EntityRequest) => {
  return await pr.getPropertiesAsync({
    token,
    id,
    api: "RecommendableItems",
  });
};

export const setPropertiesAsync = async ({
  token,
  id,
  properties,
}: SetpropertiesRequest) => {
  return await pr.setPropertiesAsync({
    token,
    id,
    properties,
    api: "RecommendableItems",
  });
};
