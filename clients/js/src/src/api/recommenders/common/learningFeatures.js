import { executeFetch } from "../../client/apiClient";

export const fetchLearningFeaturesAsync = async ({
  recommenderApiName,
  token,
  id,
  useInternalId,
}) => {
  return await executeFetch({
    token,
    query: { useInternalId },
    path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
  });
};

export const setLearningFeaturesAsync = async ({
  recommenderApiName,
  token,
  id,
  useInternalId,
  featureIds,
}) => {
  return await executeFetch({
    path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
    token,
    method: "post",
    query: { useInternalId },
    body: { featureIds, useInternalId },
  });
};
