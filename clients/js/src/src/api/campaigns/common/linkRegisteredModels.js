import { executeFetch } from "../../client/apiClient";

export const fetchLinkedRegisteredModelAsync = async ({
  campaignApiName,
  token,
  id,
}) => {
  return await executeFetch({
    path: `api/campaigns/${campaignApiName}/${id}/ModelRegistration`,
    token,
  });
};

export const createLinkedRegisteredModelAsync = async ({
  campaignApiName,
  token,
  id,
  modelId,
}) => {
  return await executeFetch({
    path: `api/campaigns/${campaignApiName}/${id}/ModelRegistration`,
    token,
    method: "post",
    body: { modelId },
  });
};
