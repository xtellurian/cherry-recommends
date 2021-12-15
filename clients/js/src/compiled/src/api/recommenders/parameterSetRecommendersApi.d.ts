export function fetchParameterSetRecommendersAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
export function fetchParameterSetRecommenderAsync({ token, id, searchTerm, }: {
    token: any;
    id: any;
    searchTerm: any;
}): Promise<any>;
export function createParameterSetRecommenderAsync({ token, payload, }: {
    token: any;
    payload: any;
}): Promise<any>;
export function deleteParameterSetRecommenderAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function fetchParameterSetRecommendationsAsync({ token, page, id, }: {
    token: any;
    page: any;
    id: any;
}): Promise<any>;
export function createLinkRegisteredModelAsync({ token, id, modelId, }: {
    token: any;
    id: any;
    modelId: any;
}): Promise<any>;
export function fetchLinkedRegisteredModelAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export function invokeParameterSetRecommenderAsync({ token, id, input, }: {
    token: any;
    id: any;
    input: any;
}): Promise<any>;
export function fetchInvokationLogsAsync({ id, token, page }: {
    id: any;
    token: any;
    page: any;
}): Promise<any>;
export function fetchTargetVariablesAsync({ id, token, name }: {
    id: any;
    token: any;
    name: any;
}): Promise<any>;
export function createTargetVariableAsync({ id, token, targetVariableValue, }: {
    id: any;
    token: any;
    targetVariableValue: any;
}): Promise<any>;
export function updateErrorHandlingAsync({ id, token, errorHandling, }: {
    id: any;
    token: any;
    errorHandling: any;
}): Promise<any>;
export function setSettingsAsync({ id, token, settings }: {
    id: any;
    token: any;
    settings: any;
}): Promise<any>;
export function fetchRecommenderTrackedUserActionsAsync({ id, token, page, revenueOnly, }: {
    id: any;
    token: any;
    page: any;
    revenueOnly: any;
}): Promise<any>;
export function setArgumentsAsync({ id, token, args }: {
    id: any;
    token: any;
    args: any;
}): Promise<any>;
export function fetchDestinationsAsync({ id, token }: {
    id: any;
    token: any;
}): Promise<any>;
export function createDestinationAsync({ id, token, destination }: {
    id: any;
    token: any;
    destination: any;
}): Promise<any>;
export function removeDestinationAsync({ id, token, destinationId }: {
    id: any;
    token: any;
    destinationId: any;
}): Promise<any>;
export function fetchTriggerAsync({ id, token }: {
    id: any;
    token: any;
}): Promise<any>;
export function setTriggerAsync({ id, token, trigger }: {
    id: any;
    token: any;
    trigger: any;
}): Promise<any>;
export function fetchLearningFeaturesAsync({ id, token, useInternalId, }: {
    id: any;
    token: any;
    useInternalId: any;
}): Promise<any>;
export function setLearningFeaturesAsync({ id, token, featureIds, useInternalId, }: {
    id: any;
    token: any;
    featureIds: any;
    useInternalId: any;
}): Promise<any>;
