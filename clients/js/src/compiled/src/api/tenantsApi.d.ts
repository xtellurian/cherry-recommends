import { AuthenticatedRequest, EntityRequest } from "../interfaces";
import { components } from "../model/api";
declare type Tenant = components["schemas"]["Tenant"];
declare type BillingAccount = components["schemas"]["BillingAccount"];
declare type PaginatedMemberships = components["schemas"]["UserInfoPaginated"];
declare type Hosting = components["schemas"]["Hosting"];
export declare const fetchCurrentTenantAsync: ({ token, }: AuthenticatedRequest) => Promise<Tenant>;
export declare const fetchAccountAsync: ({ token, id, }: EntityRequest) => Promise<BillingAccount>;
export declare const fetchHostingAsync: ({ token, }: AuthenticatedRequest) => Promise<Hosting>;
export declare const fetchCurrentTenantMembershipsAsync: ({ token, }: AuthenticatedRequest) => Promise<PaginatedMemberships>;
interface CreateTenantMembershipRequest extends AuthenticatedRequest {
    email: string;
}
export declare const createTenantMembershipAsync: ({ token, email, }: CreateTenantMembershipRequest) => Promise<any>;
export {};
