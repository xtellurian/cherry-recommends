import { executeFetch } from "../../client/apiClientTs";

import { components } from "../../../model/api";
import { EntityRequest } from "../../../interfaces";

interface CommonFetchLearningMetricsRequest extends EntityRequest {
  recommenderApiName:
    | "ParameterSetRecommenders"
    | "ItemsRecommenders"
    | "PromotionsRecommenders";
}
export const fetchLearningMetricsAsync = async ({
  recommenderApiName,
  token,
  id,
  useInternalId,
}: CommonFetchLearningMetricsRequest) => {
  return await executeFetch({
    token,
    query: { useInternalId },
    path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
  });
};

interface CommonSetLearningMetricsRequest extends EntityRequest {
  recommenderApiName:
    | "ParameterSetRecommenders"
    | "ItemsRecommenders"
    | "PromotionsRecommenders";
  metricIds: string[];
}

export const setLearningMetricsAsync = async ({
  recommenderApiName,
  token,
  id,
  useInternalId,
  metricIds,
}: CommonSetLearningMetricsRequest) => {
  return await executeFetch({
    path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
    token,
    method: "post",
    query: { useInternalId },
    body: { metricIds, useInternalId },
  });
};
