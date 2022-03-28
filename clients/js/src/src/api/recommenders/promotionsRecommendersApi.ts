import { executeFetch } from "../client/apiClientTs";
import * as link from "./common/linkRegisteredModels";

import { components } from "../../model/api";

import * as ar from "./common/args";
import * as tv from "./common/targetvariables";
import * as il from "./common/invokationLogs";
import * as st from "./common/settings";
import * as ds from "./common/destinations";
import * as trig from "./common/trigger";
import * as lf from "./common/learningFeatures";
import * as lm from "./common/learningMetrics";
import * as ri from "./common/reportImages";

import {
  PaginatedRequest,
  EntityRequest,
  DeleteRequest,
  AuthenticatedRequest,
  ModelInput,
  PaginatedEntityRequest,
  PromotionsRecommendation,
} from "../../interfaces";

const recommenderApiName = "PromotionsRecommenders";

export const fetchPromotionsRecommendersAsync = async ({
  token,
  page,
}: PaginatedRequest) => {
  return await executeFetch({
    token,
    path: "api/recommenders/PromotionsRecommenders",
    page,
  });
};

export const fetchPromotionsRecommenderAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/recommenders/PromotionsRecommenders/${id}`,
    token,
  });
};

interface PromotionsRecommendationsRequest extends PaginatedEntityRequest {
  page: number;
}

export const fetchPromotionsRecommendationsAsync = async ({
  token,
  page,
  pageSize,
  id,
}: PromotionsRecommendationsRequest) => {
  return await executeFetch({
    token,
    path: `api/recommenders/PromotionsRecommenders/${id}/Recommendations`,
    page,
    pageSize,
  });
};

export const deletePromotionsRecommenderAsync = async ({
  token,
  id,
}: DeleteRequest) => {
  return await executeFetch({
    path: `api/recommenders/PromotionsRecommenders/${id}`,
    token,
    method: "delete",
  });
};

interface CreatePromotionsRecommenderRequest extends AuthenticatedRequest {
  payload: components["schemas"]["CreatePromotionsRecommender"];
}
export const createPromotionsRecommenderAsync = async ({
  token,
  payload,
  useInternalId,
}: CreatePromotionsRecommenderRequest) => {
  return await executeFetch({
    path: "api/recommenders/PromotionsRecommenders",
    token,
    method: "post",
    body: payload,
    query: { useInternalId },
  });
};

export const fetchPromotionsAsync = async ({ token, id }: EntityRequest) => {
  return await executeFetch({
    path: `api/recommenders/PromotionsRecommenders/${id}/Promotions`,
    token,
  });
};

type Audience = components["schemas"]["Audience"];
export const fetchAudienceAsync = async ({
  token,
  id,
}: EntityRequest): Promise<Audience> => {
  return await executeFetch({
    path: `api/recommenders/PromotionsRecommenders/${id}/Audience`,
    token,
  });
};

interface AddPromotionPayload {
  id: number | undefined;
  commonId: string | undefined;
}

interface AddPromotionRequest extends EntityRequest {
  promotion: AddPromotionPayload;
}
export const addPromotionAsync = async ({
  token,
  id,
  promotion,
}: AddPromotionRequest) => {
  return await executeFetch({
    path: `api/recommenders/PromotionsRecommenders/${id}/Promotions`,
    token,
    method: "post",
    body: promotion,
  });
};

interface RemovePromotionRequest extends EntityRequest {
  promotionId: string | number;
}
export const removePromotionAsync = async ({
  token,
  id,
  promotionId,
}: RemovePromotionRequest) => {
  return await executeFetch({
    path: `api/recommenders/PromotionsRecommenders/${id}/Promotions/${promotionId}`,
    token,
    method: "post",
  });
};

interface SetBaselinePromotionRequest extends EntityRequest {
  promotionId: string | number;
}
export const setBaselinePromotionAsync = async ({
  token,
  id,
  promotionId,
}: SetBaselinePromotionRequest) => {
  return await executeFetch({
    path: `api/recommenders/PromotionsRecommenders/${id}/BaselinePromotion`,
    token,
    method: "post",
    body: { promotionId },
  });
};

export const getBaselinePromotionAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/recommenders/PromotionsRecommenders/${id}/BaselinePromotion`,
    token,
  });
};

type LinkRegisteredModelRequest = EntityRequest &
  components["schemas"]["LinkModel"];
export const createLinkRegisteredModelAsync = async ({
  token,
  id,
  modelId,
}: LinkRegisteredModelRequest) => {
  return await link.createLinkedRegisteredModelAsync({
    recommenderApiName,
    id,
    modelId,
    token,
  });
};

export const fetchLinkedRegisteredModelAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await link.fetchLinkedRegisteredModelAsync({
    recommenderApiName,
    id,
    token,
  });
};

interface InvokePromotionRecommenderRequest extends EntityRequest {
  input: ModelInput;
}
export const invokePromotionsRecommenderAsync = async ({
  token,
  id,
  input,
}: InvokePromotionRecommenderRequest): Promise<PromotionsRecommendation> => {
  return await executeFetch({
    path: `api/recommenders/PromotionsRecommenders/${id}/Invoke`,
    token,
    method: "post",
    body: input,
  });
};

interface FetchInvokationLogsRequest extends EntityRequest {
  page: number;
}
export const fetchInvokationLogsAsync = async ({
  id,
  token,
  page,
}: FetchInvokationLogsRequest) => {
  return await il.fetchRecommenderInvokationLogsAsync({
    recommenderApiName,
    id,
    token,
    page,
  });
};

export const fetchTargetVariablesAsync = async ({ id, token, name }: any) => {
  return await tv.fetchRecommenderTargetVariableValuesAsync({
    recommenderApiName,
    id,
    token,
    name,
  });
};

export const createTargetVariableAsync = async ({
  id,
  token,
  targetVariableValue,
}: any) => {
  return await tv.createRecommenderTargetVariableValueAsync({
    recommenderApiName,
    id,
    token,
    targetVariableValue,
  });
};

interface RecommenderSettings {
  requireConsumptionEvent: boolean;
  throwOnBadInput: boolean;
  recommendationCacheTime: string;
}
interface SetSettingsRequest extends EntityRequest {
  settings: RecommenderSettings;
}
export const setSettingsAsync = async ({
  id,
  token,
  settings,
}: SetSettingsRequest) => {
  return await st.setSettingsAsync({
    recommenderApiName,
    id,
    token,
    settings,
  });
};

interface Argument {
  commonId: string;
  argumentType: "Numerical" | "Categorical";
  defaultValue: string | number;
  isRequired: boolean;
}
interface SetArgumentsRequest extends EntityRequest {
  args: Argument[];
}

export const setArgumentsAsync = async ({
  id,
  token,
  args,
}: SetArgumentsRequest) => {
  return await ar.setArgumentsAsync({
    recommenderApiName,
    id,
    token,
    args,
  });
};

export const fetchDestinationsAsync = async ({ id, token }: EntityRequest) => {
  return await ds.fetchDestinationsAsync({
    recommenderApiName,
    id,
    token,
  });
};

interface Destination {
  destinationType:
    | "Webhook"
    | "SegmentSourceFunction"
    | "HubspotContactProperty";
  endpoint: string;
  integratedSystemId: number;
}
interface CreateDestinationRequest extends EntityRequest {
  destination: Destination;
}
export const createDestinationAsync = async ({
  id,
  token,
  destination,
}: CreateDestinationRequest) => {
  return await ds.createDestinationAsync({
    recommenderApiName,
    id,
    token,
    destination,
  });
};

interface RemoveDestinationRequest extends EntityRequest {
  destinationId: number;
}
export const removeDestinationAsync = async ({
  id,
  token,
  destinationId,
}: RemoveDestinationRequest) => {
  return await ds.removeDestinationAsync({
    recommenderApiName,
    id,
    token,
    destinationId,
  });
};

export const fetchTriggerAsync = async ({ id, token }: EntityRequest) => {
  return await trig.fetchTriggerAsync({
    recommenderApiName,
    id,
    token,
  });
};

interface Trigger {
  featuresChanged: any;
}
interface SetTriggerRequest extends EntityRequest {
  trigger: Trigger;
}
export const setTriggerAsync = async ({
  id,
  token,
  trigger,
}: SetTriggerRequest) => {
  return await trig.setTriggerAsync({
    recommenderApiName,
    id,
    token,
    trigger,
  });
};

export const fetchLearningFeaturesAsync = async ({
  id,
  token,
  useInternalId,
}: EntityRequest) => {
  return await lf.fetchLearningFeaturesAsync({
    recommenderApiName,
    id,
    token,
    useInternalId,
  });
};

interface SetLearningFeaturesRequest extends EntityRequest {
  featureIds: string[];
}
export const setLearningFeaturesAsync = async ({
  id,
  token,
  featureIds,
  useInternalId,
}: SetLearningFeaturesRequest) => {
  return await lf.setLearningFeaturesAsync({
    recommenderApiName,
    id,
    token,
    useInternalId,
    featureIds,
  });
};

export const fetchLearningMetricsAsync = async ({
  id,
  token,
  useInternalId,
}: EntityRequest) => {
  return await lm.fetchLearningMetricsAsync({
    recommenderApiName,
    id,
    token,
    useInternalId,
  });
};

interface SetLearningMetricsRequest extends EntityRequest {
  metricIds: string[];
}
export const setLearningMetricsAsync = async ({
  id,
  token,
  metricIds,
  useInternalId,
}: SetLearningMetricsRequest) => {
  return await lm.setLearningMetricsAsync({
    recommenderApiName,
    id,
    token,
    useInternalId,
    metricIds,
  });
};

type RecommenderStatistics = components["schemas"]["RecommenderStatistics"];
export const fetchStatisticsAsync = async ({
  id,
  token,
}: EntityRequest): Promise<RecommenderStatistics> => {
  return await executeFetch({
    path: `api/recommenders/PromotionsRecommenders/${id}/Statistics`,
    token,
  });
};

export const fetchReportImageBlobUrlAsync = async ({
  id,
  token,
  useInternalId,
}: EntityRequest): Promise<string | void> => {
  return await ri.fetchReportImageBlobUrlAsync({
    recommenderApiName,
    id,
    token,
    useInternalId,
  });
};

type PerformanceResponse =
  components["schemas"]["ItemsRecommenderPerformanceReport"];

interface PerformanceRequest extends EntityRequest {
  reportId?: string | number | undefined;
}
export const fetchPerformanceAsync = async ({
  token,
  id,
  reportId,
}: PerformanceRequest): Promise<PerformanceResponse> => {
  return await executeFetch({
    token,
    path: `api/recommenders/PromotionsRecommenders/${id}/Performance/${
      reportId ?? "latest"
    }`,
  });
};

type PromotionOptimiser = components["schemas"]["PromotionOptimiser"];
export const fetchPromotionOptimiserAsync = async ({
  token,
  useInternalId,
  id,
}: EntityRequest): Promise<PromotionOptimiser> => {
  return await executeFetch({
    token,
    useInternalId,
    path: `api/recommenders/PromotionsRecommenders/${id}/Optimiser/`,
  });
};

interface SetAllPromotionOptimiserWeightsRequest extends EntityRequest {
  weights: components["schemas"]["UpdateWeightDto"][];
}
export const setAllPromotionOptimiserWeightsAsync = async ({
  token,
  useInternalId,
  id,
  weights,
}: SetAllPromotionOptimiserWeightsRequest): Promise<PromotionOptimiser> => {
  return await executeFetch({
    token,
    useInternalId,
    path: `api/recommenders/PromotionsRecommenders/${id}/Optimiser/Weights/`,
    method: "post",
    body: weights,
  });
};

interface SetPromotionOptimiserWeightRequest extends EntityRequest {
  weightId: number;
  weight: number;
}
export const setPromotionOptimiserWeightAsync = async ({
  token,
  useInternalId,
  id,
  weightId,
  weight,
}: SetPromotionOptimiserWeightRequest): Promise<PromotionOptimiser> => {
  return await executeFetch({
    token,
    useInternalId,
    path: `api/recommenders/PromotionsRecommenders/${id}/Optimiser/Weights/${weightId}`,
    method: "post",
    body: { weight },
  });
};

interface SetUseOptimiserRequest extends EntityRequest {
  useOptimiser: boolean;
}
export const setUseOptimiserAsync = async ({
  token,
  useInternalId,
  id,
  useOptimiser,
}: SetUseOptimiserRequest): Promise<PromotionOptimiser> => {
  return await executeFetch({
    token,
    useInternalId,
    path: `api/recommenders/PromotionsRecommenders/${id}/UseOptimiser`,
    method: "post",
    body: { useOptimiser },
  });
};
