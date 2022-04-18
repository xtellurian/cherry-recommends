import { AuthenticatedRequest, EntityRequest } from "../interfaces";
import { executeFetch } from "./client/apiClient";
import { components } from "../model/api";

type Tenant = components["schemas"]["Tenant"];
type BillingAccount = components["schemas"]["BillingAccount"];
type PaginatedMemberships = components["schemas"]["UserInfoPaginated"];
type Hosting = components["schemas"]["Hosting"];

export const fetchCurrentTenantAsync = async ({
  token,
}: AuthenticatedRequest): Promise<Tenant> => {
  return await executeFetch({
    path: "api/tenants/current",
    token,
    method: "get",
  });
};

export const fetchAccountAsync = async ({
  token,
  id,
}: EntityRequest): Promise<BillingAccount> => {
  return await executeFetch({
    path: `api/Tenants/${id}/Account`,
    token,
    method: "get",
  });
};

export const fetchHostingAsync = async ({
  token,
}: AuthenticatedRequest): Promise<Hosting> => {
  return await executeFetch({
    path: "api/Tenants/Hosting",
    token,
    method: "get",
  });
};

export const fetchCurrentTenantMembershipsAsync = async ({
  token,
}: AuthenticatedRequest): Promise<PaginatedMemberships> => {
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
