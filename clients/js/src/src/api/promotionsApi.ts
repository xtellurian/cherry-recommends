import {
    AuthenticatedRequest,
    DeleteRequest,
    DeleteResponse,
    EntityRequest,
    EntitySearchRequest,
    PaginateResponse,
    Promotion,
    SetpropertiesRequest,
  } from "../interfaces";
  import { executeFetch } from "./client/apiClientTs";
  import { components } from "../model/api";
  
  import * as pr from "./commonEntity/propertiesApiUtil";
  
  export const fetchPromotionsAsync = async ({
    token,
    page,
    searchTerm,
  }: EntitySearchRequest): Promise<PaginateResponse<Promotion>> => {
    return await executeFetch({
      path: "api/Promotions",
      token,
      page,
      query: {
        "q.term": searchTerm,
      },
    });
  };
  
  export const fetchPromotionAsync = async ({ token, id }: EntityRequest) => {
    return await executeFetch({
      path: `api/Promotions/${id}`,
      token,
    });
  };
  
  interface CreatePromotionRequest extends AuthenticatedRequest {
    promotion: components["schemas"]["CreatePromotionDto"];
  }
  export const createPromotionAsync = async ({
    token,
    promotion,
  }: CreatePromotionRequest): Promise<Promotion> => {
    return await executeFetch({
      path: "api/Promotions",
      token,
      method: "post",
      body: promotion,
    });
  };
  
  interface UpdatePromotionRequest extends EntityRequest {
    promotion: {
      name: string;
      description: string;
      properties: any;
    };
  }
  export const updatePromotionAsync = async ({
    token,
    id,
    promotion,
  }: UpdatePromotionRequest): Promise<Promotion> => {
    return await executeFetch({
      path: `api/Promotions/${id}`,
      token,
      method: "post",
      body: promotion,
    });
  };
  
  export const deletePromotionAsync = async ({
    token,
    id,
  }: DeleteRequest): Promise<DeleteResponse> => {
    return await executeFetch({
      path: `api/Promotions/${id}`,
      token,
      method: "delete",
    });
  };
  
  export const getPropertiesAsync = async ({ token, id }: EntityRequest) => {
    return await pr.getPropertiesAsync({
      token,
      id,
      api: "Promotions",
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
      api: "Promotions",
    });
  };
  