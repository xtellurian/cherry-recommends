import { executeFetch } from "../../client/apiClient";
export const fetchCampaignTargetVariableValuesAsync = async ({ campaignApiName, name, token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/TargetVariableValues`,
        token,
        query: {
            name,
        },
    });
};
export const createCampaignTargetVariableValueAsync = async ({ campaignApiName, targetVariableValue, token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/TargetVariableValues`,
        token,
        method: "post",
        body: targetVariableValue,
    });
};
