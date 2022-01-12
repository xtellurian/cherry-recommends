import { components } from "../../model/api";
import { AuthenticatedRequest, EntityRequest, EntitySearchRequest, PaginatedEntityRequest, PaginatedRequest } from "../../interfaces";
export declare const fetchParameterSetRecommendersAsync: ({ token, page, }: PaginatedRequest) => Promise<any>;
export declare const fetchParameterSetRecommenderAsync: ({ token, id, searchTerm, }: EntitySearchRequest) => Promise<any>;
interface CreateParameterSetRecommenderRequest extends AuthenticatedRequest {
    payload: {
        name: string;
        commonId: string;
        settings: components["schemas"]["RecommenderSettingsDto"];
        parameters: string[];
        bounds: components["schemas"]["ParameterBounds"][];
        arguments: components["schemas"]["CreateOrUpdateRecommenderArgument"][];
    };
}
export declare const createParameterSetRecommenderAsync: ({ token, payload, }: CreateParameterSetRecommenderRequest) => Promise<any>;
export declare const deleteParameterSetRecommenderAsync: ({ token, id, }: EntityRequest) => Promise<any>;
export declare const fetchParameterSetRecommendationsAsync: ({ token, page, pageSize, id, }: PaginatedEntityRequest) => Promise<any>;
declare type LinkRegisteredModelRequest = EntityRequest & components["schemas"]["LinkModel"];
export declare const createLinkRegisteredModelAsync: ({ token, id, modelId, }: LinkRegisteredModelRequest) => Promise<any>;
export declare const fetchLinkedRegisteredModelAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface InvokeParameterSetRecommenderRequest extends EntityRequest {
    input: components["schemas"]["ModelInputDto"];
}
export declare const invokeParameterSetRecommenderAsync: ({ token, id, input, }: InvokeParameterSetRecommenderRequest) => Promise<any>;
export declare const fetchInvokationLogsAsync: ({ id, token, page, }: PaginatedEntityRequest) => Promise<any>;
export declare const fetchTargetVariablesAsync: ({ id, token, name }: any) => Promise<any>;
export declare const createTargetVariableAsync: ({ id, token, targetVariableValue, }: any) => Promise<any>;
interface SetSettingsRequest extends EntityRequest {
    settings: components["schemas"]["RecommenderSettingsDto"];
}
export declare const setSettingsAsync: ({ id, token, settings, }: SetSettingsRequest) => Promise<any>;
interface SetArgumentsRequest extends EntityRequest {
    args: components["schemas"]["CreateOrUpdateRecommenderArgument"][];
}
export declare const setArgumentsAsync: ({ id, token, args, }: SetArgumentsRequest) => Promise<any>;
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
declare type RecommenderStatistics = components["schemas"]["RecommenderStatistics"];
export declare const fetchStatisticsAsync: ({ id, token, }: EntityRequest) => Promise<RecommenderStatistics>;
export declare const fetchReportImageBlobUrlAsync: ({ id, token, useInternalId, }: EntityRequest) => Promise<RecommenderStatistics>;
export {};
