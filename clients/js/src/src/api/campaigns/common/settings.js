import { executeFetch } from "../../client/apiClient";

export const setSettingsAsync = async ({
  campaignApiName,
  token,
  id,
  settings,
}) => {
  return await executeFetch({
    path: `api/campaigns/${campaignApiName}/${id}/Settings`,
    token,
    method: "post",
    body: settings,
  });
};

export const getSettingsAsync = async ({ campaignApiName, token, id }) => {
  return await executeFetch({
    token,
    path: `api/campaigns/${campaignApiName}/${id}/Settings`,
  });
};
