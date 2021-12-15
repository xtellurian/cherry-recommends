import { executeFetch } from "./client/apiClient";
export const fetchRewardSelectorsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/RewardSelectors",
        token,
        page,
    });
};
export const fetchRewardSelectorAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/RewardSelectors/${id}`,
        token,
        page,
    });
};
export const deleteRewardSelectorAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/RewardSelectors/${id}`,
        token,
        method: "delete",
    });
};
export const createRewardSelectorAsync = async ({ token, entity }) => {
    return await executeFetch({
        path: "api/RewardSelectors",
        token,
        method: "post",
        body: entity,
    });
};
