import { executeFetch } from "../../client/apiClient";

export const fetchCampaignInvokationLogsAsync = async ({
  campaignApiName,
  token,
  id,
  page,
  pageSize,
}) => {
  return await executeFetch({
    path: `api/campaigns/${campaignApiName}/${id}/InvokationLogs`,
    page,
    pageSize,
    token,
  });
};
