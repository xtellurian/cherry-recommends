import { components } from "../../model/api";
import { PaginatedRequest, EntityRequest, DeleteRequest, AuthenticatedRequest, ModelInput, PaginatedEntityRequest, PromotionsRecommendation } from "../../interfaces";
export declare const fetchPromotionsCampaignsAsync: ({ token, page, }: PaginatedRequest) => Promise<any>;
export declare const fetchPromotionsCampaignAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface PromotionsRecommendationsRequest extends PaginatedEntityRequest {
    page: number;
}
export declare const fetchPromotionsRecommendationsAsync: ({ token, page, pageSize, id, }: PromotionsRecommendationsRequest) => Promise<any>;
export declare const deletePromotionsCampaignAsync: ({ token, id, }: DeleteRequest) => Promise<any>;
interface CreatePromotionsCampaignRequest extends AuthenticatedRequest {
    payload: components["schemas"]["CreatePromotionsCampaign"];
}
export declare const createPromotionsCampaignAsync: ({ token, payload, useInternalId, }: CreatePromotionsCampaignRequest) => Promise<any>;
export declare const fetchPromotionsAsync: ({ token, id }: EntityRequest) => Promise<any>;
declare type Audience = components["schemas"]["Audience"];
export declare const fetchAudienceAsync: ({ token, id, }: EntityRequest) => Promise<Audience>;
interface AddPromotionPayload {
    id: number | undefined;
    commonId: string | undefined;
}
interface AddPromotionRequest extends EntityRequest {
    promotion: AddPromotionPayload;
}
export declare const addPromotionAsync: ({ token, id, promotion, }: AddPromotionRequest) => Promise<any>;
interface RemovePromotionRequest extends EntityRequest {
    promotionId: string | number;
}
export declare const removePromotionAsync: ({ token, id, promotionId, }: RemovePromotionRequest) => Promise<any>;
interface SetBaselinePromotionRequest extends EntityRequest {
    promotionId: string | number;
}
export declare const setBaselinePromotionAsync: ({ token, id, promotionId, }: SetBaselinePromotionRequest) => Promise<any>;
export declare const getBaselinePromotionAsync: ({ token, id, }: EntityRequest) => Promise<any>;
declare type LinkRegisteredModelRequest = EntityRequest & components["schemas"]["LinkModel"];
export declare const createLinkRegisteredModelAsync: ({ token, id, modelId, }: LinkRegisteredModelRequest) => Promise<any>;
export declare const fetchLinkedRegisteredModelAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface InvokePromotionCampaignRequest extends EntityRequest {
    input: ModelInput;
}
export declare const invokePromotionsCampaignAsync: ({ token, id, input, }: InvokePromotionCampaignRequest) => Promise<PromotionsRecommendation>;
export declare const fetchInvokationLogsAsync: ({ id, token, page, pageSize, }: PaginatedEntityRequest) => Promise<any>;
export declare const fetchTargetVariablesAsync: ({ id, token, name }: any) => Promise<any>;
export declare const createTargetVariableAsync: ({ id, token, targetVariableValue, }: any) => Promise<any>;
interface CampaignSettings {
    requireConsumptionEvent: boolean;
    throwOnBadInput: boolean;
    recommendationCacheTime: string;
}
interface SetSettingsRequest extends EntityRequest {
    settings: CampaignSettings;
}
export declare const setSettingsAsync: ({ id, token, settings, }: SetSettingsRequest) => Promise<any>;
declare type CreateArgument = components["schemas"]["CreateOrUpdateCampaignArgument"];
declare type Argument = components["schemas"]["CampaignArgument"];
interface SetArgumentsRequest extends EntityRequest {
    args: CreateArgument[];
}
export declare const fetchArgumentsAsync: ({ id, token }: EntityRequest) => Promise<any>;
export declare const setArgumentsAsync: ({ id, token, args, }: SetArgumentsRequest) => Promise<Argument[]>;
interface CreateChoosePromotionArgumentRule extends EntityRequest {
    rule: components["schemas"]["CreateChoosePromotionArgumentRuleDto"];
}
export declare const createChoosePromotionArgumentRuleAsync: ({ id, useInternalId, token, rule, }: CreateChoosePromotionArgumentRule) => Promise<any>;
interface UpdateChoosePromotionArgumentRule extends EntityRequest {
    rule: components["schemas"]["UpdateChoosePromotionArgumentRuleDto"];
    ruleId: number;
}
export declare const updateChoosePromotionArgumentRuleAsync: ({ id, useInternalId, token, rule, ruleId, }: UpdateChoosePromotionArgumentRule) => Promise<any>;
export declare const fetchChoosePromotionArgumentRulesAsync: ({ id, useInternalId, token, }: CreateChoosePromotionArgumentRule) => Promise<any>;
interface CreateChooseSegmentArgumentRule extends EntityRequest {
    rule: components["schemas"]["CreateChooseSegmentArgumentRuleDto"];
}
export declare const createChooseSegmentArgumentRuleAsync: ({ id, useInternalId, token, rule, }: CreateChooseSegmentArgumentRule) => Promise<any>;
interface UpdateChooseSegmentArgumentRule extends EntityRequest {
    rule: components["schemas"]["UpdateChooseSegmentArgumentRuleDto"];
    ruleId: number;
}
export declare const updateChooseSegmentArgumentRuleAsync: ({ id, useInternalId, token, rule, ruleId, }: UpdateChooseSegmentArgumentRule) => Promise<any>;
export declare const fetchChooseSegmentArgumentRulesAsync: ({ id, useInternalId, token, }: CreateChoosePromotionArgumentRule) => Promise<any>;
interface DeleteArgumentRule extends EntityRequest {
    ruleId: number;
}
export declare const deleteArgumentRuleAsync: ({ id, useInternalId, token, ruleId, }: DeleteArgumentRule) => Promise<any>;
export declare const fetchDestinationsAsync: ({ id, token }: EntityRequest) => Promise<any>;
interface Destination {
    destinationType: "Webhook" | "SegmentSourceFunction" | "HubspotContactProperty";
    endpoint: string;
    integratedSystemId: number;
}
interface CreateDestinationRequest extends EntityRequest {
    destination: Destination;
}
export declare const createDestinationAsync: ({ id, token, destination, }: CreateDestinationRequest) => Promise<any>;
interface RemoveDestinationRequest extends EntityRequest {
    destinationId: number;
}
export declare const removeDestinationAsync: ({ id, token, destinationId, }: RemoveDestinationRequest) => Promise<any>;
export declare const fetchTriggerAsync: ({ id, token }: EntityRequest) => Promise<any>;
interface Trigger {
    featuresChanged: any;
}
interface SetTriggerRequest extends EntityRequest {
    trigger: Trigger;
}
export declare const setTriggerAsync: ({ id, token, trigger, }: SetTriggerRequest) => Promise<any>;
export declare const fetchLearningFeaturesAsync: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningFeaturesRequest extends EntityRequest {
    featureIds: string[];
}
export declare const setLearningFeaturesAsync: ({ id, token, featureIds, useInternalId, }: SetLearningFeaturesRequest) => Promise<any>;
export declare const fetchLearningMetricsAsync: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningMetricsRequest extends EntityRequest {
    metricIds: string[];
}
export declare const setLearningMetricsAsync: ({ id, token, metricIds, useInternalId, }: SetLearningMetricsRequest) => Promise<any>;
declare type CampaignStatistics = components["schemas"]["CampaignStatistics"];
export declare const fetchStatisticsAsync: ({ id, token, }: EntityRequest) => Promise<CampaignStatistics>;
export declare const fetchReportImageBlobUrlAsync: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
declare type PerformanceResponse = components["schemas"]["ItemsRecommenderPerformanceReport"];
interface PerformanceRequest extends EntityRequest {
    reportId?: string | number | undefined;
}
export declare const fetchPerformanceAsync: ({ token, id, reportId, }: PerformanceRequest) => Promise<PerformanceResponse>;
declare type PromotionOptimiser = components["schemas"]["PromotionOptimiser"];
export declare const fetchPromotionOptimiserAsync: ({ token, useInternalId, id, }: EntityRequest) => Promise<PromotionOptimiser>;
interface SetAllPromotionOptimiserWeightsRequest extends EntityRequest {
    weights: components["schemas"]["UpdateWeightDto"][];
}
export declare const setAllPromotionOptimiserWeightsAsync: ({ token, useInternalId, id, weights, }: SetAllPromotionOptimiserWeightsRequest) => Promise<PromotionOptimiser>;
interface SetPromotionOptimiserWeightRequest extends EntityRequest {
    weightId: number;
    weight: number;
}
export declare const setPromotionOptimiserWeightAsync: ({ token, useInternalId, id, weightId, weight, }: SetPromotionOptimiserWeightRequest) => Promise<PromotionOptimiser>;
interface SetUseOptimiserRequest extends EntityRequest {
    useOptimiser: boolean;
}
export declare const setUseOptimiserAsync: ({ token, useInternalId, id, useOptimiser, }: SetUseOptimiserRequest) => Promise<PromotionOptimiser>;
export declare const fetchCampaignChannelsAsync: ({ id, token, }: EntityRequest) => Promise<any>;
declare type Channel = components["schemas"]["ChannelBase"];
interface AddCampaignChannelRequest extends EntityRequest {
    channel: components["schemas"]["AddCampaignChannelDto"];
}
export declare const addCampaignChannelAsync: ({ token, id, channel, }: AddCampaignChannelRequest) => Promise<Channel>;
declare type PromotionsCampaigns = components["schemas"]["PromotionsCampaign"];
interface RemoveCampaignChannelRequest extends EntityRequest {
    channelId: number;
}
export declare const removeCampaignChannelAsync: ({ id, token, channelId, }: RemoveCampaignChannelRequest) => Promise<PromotionsCampaigns>;
interface RecommendationRequest extends EntityRequest {
    recommendationId: number;
}
export declare const fetchPromotionsRecommendationAsync: ({ token, recommendationId, }: RecommendationRequest) => Promise<any>;
interface OffersRequest extends PaginatedEntityRequest {
    page: number;
    offerState?: string;
}
export declare const fetchOffersAsync: ({ token, page, pageSize, id, offerState, }: OffersRequest) => Promise<any>;
declare type ARPOReport = components["schemas"]["ARPOReportDto"];
export declare const fetchARPOReportAsync: ({ token, id, }: EntityRequest) => Promise<ARPOReport>;
declare type APVReport = components["schemas"]["APVReportDto"];
export declare const fetchAPVReportAsync: ({ token, id, }: EntityRequest) => Promise<APVReport>;
declare type OfferConversionRateReport = components["schemas"]["OfferConversionRateReportDto"];
export declare const fetchOfferConversionRateReportAsync: ({ token, id, }: EntityRequest) => Promise<OfferConversionRateReport>;
export {};
