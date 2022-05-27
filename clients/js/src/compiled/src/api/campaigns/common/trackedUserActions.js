import { executeFetch } from "../../client/apiClient";
export const fetchCampaignTrackedUserActionsAsync = async ({ campaignApiName, revenueOnly, token, id, page, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/TrackedUserActions`,
        token,
        page,
        query: {
            revenueOnly,
        },
    });
};
