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
    path: `api/Businesses/${id}/properties`,
    method: "post",
    body: properties,
  });
};
