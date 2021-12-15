import {
  AuthenticatedRequest,
  PaginatedRequest,
  DeleteRequest,
} from "../interfaces";
import { executeFetch } from "./client/apiClientTs";

export const fetchApiKeysAsync = async ({ token, page }: PaginatedRequest) => {
  return await executeFetch({
    path: "api/apiKeys",
    token,
    page,
  });
};

enum ApiKeyType {
  Server = "Server",
  Web = "Web",
}
interface CreateApiKeyPayload {
  name: string;
  apiKeyType: ApiKeyType;
}

interface CreateApiKeyRequest extends AuthenticatedRequest {
  payload: CreateApiKeyPayload;
}

export const createApiKeyAsync = async ({
  token,
  payload,
}: CreateApiKeyRequest) => {
  return await executeFetch({
    path: "api/apiKeys",
    token,
    method: "post",
    body: payload,
  });
};

interface ExchangeApiKeyRequest {
  apiKey: string;
}
interface AccessTokenResponse {
  access_token: string;
}
export const exchangeApiKeyAsync = async ({
  apiKey,
}: ExchangeApiKeyRequest): Promise<AccessTokenResponse> => {
  return await executeFetch({
    path: "api/apiKeys/exchange",
    method: "post",
    body: { apiKey },
  });
};

export const deleteApiKeyAsync = async ({ token, id }: DeleteRequest) => {
  return await executeFetch({
    path: `api/apiKeys/${id}`,
    token,
    method: "delete",
  });
};
