import { executeFetch } from "../../client/apiClient";

import { EntityRequest } from "../../../interfaces";

interface CommonFetchLearningMetricsRequest extends EntityRequest {
  campaignApiName:
    | "ParameterSetRecommenders"
    | "ItemsRecommenders"
    | "PromotionsRecommenders"
    | "ParameterSetCampaigns"
    | "PromotionsCampaigns";
}
export const fetchLearningMetricsAsync = async ({
  campaignApiName,
  token,
  id,
  useInternalId,
}: CommonFetchLearningMetricsRequest) => {
  return await executeFetch({
    token,
    query: { useInternalId },
    path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
  });
};

interface CommonSetLearningMetricsRequest extends EntityRequest {
  campaignApiName:
    | "ParameterSetRecommenders"
    | "ItemsRecommenders"
    | "PromotionsRecommenders"
    | "ParameterSetCampaigns"
    | "PromotionsCampaigns";
  metricIds: string[];
}

export const setLearningMetricsAsync = async ({
  campaignApiName,
  token,
  id,
  useInternalId,
  metricIds,
}: CommonSetLearningMetricsRequest) => {
  return await executeFetch({
    path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
    token,
    method: "post",
    query: { useInternalId },
    body: { metricIds, useInternalId },
  });
};
