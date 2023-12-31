import { components } from "../../model/api";
import { PaginatedRequest, EntityRequest, DeleteRequest, AuthenticatedRequest, ModelInput, ItemsRecommendation, PaginatedEntityRequest } from "../../interfaces";
export declare const fetchItemsRecommendersAsync: ({ token, page, }: PaginatedRequest) => Promise<any>;
export declare const fetchItemsRecommenderAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface ItemsRecommendationsRequest extends PaginatedEntityRequest {
    page: number;
}
export declare const fetchItemsRecommendationsAsync: ({ token, page, pageSize, id, }: ItemsRecommendationsRequest) => Promise<any>;
export declare const deleteItemsRecommenderAsync: ({ token, id, }: DeleteRequest) => Promise<any>;
interface CreateItemsRecommenderRequest extends AuthenticatedRequest {
    payload: components["schemas"]["CreatePromotionsCampaign"];
}
export declare const createItemsRecommenderAsync: ({ token, payload, useInternalId, }: CreateItemsRecommenderRequest) => Promise<any>;
export declare const fetchItemsAsync: ({ token, id }: EntityRequest) => Promise<any>;
interface AddItemPayload {
    id: number | undefined;
    commonId: string | undefined;
}
interface AddItemRequest extends EntityRequest {
    item: AddItemPayload;
}
export declare const addItemAsync: ({ token, id, item }: AddItemRequest) => Promise<any>;
interface RemoveItemRequest extends EntityRequest {
    itemId: string | number;
}
export declare const removeItemAsync: ({ token, id, itemId, }: RemoveItemRequest) => Promise<any>;
interface SetBaselineItemRequest extends EntityRequest {
    itemId: string | number;
}
export declare const setBaselineItemAsync: ({ token, id, itemId, }: SetBaselineItemRequest) => Promise<any>;
export declare const setDefaultItemAsync: ({ token, id, itemId, }: SetBaselineItemRequest) => Promise<any>;
export declare const getBaselineItemAsync: ({ token, id }: EntityRequest) => Promise<any>;
export declare const getDefaultItemAsync: ({ token, id }: EntityRequest) => Promise<any>;
declare type LinkRegisteredModelRequest = EntityRequest & components["schemas"]["LinkModel"];
export declare const createLinkRegisteredModelAsync: ({ token, id, modelId, }: LinkRegisteredModelRequest) => Promise<any>;
export declare const fetchLinkedRegisteredModelAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface InvokeItemRecommenderRequest extends EntityRequest {
    input: ModelInput;
}
export declare const invokeItemsRecommenderAsync: ({ token, id, input, }: InvokeItemRecommenderRequest) => Promise<ItemsRecommendation>;
export declare const fetchInvokationLogsAsync: ({ id, token, page, pageSize, }: PaginatedEntityRequest) => Promise<any>;
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
declare type RecommenderStatistics = components["schemas"]["CampaignStatistics"];
export declare const fetchStatisticsAsync: ({ id, token, }: EntityRequest) => Promise<RecommenderStatistics>;
export declare const fetchReportImageBlobUrlAsync: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
declare type PerformanceResponse = components["schemas"]["ItemsRecommenderPerformanceReport"];
interface PerformanceRequest extends EntityRequest {
    reportId?: string | number | undefined;
}
export declare const fetchPerformanceAsync: ({ token, id, reportId, }: PerformanceRequest) => Promise<PerformanceResponse>;
export {};
