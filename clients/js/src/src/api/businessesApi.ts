import {
  Business,
  DeleteRequest,
  EntityRequest,
  AuthenticatedRequest,
  EntitySearchRequest,
  PaginateResponse,
} from "../interfaces";
import { executeFetch } from "./client/apiClientTs";
import { components } from "../model/api";

export const fetchBusinessesAsync = async ({
  token,
  page,
  searchTerm,
}: EntitySearchRequest): Promise<PaginateResponse<Business>> => {
  return await executeFetch({
    path: "api/Businesses",
    token,
    page,
    query: {
      "q.term": searchTerm,
    },
  });
};

export const fetchBusinessAsync = async ({
  token,
  id,
}: EntityRequest): Promise<components["schemas"]["Business"]> => {
  return await executeFetch({
    path: `api/Businesses/${id}`,
    token,
  });
};

export const deleteBusinessAsync = async ({ token, id }: DeleteRequest) => {
  return await executeFetch({
    path: `api/Businesses/${id}`,
    token,
    method: "delete",
  });
};

interface CreateBusinessRequest extends AuthenticatedRequest {
  business: components["schemas"]["CreateBusinessDto"];
}
export const createBusinessAsync = async ({
  token,
  business,
}: CreateBusinessRequest) => {
  return await executeFetch({
    path: "api/Businesses",
    token,
    method: "post",
    body: business,
  });
};

interface UpdateBusinessPropertiesRequest extends EntityRequest {
  properties?: { [key: string]: unknown } | null;
}
export const updateBusinessPropertiesAsync = async ({ 
  token, 
  id, 
  properties 
}: UpdateBusinessPropertiesRequest) => {
  return await executeFetch({
    token,
    path: `api/Businesses/${id}/Properties`,
    method: "post",
    body: properties,
  });
};

export const fetchBusinessMembersAsync = async ({
  token,
  id,
  page,
  searchTerm,
}: EntitySearchRequest): Promise<PaginateResponse<Business>> => {
  return await executeFetch({
    path: `api/Businesses/${id}/Members`,
    token,
    page,
    query: {
      "q.term": searchTerm,
    },
  });
};

interface DeleteBusinessMemberRequest extends DeleteRequest {
  customerId: number;
}

export const deleteBusinessMemberAsync = async ({ 
  token, 
  id, 
  customerId 
}: DeleteBusinessMemberRequest) => {
  return await executeFetch({
    path: `api/Businesses/${id}/Members/${customerId}`,
    token,
    method: "delete",
  });
};
