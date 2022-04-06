import { AuthenticatedRequest } from "../interfaces";
import { components } from "../model/api";
declare type Tenant = components["schemas"]["Tenant"];
export declare const fetchCurrentTenantAsync: ({ token, }: AuthenticatedRequest) => Promise<Tenant>;
export declare const fetchHostingAsync: ({ token }: AuthenticatedRequest) => Promise<any>;
export declare const fetchCurrentTenantMembershipsAsync: ({ token, }: AuthenticatedRequest) => Promise<any>;
interface CreateTenantMembershipRequest extends AuthenticatedRequest {
    email: string;
}
export declare const createTenantMembershipAsync: ({ token, email, }: CreateTenantMembershipRequest) => Promise<any>;
export {};
