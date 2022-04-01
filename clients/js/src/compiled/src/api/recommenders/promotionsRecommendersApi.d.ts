import { components } from "../../model/api";
import { PaginatedRequest, EntityRequest, DeleteRequest, AuthenticatedRequest, ModelInput, PaginatedEntityRequest, PromotionsRecommendation } from "../../interfaces";
export declare const fetchPromotionsRecommendersAsync: ({ token, page, }: PaginatedRequest) => Promise<any>;
export declare const fetchPromotionsRecommenderAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface PromotionsRecommendationsRequest extends PaginatedEntityRequest {
    page: number;
}
export declare const fetchPromotionsRecommendationsAsync: ({ token, page, pageSize, id, }: PromotionsRecommendationsRequest) => Promise<any>;
export declare const deletePromotionsRecommenderAsync: ({ token, id, }: DeleteRequest) => Promise<any>;
interface CreatePromotionsRecommenderRequest extends AuthenticatedRequest {
    payload: components["schemas"]["CreatePromotionsRecommender"];
}
export declare const createPromotionsRecommenderAsync: ({ token, payload, useInternalId, }: CreatePromotionsRecommenderRequest) => Promise<any>;
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
interface InvokePromotionRecommenderRequest extends EntityRequest {
    input: ModelInput;
}
export declare const invokePromotionsRecommenderAsync: ({ token, id, input, }: InvokePromotionRecommenderRequest) => Promise<PromotionsRecommendation>;
interface FetchInvokationLogsRequest extends EntityRequest {
    page: number;
}
export declare const fetchInvokationLogsAsync: ({ id, token, page, }: FetchInvokationLogsRequest) => Promise<any>;
export declare const fetchTargetVariablesAsync: ({ id, token, name }: any) => Promise<any>;
export declare const createTargetVariableAsync: ({ id, token, targetVariableValue, }: any) => Promise<any>;
interface RecommenderSettings {
    requireConsumptionEvent: boolean;
    throwOnBadInput: boolean;
    recommendationCacheTime: string;
}
interface SetSettingsRequest extends EntityRequest {
    settings: RecommenderSettings;
}
export declare const setSettingsAsync: ({ id, token, settings, }: SetSettingsRequest) => Promise<any>;
interface Argument {
    commonId: string;
    argumentType: "Numerical" | "Categorical";
    defaultValue: string | number;
    isRequired: boolean;
}
interface SetArgumentsRequest extends EntityRequest {
    args: Argument[];
}
export declare const setArgumentsAsync: ({ id, token, args, }: SetArgumentsRequest) => Promise<any>;
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
declare type RecommenderStatistics = components["schemas"]["RecommenderStatistics"];
export declare const fetchStatisticsAsync: ({ id, token, }: EntityRequest) => Promise<RecommenderStatistics>;
export declare const fetchReportImageBlobUrlAsync: ({ id, token, useInternalId, }: EntityRequest) => Promise<string | void>;
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
export declare const fetchRecommenderChannelsAsync: ({ id, token, }: EntityRequest) => Promise<any>;
interface AddRecommenderChannelRequest extends EntityRequest {
    channel: components["schemas"]["AddRecommenderChannelDto"];
}
export declare const addRecommenderChannelAsync: ({ token, id, channel, }: AddRecommenderChannelRequest) => Promise<any>;
declare type PromotionsRecommenders = components["schemas"]["ItemsRecommender"];
interface RemoveRecommenderChannelRequest extends EntityRequest {
    channelId: number;
}
export declare const removeRecommenderChannelAsync: ({ id, token, channelId, }: RemoveRecommenderChannelRequest) => Promise<PromotionsRecommenders>;
export {};
