import {
  EntitySearchRequest,
  PaginateResponse,
  Business
} from "../interfaces";
import { executeFetch } from "./client/apiClientTs";

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
