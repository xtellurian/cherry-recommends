import { executeFetch } from "../../client/apiClient";
export const setArgumentsAsync = async ({ recommenderApiName, token, id, args, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Arguments`,
        token,
        method: "post",
        body: args,
    });
};
export const fetchArgumentsAsync = async ({ recommenderApiName, token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Arguments`,
        token,
        method: "get",
    });
};
export const fetchChoosePromotionArgumentRulesAsync = async ({ recommenderApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChoosePromotionArgumentRules`,
        token,
        method: "get",
        query: {
            useInternalId,
        },
    });
};
export const createChoosePromotionArgumentRuleAsync = async ({ recommenderApiName, token, id, useInternalId, rule, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChoosePromotionArgumentRules`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
export const updateChoosePromotionArgumentRuleAsync = async ({ recommenderApiName, token, id, useInternalId, rule, ruleId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChoosePromotionArgumentRules/${ruleId}`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
export const deleteArgumentRuleAsync = async ({ recommenderApiName, token, id, useInternalId, ruleId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ArgumentRules/${ruleId}`,
        token,
        method: "delete",
        query: {
            useInternalId,
        },
    });
};
