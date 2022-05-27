import { components } from "../../model/api";
import { AuthenticatedRequest, EntityRequest, EntitySearchRequest, PaginatedEntityRequest, PaginatedRequest } from "../../interfaces";
export declare const fetchParameterSetCampaignsAsync: ({ token, page, }: PaginatedRequest) => Promise<any>;
export declare const fetchParameterSetCampaignAsync: ({ token, id, searchTerm, }: EntitySearchRequest) => Promise<any>;
interface CreateParameterSetCampaignRequest extends AuthenticatedRequest {
    payload: {
        name: string;
        commonId: string;
        settings: components["schemas"]["CampaignSettingsDto"];
        parameters: string[];
        bounds: components["schemas"]["ParameterBounds"][];
        arguments: components["schemas"]["CreateOrUpdateCampaignArgument"][];
    };
}
export declare const createParameterSetCampaignAsync: ({ token, payload, }: CreateParameterSetCampaignRequest) => Promise<any>;
export declare const deleteParameterSetCampaignAsync: ({ token, id, }: EntityRequest) => Promise<any>;
export declare const fetchParameterSetRecommendationsAsync: ({ token, page, pageSize, id, }: PaginatedEntityRequest) => Promise<any>;
declare type LinkRegisteredModelRequest = EntityRequest & components["schemas"]["LinkModel"];
export declare const createLinkRegisteredModelAsync: ({ token, id, modelId, }: LinkRegisteredModelRequest) => Promise<any>;
export declare const fetchLinkedRegisteredModelAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface InvokeParameterSetCampaignRequest extends EntityRequest {
    input: components["schemas"]["ModelInputDto"];
}
export declare const invokeParameterSetCampaignAsync: ({ token, id, input, }: InvokeParameterSetCampaignRequest) => Promise<any>;
export declare const fetchInvokationLogsAsync: ({ id, token, page, pageSize, }: PaginatedEntityRequest) => Promise<any>;
export declare const fetchTargetVariablesAsync: ({ id, token, name }: any) => Promise<any>;
export declare const createTargetVariableAsync: ({ id, token, targetVariableValue, }: any) => Promise<any>;
interface SetSettingsRequest extends EntityRequest {
    settings: components["schemas"]["CampaignSettingsDto"];
}
export declare const setSettingsAsync: ({ id, token, settings, }: SetSettingsRequest) => Promise<any>;
declare type CreateArgument = components["schemas"]["CreateOrUpdateCampaignArgument"];
declare type Argument = components["schemas"]["CampaignArgument"];
interface SetArgumentsRequest extends EntityRequest {
    args: CreateArgument[];
}
export declare const fetchArgumentsAsync: ({ id, token }: EntityRequest) => Promise<any>;
export declare const setArgumentsAsync: ({ id, token, args, }: SetArgumentsRequest) => Promise<Argument[]>;
export declare const fetchDestinationsAsync: ({ id, token }: EntityRequest) => Promise<any>;
interface CreateDestinationRequest extends EntityRequest {
    destination: components["schemas"]["CreateDestinationDto"];
}
export declare const createDestinationAsync: ({ id, token, destination, }: CreateDestinationRequest) => Promise<any>;
interface RemoveDestinationRequest extends EntityRequest {
    destinationId: string | number;
}
export declare const removeDestinationAsync: ({ id, token, destinationId, }: RemoveDestinationRequest) => Promise<any>;
export declare const fetchTriggerAsync: ({ id, token }: EntityRequest) => Promise<any>;
interface SetTriggerRequest extends EntityRequest {
    trigger: components["schemas"]["SetTriggersDto"];
}
export declare const setTriggerAsync: ({ id, token, trigger, }: SetTriggerRequest) => Promise<any>;
export declare const fetchLearningFeaturesAsync: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningFeaturesRequest extends EntityRequest {
    featureIds: string[] | number[];
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
export {};
