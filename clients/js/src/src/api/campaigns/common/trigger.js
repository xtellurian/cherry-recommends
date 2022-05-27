import { executeFetch } from "../../client/apiClient";

export const setTriggerAsync = async ({
  campaignApiName,
  token,
  id,
  trigger,
}) => {
  return await executeFetch({
    path: `api/campaigns/${campaignApiName}/${id}/TriggerCollection`,
    token,
    method: "post",
    body: trigger,
  });
};

export const fetchTriggerAsync = async ({ campaignApiName, token, id }) => {
  return await executeFetch({
    token,
    path: `api/campaigns/${campaignApiName}/${id}/TriggerCollection`,
  });
};
