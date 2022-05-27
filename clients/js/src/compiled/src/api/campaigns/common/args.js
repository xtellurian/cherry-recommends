import { executeFetch } from "../../client/apiClient";
export const setArgumentsAsync = async ({ campaignApiName, token, id, args, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/Arguments`,
        token,
        method: "post",
        body: args,
    });
};
export const fetchArgumentsAsync = async ({ campaignApiName, token, id }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/Arguments`,
        token,
        method: "get",
    });
};
export const fetchChoosePromotionArgumentRulesAsync = async ({ campaignApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChoosePromotionArgumentRules`,
        token,
        method: "get",
        query: {
            useInternalId,
        },
    });
};
export const createChoosePromotionArgumentRuleAsync = async ({ campaignApiName, token, id, useInternalId, rule, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChoosePromotionArgumentRules`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
export const updateChoosePromotionArgumentRuleAsync = async ({ campaignApiName, token, id, useInternalId, rule, ruleId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChoosePromotionArgumentRules/${ruleId}`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
export const fetchChooseSegmentArgumentRulesAsync = async ({ campaignApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChooseSegmentArgumentRules`,
        token,
        method: "get",
        query: {
            useInternalId,
        },
    });
};
export const createChooseSegmentArgumentRuleAsync = async ({ campaignApiName, token, id, useInternalId, rule, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChooseSegmentArgumentRules`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
export const updateChooseSegmentArgumentRuleAsync = async ({ campaignApiName, token, id, useInternalId, rule, ruleId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChooseSegmentArgumentRules/${ruleId}`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
export const deleteArgumentRuleAsync = async ({ campaignApiName, token, id, useInternalId, ruleId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ArgumentRules/${ruleId}`,
        token,
        method: "delete",
        query: {
            useInternalId,
        },
    });
};
