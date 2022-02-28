import {
  AuthenticatedRequest,
  EntitySearchRequest,
  PaginateResponse,
  Business
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

interface CreateBusinessRequest extends AuthenticatedRequest {
  business: components["schemas"]["CreateBusiness"];
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
