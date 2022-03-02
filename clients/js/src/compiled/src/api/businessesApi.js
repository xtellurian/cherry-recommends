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
        path: `api/Businesses/${id}/properties`,
        method: "post",
        body: properties,
    });
};
