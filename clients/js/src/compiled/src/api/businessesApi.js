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
export const createBusinessAsync = async ({ token, business, }) => {
    return await executeFetch({
        path: "api/Businesses",
        token,
        method: "post",
        body: business,
    });
};
