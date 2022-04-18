import { executeFetch } from "./client/apiClient";
export const fetchCurrentTenantAsync = async ({ token, }) => {
    return await executeFetch({
        path: "api/tenants/current",
        token,
        method: "get",
    });
};
export const fetchAccountAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Tenants/${id}/Account`,
        token,
        method: "get",
    });
};
export const fetchHostingAsync = async ({ token, }) => {
    return await executeFetch({
        path: "api/Tenants/Hosting",
        token,
        method: "get",
    });
};
export const fetchCurrentTenantMembershipsAsync = async ({ token, }) => {
    return await executeFetch({
        path: "api/tenants/current/memberships",
        token,
        method: "get",
    });
};
export const createTenantMembershipAsync = async ({ token, email, }) => {
    return await executeFetch({
        path: "api/tenants/current/memberships",
        token,
        method: "post",
        body: { email },
    });
};
