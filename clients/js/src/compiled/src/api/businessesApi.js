import { executeFetch } from "./client/apiClientTs";
export const fetchBusinessesAsync = async ({ token, page, searchTerm, }) => {
    return await executeFetch({
        path: "api/Businesses",
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
export const fetchBusinessAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}`,
        token,
    });
};
export const deleteBusinessAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Businesses/${id}`,
        token,
        method: "delete",
    });
};
export const createBusinessAsync = async ({ token, business, }) => {
    return await executeFetch({
        path: "api/Businesses",
        token,
        method: "post",
        body: business,
    });
};
export const updateBusinessPropertiesAsync = async ({ token, id, properties }) => {
    return await executeFetch({
        token,
        path: `api/Businesses/${id}/Properties`,
        method: "post",
        body: properties,
    });
};
export const fetchBusinessMembersAsync = async ({ token, id, page, searchTerm, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/Members`,
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
export const deleteBusinessMemberAsync = async ({ token, id, customerId }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/Members/${customerId}`,
        token,
        method: "delete",
    });
};
