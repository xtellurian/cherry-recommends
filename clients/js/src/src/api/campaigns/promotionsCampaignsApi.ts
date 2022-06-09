import { executeFetch } from "../client/apiClient";
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

const campaignApiName = "PromotionsCampaigns";

export const fetchPromotionsCampaignsAsync = async ({
  token,
  page,
}: PaginatedRequest) => {
  return await executeFetch({
    token,
    path: "api/campaigns/PromotionsCampaigns",
    page,
  });
};

export const fetchPromotionsCampaignAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/campaigns/PromotionsCampaigns/${id}`,
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
    path: `api/campaigns/PromotionsCampaigns/${id}/Recommendations`,
    page,
    pageSize,
  });
};

export const deletePromotionsCampaignAsync = async ({
  token,
  id,
}: DeleteRequest) => {
  return await executeFetch({
    path: `api/campaigns/PromotionsCampaigns/${id}`,
    token,
    method: "delete",
  });
};

interface CreatePromotionsCampaignRequest extends AuthenticatedRequest {
  payload: components["schemas"]["CreatePromotionsCampaign"];
}
export const createPromotionsCampaignAsync = async ({
  token,
  payload,
  useInternalId,
}: CreatePromotionsCampaignRequest) => {
  return await executeFetch({
    path: "api/campaigns/PromotionsCampaigns",
    token,
    method: "post",
    body: payload,
    query: { useInternalId },
  });
};

export const fetchPromotionsAsync = async ({ token, id }: EntityRequest) => {
  return await executeFetch({
    path: `api/campaigns/PromotionsCampaigns/${id}/Promotions`,
    token,
  });
};

type Audience = components["schemas"]["Audience"];
export const fetchAudienceAsync = async ({
  token,
  id,
}: EntityRequest): Promise<Audience> => {
  return await executeFetch({
    path: `api/campaigns/PromotionsCampaigns/${id}/Audience`,
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
    path: `api/campaigns/PromotionsCampaigns/${id}/Promotions`,
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
    path: `api/campaigns/PromotionsCampaigns/${id}/Promotions/${promotionId}`,
    token,
    method: "delete",
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
    path: `api/campaigns/PromotionsCampaigns/${id}/BaselinePromotion`,
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
    path: `api/campaigns/PromotionsCampaigns/${id}/BaselinePromotion`,
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
    campaignApiName,
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
    campaignApiName,
    id,
    token,
  });
};

interface InvokePromotionCampaignRequest extends EntityRequest {
  input: ModelInput;
}
export const invokePromotionsCampaignAsync = async ({
  token,
  id,
  input,
}: InvokePromotionCampaignRequest): Promise<PromotionsRecommendation> => {
  return await executeFetch({
    path: `api/campaigns/PromotionsCampaigns/${id}/Invoke`,
    token,
    method: "post",
    body: input,
  });
};

export const fetchInvokationLogsAsync = async ({
  id,
  token,
  page,
  pageSize,
}: PaginatedEntityRequest) => {
  return await il.fetchCampaignInvokationLogsAsync({
    campaignApiName,
    id,
    token,
    page,
    pageSize,
  });
};

export const fetchTargetVariablesAsync = async ({ id, token, name }: any) => {
  return await tv.fetchCampaignTargetVariableValuesAsync({
    campaignApiName,
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
  return await tv.createCampaignTargetVariableValueAsync({
    campaignApiName,
    id,
    token,
    targetVariableValue,
  });
};

interface CampaignSettings {
  requireConsumptionEvent: boolean;
  throwOnBadInput: boolean;
  recommendationCacheTime: string;
}
interface SetSettingsRequest extends EntityRequest {
  settings: CampaignSettings;
}
export const setSettingsAsync = async ({
  id,
  token,
  settings,
}: SetSettingsRequest) => {
  return await st.setSettingsAsync({
    campaignApiName,
    id,
    token,
    settings,
  });
};

type CreateArgument = components["schemas"]["CreateOrUpdateCampaignArgument"];
type Argument = components["schemas"]["CampaignArgument"];

interface SetArgumentsRequest extends EntityRequest {
  args: CreateArgument[];
}

export const fetchArgumentsAsync = async ({ id, token }: EntityRequest) => {
  return await ar.fetchArgumentsAsync({
    campaignApiName,
    id,
    token,
  });
};

export const setArgumentsAsync = async ({
  id,
  token,
  args,
}: SetArgumentsRequest): Promise<Argument[]> => {
  return await ar.setArgumentsAsync({
    campaignApiName,
    id,
    token,
    args,
  });
};

interface CreateChoosePromotionArgumentRule extends EntityRequest {
  rule: components["schemas"]["CreateChoosePromotionArgumentRuleDto"];
}
export const createChoosePromotionArgumentRuleAsync = async ({
  id,
  useInternalId,
  token,
  rule,
}: CreateChoosePromotionArgumentRule) => {
  return await ar.createChoosePromotionArgumentRuleAsync({
    campaignApiName,
    id,
    token,
    useInternalId,
    rule,
  });
};

interface UpdateChoosePromotionArgumentRule extends EntityRequest {
  rule: components["schemas"]["UpdateChoosePromotionArgumentRuleDto"];
  ruleId: number;
}
export const updateChoosePromotionArgumentRuleAsync = async ({
  id,
  useInternalId,
  token,
  rule,
  ruleId,
}: UpdateChoosePromotionArgumentRule) => {
  return await ar.updateChoosePromotionArgumentRuleAsync({
    campaignApiName,
    id,
    token,
    useInternalId,
    rule,
    ruleId,
  });
};

export const fetchChoosePromotionArgumentRulesAsync = async ({
  id,
  useInternalId,
  token,
}: CreateChoosePromotionArgumentRule) => {
  return await ar.fetchChoosePromotionArgumentRulesAsync({
    campaignApiName,
    id,
    token,
    useInternalId,
  });
};

interface CreateChooseSegmentArgumentRule extends EntityRequest {
  rule: components["schemas"]["CreateChooseSegmentArgumentRuleDto"];
}
export const createChooseSegmentArgumentRuleAsync = async ({
  id,
  useInternalId,
  token,
  rule,
}: CreateChooseSegmentArgumentRule) => {
  return await ar.createChooseSegmentArgumentRuleAsync({
    campaignApiName,
    id,
    token,
    useInternalId,
    rule,
  });
};

interface UpdateChooseSegmentArgumentRule extends EntityRequest {
  rule: components["schemas"]["UpdateChooseSegmentArgumentRuleDto"];
  ruleId: number;
}
export const updateChooseSegmentArgumentRuleAsync = async ({
  id,
  useInternalId,
  token,
  rule,
  ruleId,
}: UpdateChooseSegmentArgumentRule) => {
  return await ar.updateChooseSegmentArgumentRuleAsync({
    campaignApiName,
    id,
    token,
    useInternalId,
    rule,
    ruleId,
  });
};

export const fetchChooseSegmentArgumentRulesAsync = async ({
  id,
  useInternalId,
  token,
}: CreateChoosePromotionArgumentRule) => {
  return await ar.fetchChooseSegmentArgumentRulesAsync({
    campaignApiName,
    id,
    token,
    useInternalId,
  });
};

interface DeleteArgumentRule extends EntityRequest {
  ruleId: number;
}
export const deleteArgumentRuleAsync = async ({
  id,
  useInternalId,
  token,
  ruleId,
}: DeleteArgumentRule) => {
  return await ar.deleteArgumentRuleAsync({
    campaignApiName,
    id,
    token,
    useInternalId,
    ruleId,
  });
};

export const fetchDestinationsAsync = async ({ id, token }: EntityRequest) => {
  return await ds.fetchDestinationsAsync({
    campaignApiName,
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
    campaignApiName,
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
    campaignApiName,
    id,
    token,
    destinationId,
  });
};

export const fetchTriggerAsync = async ({ id, token }: EntityRequest) => {
  return await trig.fetchTriggerAsync({
    campaignApiName,
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
    campaignApiName,
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
    campaignApiName,
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
    campaignApiName,
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
    campaignApiName,
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
    campaignApiName,
    id,
    token,
    useInternalId,
    metricIds,
  });
};

type CampaignStatistics = components["schemas"]["CampaignStatistics"];
export const fetchStatisticsAsync = async ({
  id,
  token,
}: EntityRequest): Promise<CampaignStatistics> => {
  return await executeFetch({
    path: `api/campaigns/PromotionsCampaigns/${id}/Statistics`,
    token,
  });
};

export const fetchReportImageBlobUrlAsync = async ({
  id,
  token,
  useInternalId,
}: EntityRequest): Promise<any> => {
  return await ri.fetchReportImageBlobUrlAsync({
    campaignApiName,
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
    path: `api/campaigns/PromotionsCampaigns/${id}/Performance/${
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
    query: { useInternalId },
    path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/`,
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
    query: { useInternalId },
    path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Weights/`,
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
    query: { useInternalId },
    path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Weights/${weightId}`,
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
    query: { useInternalId },
    path: `api/campaigns/PromotionsCampaigns/${id}/UseOptimiser`,
    method: "post",
    body: { useOptimiser },
  });
};

export const fetchCampaignChannelsAsync = async ({
  id,
  token,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/campaigns/PromotionsCampaigns/${id}/Channels`,
    token,
  });
};

type Channel = components["schemas"]["ChannelBase"];
interface AddCampaignChannelRequest extends EntityRequest {
  channel: components["schemas"]["AddCampaignChannelDto"];
}
export const addCampaignChannelAsync = async ({
  token,
  id,
  channel,
}: AddCampaignChannelRequest): Promise<Channel> => {
  return await executeFetch({
    path: `api/campaigns/PromotionsCampaigns/${id}/Channels`,
    token,
    method: "post",
    body: channel,
  });
};

type PromotionsCampaigns = components["schemas"]["PromotionsCampaign"];
interface RemoveCampaignChannelRequest extends EntityRequest {
  channelId: number;
}
export const removeCampaignChannelAsync = async ({
  id,
  token,
  channelId,
}: RemoveCampaignChannelRequest): Promise<PromotionsCampaigns> => {
  return await executeFetch({
    path: `api/campaigns/PromotionsCampaigns/${id}/Channels/${channelId}`,
    token,
    method: "delete",
  });
};

interface RecommendationRequest extends EntityRequest {
  recommendationId: number;
}

export const fetchPromotionsRecommendationAsync = async ({
  token,
  recommendationId,
}: RecommendationRequest) => {
  return await executeFetch({
    token,
    path: `api/campaigns/PromotionsCampaigns/Recommendations/${recommendationId}`,
  });
};

interface OffersRequest extends PaginatedEntityRequest {
  page: number;
  offerState?: string;
}

export const fetchOffersAsync = async ({
  token,
  page,
  pageSize,
  id,
  offerState,
}: OffersRequest) => {
  return await executeFetch({
    token,
    path: `api/campaigns/PromotionsCampaigns/${id}/Offers`,
    page,
    pageSize,
    query: { offerState },
  });
};

type ARPOReport = components["schemas"]["ARPOReportDto"];
export const fetchARPOReportAsync = async ({
  token,
  id,
}: EntityRequest): Promise<ARPOReport> => {
  return await executeFetch({
    token,
    path: `api/campaigns/PromotionsCampaigns/${id}/ARPOReport`,
  });
};

type APVReport = components["schemas"]["APVReportDto"];
export const fetchAPVReportAsync = async ({
  token,
  id,
}: EntityRequest): Promise<APVReport> => {
  return await executeFetch({
    token,
    path: `api/campaigns/PromotionsCampaigns/${id}/APVReport`,
  });
};

type OfferConversionRateReport =
  components["schemas"]["OfferConversionRateReportDto"];
export const fetchOfferConversionRateReportAsync = async ({
  token,
  id,
}: EntityRequest): Promise<OfferConversionRateReport> => {
  return await executeFetch({
    token,
    path: `api/campaigns/PromotionsCampaigns/${id}/ConversionRateReport`,
  });
};
