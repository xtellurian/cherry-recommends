import { AuthenticatedRequest } from "../interfaces";
import { executeFetch } from "./client/apiClient";
import { components } from "../model/api";

type Tenant = components["schemas"]["Tenant"];

export const fetchCurrentTenantAsync = async ({
  token,
}: AuthenticatedRequest): Promise<Tenant> => {
  return await executeFetch({
    path: "api/tenants/current",
    token,
    method: "get",
  });
};

export const fetchHostingAsync = async ({ token }: AuthenticatedRequest) => {
  return await executeFetch({
    path: "api/tenants/hosting",
    token,
    method: "get",
  });
};

export const fetchCurrentTenantMembershipsAsync = async ({
  token,
}: AuthenticatedRequest) => {
  return await executeFetch({
    path: "api/tenants/current/memberships",
    token,
    method: "get",
  });
};

interface CreateTenantMembershipRequest extends AuthenticatedRequest {
  email: string;
}
export const createTenantMembershipAsync = async ({
  token,
  email,
}: CreateTenantMembershipRequest) => {
  return await executeFetch({
    path: "api/tenants/current/memberships",
    token,
    method: "post",
    body: { email },
  });
};
