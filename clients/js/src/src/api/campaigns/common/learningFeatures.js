import { executeFetch } from "../../client/apiClient";

export const fetchLearningFeaturesAsync = async ({
  campaignApiName,
  token,
  id,
  useInternalId,
}) => {
  return await executeFetch({
    token,
    query: { useInternalId },
    path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
  });
};

export const setLearningFeaturesAsync = async ({
  campaignApiName,
  token,
  id,
  useInternalId,
  featureIds,
}) => {
  return await executeFetch({
    path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
    token,
    method: "post",
    query: { useInternalId },
    body: { featureIds, useInternalId },
  });
};
