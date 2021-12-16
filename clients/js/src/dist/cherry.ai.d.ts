declare function fetchUniqueActionNamesAsync({ token, page, term }: {
    token: any;
    page: any;
    term: any;
}): Promise<any>;
declare function fetchDistinctGroupsAsync({ token, page, term }: {
    token: any;
    page: any;
    term: any;
}): Promise<any>;

declare const actionsApi_d_fetchUniqueActionNamesAsync: typeof fetchUniqueActionNamesAsync;
declare const actionsApi_d_fetchDistinctGroupsAsync: typeof fetchDistinctGroupsAsync;
declare namespace actionsApi_d {
  export {
    actionsApi_d_fetchUniqueActionNamesAsync as fetchUniqueActionNamesAsync,
    actionsApi_d_fetchDistinctGroupsAsync as fetchDistinctGroupsAsync,
  };
}

interface AuthenticatedRequest {
    token: string;
}
interface PaginatedRequest extends AuthenticatedRequest {
    page: number;
}
interface PaginateResponse<T> {
    items: T[];
    pagination: {
        pageCount: number;
        totalItemCount: number;
        pageNumber: number;
        hasPreviousPage: boolean;
        hasNextPage: boolean;
        isFirstPage: boolean;
        isLastPage: boolean;
    };
}
interface EntityRequest extends AuthenticatedRequest {
    id: number | string;
    useInternalId: boolean | undefined;
}
interface EntitySearchRequest extends PaginatedRequest {
    searchTerm: string | undefined;
}
interface DeleteRequest extends AuthenticatedRequest {
    id: number | string;
}
interface DeleteResponse {
    id: number;
    resourceUrl: string;
    success: boolean;
}
interface SetpropertiesRequest extends EntityRequest {
    properties: any;
}
interface ModelInput {
    commonUserId: string;
    arguments: any | undefined;
}
interface Entity {
    id: number;
}
interface CommonEntity extends Entity {
    commonId: string;
    properties: any;
}
interface RecommendableItem extends CommonEntity {
}
interface CustomerEvent {
    commonUserId?: string | undefined;
    customerId: string;
    eventId: string;
    timestamp?: string | undefined;
    recommendationCorrelatorId?: number | undefined | null;
    sourceSystemId?: number | null | undefined;
    kind: "Custom" | "Behaviour" | "ConsumeRecommendation";
    eventType: string;
    properties?: any | null | undefined;
}
interface Customer extends CommonEntity {
}
interface ScoredItem {
    itemId: number | undefined;
    itemCommonId: string | undefined;
    commonId: string | undefined;
    item: RecommendableItem;
    score: number;
}
interface ItemsRecommendation {
    created: string;
    correlatorId: null;
    commonUserId: string;
    scoredItems: ScoredItem[];
    customer: Customer;
    trigger: string;
}

declare const fetchApiKeysAsync: ({ token, page }: PaginatedRequest) => Promise<any>;
declare enum ApiKeyType {
    Server = "Server",
    Web = "Web"
}
interface CreateApiKeyPayload {
    name: string;
    apiKeyType: ApiKeyType;
}
interface CreateApiKeyRequest extends AuthenticatedRequest {
    payload: CreateApiKeyPayload;
}
declare const createApiKeyAsync: ({ token, payload, }: CreateApiKeyRequest) => Promise<any>;
interface ExchangeApiKeyRequest {
    apiKey: string;
}
interface AccessTokenResponse {
    access_token: string;
}
declare const exchangeApiKeyAsync: ({ apiKey, }: ExchangeApiKeyRequest) => Promise<AccessTokenResponse>;
declare const deleteApiKeyAsync: ({ token, id }: DeleteRequest) => Promise<any>;

declare const apiKeyApi_d_fetchApiKeysAsync: typeof fetchApiKeysAsync;
declare const apiKeyApi_d_createApiKeyAsync: typeof createApiKeyAsync;
declare const apiKeyApi_d_exchangeApiKeyAsync: typeof exchangeApiKeyAsync;
declare const apiKeyApi_d_deleteApiKeyAsync: typeof deleteApiKeyAsync;
declare namespace apiKeyApi_d {
  export {
    apiKeyApi_d_fetchApiKeysAsync as fetchApiKeysAsync,
    apiKeyApi_d_createApiKeyAsync as createApiKeyAsync,
    apiKeyApi_d_exchangeApiKeyAsync as exchangeApiKeyAsync,
    apiKeyApi_d_deleteApiKeyAsync as deleteApiKeyAsync,
  };
}

declare function fetchCustomersAsync({ token, page, searchTerm }: {
    token: any;
    page: any;
    searchTerm: any;
}): Promise<any>;
declare function updateMergePropertiesAsync$1({ token, id, properties }: {
    token: any;
    id: any;
    properties: any;
}): Promise<any>;
declare function fetchCustomerAsync({ token, id, useInternalId }: {
    token: any;
    id: any;
    useInternalId: any;
}): Promise<any>;
declare function fetchUniqueCustomerActionGroupsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchLatestRecommendationsAsync$1({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchCustomerActionAsync({ token, id, category, actionName, }: {
    token: any;
    id: any;
    category: any;
    actionName: any;
}): Promise<any>;
declare function uploadUserDataAsync$1({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any[] | {
    error: any;
} | undefined>;
declare function createOrUpdateCustomerAsync({ token, customer, user, }: {
    token: any;
    customer: any;
    user: any;
}): Promise<any>;
declare function fetchCustomersActionsAsync({ token, page, id, revenueOnly, }: {
    token: any;
    page: any;
    id: any;
    revenueOnly: any;
}): Promise<any>;

declare const customersApi_d_fetchCustomersAsync: typeof fetchCustomersAsync;
declare const customersApi_d_fetchCustomerAsync: typeof fetchCustomerAsync;
declare const customersApi_d_fetchUniqueCustomerActionGroupsAsync: typeof fetchUniqueCustomerActionGroupsAsync;
declare const customersApi_d_fetchCustomerActionAsync: typeof fetchCustomerActionAsync;
declare const customersApi_d_createOrUpdateCustomerAsync: typeof createOrUpdateCustomerAsync;
declare const customersApi_d_fetchCustomersActionsAsync: typeof fetchCustomersActionsAsync;
declare namespace customersApi_d {
  export {
    customersApi_d_fetchCustomersAsync as fetchCustomersAsync,
    updateMergePropertiesAsync$1 as updateMergePropertiesAsync,
    customersApi_d_fetchCustomerAsync as fetchCustomerAsync,
    customersApi_d_fetchUniqueCustomerActionGroupsAsync as fetchUniqueCustomerActionGroupsAsync,
    fetchLatestRecommendationsAsync$1 as fetchLatestRecommendationsAsync,
    customersApi_d_fetchCustomerActionAsync as fetchCustomerActionAsync,
    uploadUserDataAsync$1 as uploadUserDataAsync,
    customersApi_d_createOrUpdateCustomerAsync as createOrUpdateCustomerAsync,
    customersApi_d_fetchCustomersActionsAsync as fetchCustomersActionsAsync,
  };
}

declare function fetchEventSummaryAsync({ token }: {
    token: any;
}): Promise<any>;
declare function fetchEventTimelineAsync({ token, kind, eventType }: {
    token: any;
    kind: any;
    eventType: any;
}): Promise<any>;
declare function fetchDashboardAsync({ token, scope }: {
    token: any;
    scope: any;
}): Promise<any>;
declare function fetchLatestActionsAsync({ token }: {
    token: any;
}): Promise<any>;

declare const dataSummaryApi_d_fetchEventSummaryAsync: typeof fetchEventSummaryAsync;
declare const dataSummaryApi_d_fetchEventTimelineAsync: typeof fetchEventTimelineAsync;
declare const dataSummaryApi_d_fetchDashboardAsync: typeof fetchDashboardAsync;
declare const dataSummaryApi_d_fetchLatestActionsAsync: typeof fetchLatestActionsAsync;
declare namespace dataSummaryApi_d {
  export {
    dataSummaryApi_d_fetchEventSummaryAsync as fetchEventSummaryAsync,
    dataSummaryApi_d_fetchEventTimelineAsync as fetchEventTimelineAsync,
    dataSummaryApi_d_fetchDashboardAsync as fetchDashboardAsync,
    dataSummaryApi_d_fetchLatestActionsAsync as fetchLatestActionsAsync,
  };
}

declare function fetchDeploymentConfigurationAsync({ token }: {
    token: any;
}): Promise<any>;

declare const deploymentApi_d_fetchDeploymentConfigurationAsync: typeof fetchDeploymentConfigurationAsync;
declare namespace deploymentApi_d {
  export {
    deploymentApi_d_fetchDeploymentConfigurationAsync as fetchDeploymentConfigurationAsync,
  };
}

declare const Custom = "Custom";
declare const Behaviour = "Behaviour";
declare const ConsumeRecommendation = "ConsumeRecommendation";
declare const fetchEventAsync: ({ id, token }: EntityRequest) => Promise<any>;
interface CreateEventRequest {
    apiKey?: string | undefined;
    token?: string | undefined;
    events: CustomerEvent[];
}
declare const createEventsAsync: ({ apiKey, token, events, }: CreateEventRequest) => Promise<any>;
declare const fetchCustomersEventsAsync: ({ token, id, useInternalId, }: EntityRequest) => Promise<any>;
declare const fetchTrackedUsersEventsAsync: ({ token, id, useInternalId, }: EntityRequest) => Promise<any>;
interface CreateRecommendationConsumedRequest {
    token: string;
    commonUserId: string | undefined;
    customerId: string;
    correlatorId: number;
}
declare const createRecommendationConsumedEventAsync: ({ token, commonUserId, customerId, correlatorId, }: CreateRecommendationConsumedRequest) => Promise<any>;

declare const eventsApi_d_Custom: typeof Custom;
declare const eventsApi_d_Behaviour: typeof Behaviour;
declare const eventsApi_d_ConsumeRecommendation: typeof ConsumeRecommendation;
declare const eventsApi_d_fetchEventAsync: typeof fetchEventAsync;
declare const eventsApi_d_createEventsAsync: typeof createEventsAsync;
declare const eventsApi_d_fetchCustomersEventsAsync: typeof fetchCustomersEventsAsync;
declare const eventsApi_d_fetchTrackedUsersEventsAsync: typeof fetchTrackedUsersEventsAsync;
declare const eventsApi_d_createRecommendationConsumedEventAsync: typeof createRecommendationConsumedEventAsync;
declare namespace eventsApi_d {
  export {
    eventsApi_d_Custom as Custom,
    eventsApi_d_Behaviour as Behaviour,
    eventsApi_d_ConsumeRecommendation as ConsumeRecommendation,
    eventsApi_d_fetchEventAsync as fetchEventAsync,
    eventsApi_d_createEventsAsync as createEventsAsync,
    eventsApi_d_fetchCustomersEventsAsync as fetchCustomersEventsAsync,
    eventsApi_d_fetchTrackedUsersEventsAsync as fetchTrackedUsersEventsAsync,
    eventsApi_d_createRecommendationConsumedEventAsync as createRecommendationConsumedEventAsync,
  };
}

declare function fetchEnvironmentsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function createEnvironmentAsync({ token, environment }: {
    token: any;
    environment: any;
}): Promise<any>;
declare function deleteEnvironmentAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare const setDefaultEnvironmentId$1: (e: any) => void;

declare const environmentsApi_d_fetchEnvironmentsAsync: typeof fetchEnvironmentsAsync;
declare const environmentsApi_d_createEnvironmentAsync: typeof createEnvironmentAsync;
declare const environmentsApi_d_deleteEnvironmentAsync: typeof deleteEnvironmentAsync;
declare namespace environmentsApi_d {
  export {
    environmentsApi_d_fetchEnvironmentsAsync as fetchEnvironmentsAsync,
    environmentsApi_d_createEnvironmentAsync as createEnvironmentAsync,
    environmentsApi_d_deleteEnvironmentAsync as deleteEnvironmentAsync,
    setDefaultEnvironmentId$1 as setDefaultEnvironmentId,
  };
}

declare function fetchFeatureGeneratorsAsync({ page, token }: {
    page: any;
    token: any;
}): Promise<any>;
declare function createFeatureGeneratorAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
declare function deleteFeatureGeneratorAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function manualTriggerFeatureGeneratorsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;

declare const featureGeneratorsApi_d_fetchFeatureGeneratorsAsync: typeof fetchFeatureGeneratorsAsync;
declare const featureGeneratorsApi_d_createFeatureGeneratorAsync: typeof createFeatureGeneratorAsync;
declare const featureGeneratorsApi_d_deleteFeatureGeneratorAsync: typeof deleteFeatureGeneratorAsync;
declare const featureGeneratorsApi_d_manualTriggerFeatureGeneratorsAsync: typeof manualTriggerFeatureGeneratorsAsync;
declare namespace featureGeneratorsApi_d {
  export {
    featureGeneratorsApi_d_fetchFeatureGeneratorsAsync as fetchFeatureGeneratorsAsync,
    featureGeneratorsApi_d_createFeatureGeneratorAsync as createFeatureGeneratorAsync,
    featureGeneratorsApi_d_deleteFeatureGeneratorAsync as deleteFeatureGeneratorAsync,
    featureGeneratorsApi_d_manualTriggerFeatureGeneratorsAsync as manualTriggerFeatureGeneratorsAsync,
  };
}

declare function fetchFeaturesAsync({ token, page, searchTerm }: {
    token: any;
    page: any;
    searchTerm: any;
}): Promise<any>;
declare function fetchFeatureAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchFeatureTrackedUsersAsync({ token, page, id }: {
    token: any;
    page: any;
    id: any;
}): Promise<any>;
declare function fetchFeatureTrackedUserFeaturesAsync({ token, page, id, }: {
    token: any;
    page: any;
    id: any;
}): Promise<any>;
declare function createFeatureAsync({ token, feature }: {
    token: any;
    feature: any;
}): Promise<any>;
declare function deleteFeatureAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchTrackedUserFeaturesAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchTrackedUserFeatureValuesAsync({ token, id, feature, version, }: {
    token: any;
    id: any;
    feature: any;
    version: any;
}): Promise<any>;
declare function fetchDestinationsAsync$2({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createDestinationAsync$2({ token, id, destination }: {
    token: any;
    id: any;
    destination: any;
}): Promise<any>;
declare function deleteDestinationAsync({ token, id, destinationId }: {
    token: any;
    id: any;
    destinationId: any;
}): Promise<any>;
declare function fetchGeneratorsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;

declare const featuresApi_d_fetchFeaturesAsync: typeof fetchFeaturesAsync;
declare const featuresApi_d_fetchFeatureAsync: typeof fetchFeatureAsync;
declare const featuresApi_d_fetchFeatureTrackedUsersAsync: typeof fetchFeatureTrackedUsersAsync;
declare const featuresApi_d_fetchFeatureTrackedUserFeaturesAsync: typeof fetchFeatureTrackedUserFeaturesAsync;
declare const featuresApi_d_createFeatureAsync: typeof createFeatureAsync;
declare const featuresApi_d_deleteFeatureAsync: typeof deleteFeatureAsync;
declare const featuresApi_d_fetchTrackedUserFeaturesAsync: typeof fetchTrackedUserFeaturesAsync;
declare const featuresApi_d_fetchTrackedUserFeatureValuesAsync: typeof fetchTrackedUserFeatureValuesAsync;
declare const featuresApi_d_deleteDestinationAsync: typeof deleteDestinationAsync;
declare const featuresApi_d_fetchGeneratorsAsync: typeof fetchGeneratorsAsync;
declare namespace featuresApi_d {
  export {
    featuresApi_d_fetchFeaturesAsync as fetchFeaturesAsync,
    featuresApi_d_fetchFeatureAsync as fetchFeatureAsync,
    featuresApi_d_fetchFeatureTrackedUsersAsync as fetchFeatureTrackedUsersAsync,
    featuresApi_d_fetchFeatureTrackedUserFeaturesAsync as fetchFeatureTrackedUserFeaturesAsync,
    featuresApi_d_createFeatureAsync as createFeatureAsync,
    featuresApi_d_deleteFeatureAsync as deleteFeatureAsync,
    featuresApi_d_fetchTrackedUserFeaturesAsync as fetchTrackedUserFeaturesAsync,
    featuresApi_d_fetchTrackedUserFeatureValuesAsync as fetchTrackedUserFeatureValuesAsync,
    fetchDestinationsAsync$2 as fetchDestinationsAsync,
    createDestinationAsync$2 as createDestinationAsync,
    featuresApi_d_deleteDestinationAsync as deleteDestinationAsync,
    featuresApi_d_fetchGeneratorsAsync as fetchGeneratorsAsync,
  };
}

declare function fetchIntegratedSystemsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function fetchIntegratedSystemAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function renameAsync({ token, id, name }: {
    token: any;
    id: any;
    name: any;
}): Promise<any>;
declare function createIntegratedSystemAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
declare function deleteIntegratedSystemAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchWebhookReceiversAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createWebhookReceiverAsync({ token, id, useSharedSecret, }: {
    token: any;
    id: any;
    useSharedSecret: any;
}): Promise<any>;

declare const integratedSystemsApi_d_fetchIntegratedSystemsAsync: typeof fetchIntegratedSystemsAsync;
declare const integratedSystemsApi_d_fetchIntegratedSystemAsync: typeof fetchIntegratedSystemAsync;
declare const integratedSystemsApi_d_renameAsync: typeof renameAsync;
declare const integratedSystemsApi_d_createIntegratedSystemAsync: typeof createIntegratedSystemAsync;
declare const integratedSystemsApi_d_deleteIntegratedSystemAsync: typeof deleteIntegratedSystemAsync;
declare const integratedSystemsApi_d_fetchWebhookReceiversAsync: typeof fetchWebhookReceiversAsync;
declare const integratedSystemsApi_d_createWebhookReceiverAsync: typeof createWebhookReceiverAsync;
declare namespace integratedSystemsApi_d {
  export {
    integratedSystemsApi_d_fetchIntegratedSystemsAsync as fetchIntegratedSystemsAsync,
    integratedSystemsApi_d_fetchIntegratedSystemAsync as fetchIntegratedSystemAsync,
    integratedSystemsApi_d_renameAsync as renameAsync,
    integratedSystemsApi_d_createIntegratedSystemAsync as createIntegratedSystemAsync,
    integratedSystemsApi_d_deleteIntegratedSystemAsync as deleteIntegratedSystemAsync,
    integratedSystemsApi_d_fetchWebhookReceiversAsync as fetchWebhookReceiversAsync,
    integratedSystemsApi_d_createWebhookReceiverAsync as createWebhookReceiverAsync,
  };
}

declare const fetchItemsRecommendersAsync: ({ token, page, }: PaginatedRequest) => Promise<any>;
declare const fetchItemsRecommenderAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface ItemsRecommendationsRequest extends EntityRequest {
    page: number;
}
declare const fetchItemsRecommendationsAsync: ({ token, page, id, }: ItemsRecommendationsRequest) => Promise<any>;
declare const deleteItemsRecommenderAsync: ({ token, id, }: DeleteRequest) => Promise<any>;
interface CreateItemsRecommenderPayload {
    commonId: string;
    name: string;
    itemIds: number[];
    defaultItemId: string;
    numberOfItemsToRecommend: number | undefined;
    useAutoAi: boolean;
}
interface CreateItemsRecommenderRequest extends AuthenticatedRequest {
    payload: CreateItemsRecommenderPayload;
}
declare const createItemsRecommenderAsync: ({ token, payload, }: CreateItemsRecommenderRequest) => Promise<any>;
declare const fetchItemsAsync$1: ({ token, id }: EntityRequest) => Promise<any>;
interface AddItemPayload {
    id: number | undefined;
    commonId: string | undefined;
}
interface AddItemRequest extends EntityRequest {
    item: AddItemPayload;
}
declare const addItemAsync: ({ token, id, item }: AddItemRequest) => Promise<any>;
interface RemoveItemRequest extends EntityRequest {
    itemId: string | number;
}
declare const removeItemAsync: ({ token, id, itemId, }: RemoveItemRequest) => Promise<any>;
interface SetDefaultItemRequest extends EntityRequest {
    itemId: string | number;
}
declare const setDefaultItemAsync: ({ token, id, itemId, }: SetDefaultItemRequest) => Promise<any>;
declare const getDefaultItemAsync: ({ token, id }: EntityRequest) => Promise<any>;
interface LinkRegisteredModelRequest extends EntityRequest {
    modelId: number;
}
declare const createLinkRegisteredModelAsync$1: ({ token, id, modelId, }: LinkRegisteredModelRequest) => Promise<any>;
declare const fetchLinkedRegisteredModelAsync$1: ({ token, id, }: EntityRequest) => Promise<any>;
interface InvokeItemRecommenderRequest extends EntityRequest {
    input: ModelInput;
}
declare const invokeItemsRecommenderAsync: ({ token, id, input, }: InvokeItemRecommenderRequest) => Promise<ItemsRecommendation>;
interface FetchInvokationLogsRequest extends EntityRequest {
    page: number;
}
declare const fetchInvokationLogsAsync$1: ({ id, token, page, }: FetchInvokationLogsRequest) => Promise<any>;
declare const fetchTargetVariablesAsync$1: ({ id, token, name }: any) => Promise<any>;
declare const createTargetVariableAsync$1: ({ id, token, targetVariableValue, }: any) => Promise<any>;
interface RecommenderSettings {
    requireConsumptionEvent: boolean;
    throwOnBadInput: boolean;
    recommendationCacheTime: string;
}
interface SetSettingsRequest extends EntityRequest {
    settings: RecommenderSettings;
}
declare const setSettingsAsync$1: ({ id, token, settings, }: SetSettingsRequest) => Promise<any>;
interface Argument {
    commonId: string;
    argumentType: "Numerical" | "Categorical";
    defaultValue: string | number;
    isRequired: boolean;
}
interface SetArgumentsRequest extends EntityRequest {
    args: Argument[];
}
declare const setArgumentsAsync$1: ({ id, token, args, }: SetArgumentsRequest) => Promise<any>;
declare const fetchDestinationsAsync$1: ({ id, token }: EntityRequest) => Promise<any>;
interface Destination {
    destinationType: "Webhook" | "SegmentSourceFunction" | "HubspotContactProperty";
    endpoint: string;
    integratedSystemId: number;
}
interface CreateDestinationRequest extends EntityRequest {
    destination: Destination;
}
declare const createDestinationAsync$1: ({ id, token, destination, }: CreateDestinationRequest) => Promise<any>;
interface RemoveDestinationRequest extends EntityRequest {
    destinationId: number;
}
declare const removeDestinationAsync$1: ({ id, token, destinationId, }: RemoveDestinationRequest) => Promise<any>;
declare const fetchTriggerAsync$1: ({ id, token }: EntityRequest) => Promise<any>;
interface Trigger {
    featuresChanged: any;
}
interface SetTriggerRequest extends EntityRequest {
    trigger: Trigger;
}
declare const setTriggerAsync$1: ({ id, token, trigger, }: SetTriggerRequest) => Promise<any>;
declare const fetchLearningFeaturesAsync$1: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningFeaturesRequest extends EntityRequest {
    featureIds: string[];
}
declare const setLearningFeaturesAsync$1: ({ id, token, featureIds, useInternalId, }: SetLearningFeaturesRequest) => Promise<any>;

declare const itemsRecommendersApi_d_fetchItemsRecommendersAsync: typeof fetchItemsRecommendersAsync;
declare const itemsRecommendersApi_d_fetchItemsRecommenderAsync: typeof fetchItemsRecommenderAsync;
declare const itemsRecommendersApi_d_fetchItemsRecommendationsAsync: typeof fetchItemsRecommendationsAsync;
declare const itemsRecommendersApi_d_deleteItemsRecommenderAsync: typeof deleteItemsRecommenderAsync;
declare const itemsRecommendersApi_d_createItemsRecommenderAsync: typeof createItemsRecommenderAsync;
declare const itemsRecommendersApi_d_addItemAsync: typeof addItemAsync;
declare const itemsRecommendersApi_d_removeItemAsync: typeof removeItemAsync;
declare const itemsRecommendersApi_d_setDefaultItemAsync: typeof setDefaultItemAsync;
declare const itemsRecommendersApi_d_getDefaultItemAsync: typeof getDefaultItemAsync;
declare const itemsRecommendersApi_d_invokeItemsRecommenderAsync: typeof invokeItemsRecommenderAsync;
declare namespace itemsRecommendersApi_d {
  export {
    itemsRecommendersApi_d_fetchItemsRecommendersAsync as fetchItemsRecommendersAsync,
    itemsRecommendersApi_d_fetchItemsRecommenderAsync as fetchItemsRecommenderAsync,
    itemsRecommendersApi_d_fetchItemsRecommendationsAsync as fetchItemsRecommendationsAsync,
    itemsRecommendersApi_d_deleteItemsRecommenderAsync as deleteItemsRecommenderAsync,
    itemsRecommendersApi_d_createItemsRecommenderAsync as createItemsRecommenderAsync,
    fetchItemsAsync$1 as fetchItemsAsync,
    itemsRecommendersApi_d_addItemAsync as addItemAsync,
    itemsRecommendersApi_d_removeItemAsync as removeItemAsync,
    itemsRecommendersApi_d_setDefaultItemAsync as setDefaultItemAsync,
    itemsRecommendersApi_d_getDefaultItemAsync as getDefaultItemAsync,
    createLinkRegisteredModelAsync$1 as createLinkRegisteredModelAsync,
    fetchLinkedRegisteredModelAsync$1 as fetchLinkedRegisteredModelAsync,
    itemsRecommendersApi_d_invokeItemsRecommenderAsync as invokeItemsRecommenderAsync,
    fetchInvokationLogsAsync$1 as fetchInvokationLogsAsync,
    fetchTargetVariablesAsync$1 as fetchTargetVariablesAsync,
    createTargetVariableAsync$1 as createTargetVariableAsync,
    setSettingsAsync$1 as setSettingsAsync,
    setArgumentsAsync$1 as setArgumentsAsync,
    fetchDestinationsAsync$1 as fetchDestinationsAsync,
    createDestinationAsync$1 as createDestinationAsync,
    removeDestinationAsync$1 as removeDestinationAsync,
    fetchTriggerAsync$1 as fetchTriggerAsync,
    setTriggerAsync$1 as setTriggerAsync,
    fetchLearningFeaturesAsync$1 as fetchLearningFeaturesAsync,
    setLearningFeaturesAsync$1 as setLearningFeaturesAsync,
  };
}

declare function fetchModelRegistrationsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function fetchModelRegistrationAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function deleteModelRegistrationAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createModelRegistrationAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
declare function invokeModelAsync({ token, modelId, features }: {
    token: any;
    modelId: any;
    features: any;
}): Promise<any>;

declare const modelRegistrationsApi_d_fetchModelRegistrationsAsync: typeof fetchModelRegistrationsAsync;
declare const modelRegistrationsApi_d_fetchModelRegistrationAsync: typeof fetchModelRegistrationAsync;
declare const modelRegistrationsApi_d_deleteModelRegistrationAsync: typeof deleteModelRegistrationAsync;
declare const modelRegistrationsApi_d_createModelRegistrationAsync: typeof createModelRegistrationAsync;
declare const modelRegistrationsApi_d_invokeModelAsync: typeof invokeModelAsync;
declare namespace modelRegistrationsApi_d {
  export {
    modelRegistrationsApi_d_fetchModelRegistrationsAsync as fetchModelRegistrationsAsync,
    modelRegistrationsApi_d_fetchModelRegistrationAsync as fetchModelRegistrationAsync,
    modelRegistrationsApi_d_deleteModelRegistrationAsync as deleteModelRegistrationAsync,
    modelRegistrationsApi_d_createModelRegistrationAsync as createModelRegistrationAsync,
    modelRegistrationsApi_d_invokeModelAsync as invokeModelAsync,
  };
}

declare function invokeGenericModelAsync({ token, id, input }: {
    token: any;
    id: any;
    input: any;
}): Promise<any>;

declare const index_d_invokeGenericModelAsync: typeof invokeGenericModelAsync;
declare namespace index_d {
  export {
    index_d_invokeGenericModelAsync as invokeGenericModelAsync,
  };
}

declare function fetchParametersAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function fetchParameterAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function deleteParameterAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createParameterAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;

declare const parametersApi_d_fetchParametersAsync: typeof fetchParametersAsync;
declare const parametersApi_d_fetchParameterAsync: typeof fetchParameterAsync;
declare const parametersApi_d_deleteParameterAsync: typeof deleteParameterAsync;
declare const parametersApi_d_createParameterAsync: typeof createParameterAsync;
declare namespace parametersApi_d {
  export {
    parametersApi_d_fetchParametersAsync as fetchParametersAsync,
    parametersApi_d_fetchParameterAsync as fetchParameterAsync,
    parametersApi_d_deleteParameterAsync as deleteParameterAsync,
    parametersApi_d_createParameterAsync as createParameterAsync,
  };
}

declare function fetchParameterSetRecommendersAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function fetchParameterSetRecommenderAsync({ token, id, searchTerm, }: {
    token: any;
    id: any;
    searchTerm: any;
}): Promise<any>;
declare function createParameterSetRecommenderAsync({ token, payload, }: {
    token: any;
    payload: any;
}): Promise<any>;
declare function deleteParameterSetRecommenderAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchParameterSetRecommendationsAsync({ token, page, id, }: {
    token: any;
    page: any;
    id: any;
}): Promise<any>;
declare function createLinkRegisteredModelAsync({ token, id, modelId, }: {
    token: any;
    id: any;
    modelId: any;
}): Promise<any>;
declare function fetchLinkedRegisteredModelAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function invokeParameterSetRecommenderAsync({ token, id, input, }: {
    token: any;
    id: any;
    input: any;
}): Promise<any>;
declare function fetchInvokationLogsAsync({ id, token, page }: {
    id: any;
    token: any;
    page: any;
}): Promise<any>;
declare function fetchTargetVariablesAsync({ id, token, name }: {
    id: any;
    token: any;
    name: any;
}): Promise<any>;
declare function createTargetVariableAsync({ id, token, targetVariableValue, }: {
    id: any;
    token: any;
    targetVariableValue: any;
}): Promise<any>;
declare function updateErrorHandlingAsync({ id, token, errorHandling, }: {
    id: any;
    token: any;
    errorHandling: any;
}): Promise<any>;
declare function setSettingsAsync({ id, token, settings }: {
    id: any;
    token: any;
    settings: any;
}): Promise<any>;
declare function fetchRecommenderTrackedUserActionsAsync({ id, token, page, revenueOnly, }: {
    id: any;
    token: any;
    page: any;
    revenueOnly: any;
}): Promise<any>;
declare function setArgumentsAsync({ id, token, args }: {
    id: any;
    token: any;
    args: any;
}): Promise<any>;
declare function fetchDestinationsAsync({ id, token }: {
    id: any;
    token: any;
}): Promise<any>;
declare function createDestinationAsync({ id, token, destination }: {
    id: any;
    token: any;
    destination: any;
}): Promise<any>;
declare function removeDestinationAsync({ id, token, destinationId }: {
    id: any;
    token: any;
    destinationId: any;
}): Promise<any>;
declare function fetchTriggerAsync({ id, token }: {
    id: any;
    token: any;
}): Promise<any>;
declare function setTriggerAsync({ id, token, trigger }: {
    id: any;
    token: any;
    trigger: any;
}): Promise<any>;
declare function fetchLearningFeaturesAsync({ id, token, useInternalId, }: {
    id: any;
    token: any;
    useInternalId: any;
}): Promise<any>;
declare function setLearningFeaturesAsync({ id, token, featureIds, useInternalId, }: {
    id: any;
    token: any;
    featureIds: any;
    useInternalId: any;
}): Promise<any>;

declare const parameterSetRecommendersApi_d_fetchParameterSetRecommendersAsync: typeof fetchParameterSetRecommendersAsync;
declare const parameterSetRecommendersApi_d_fetchParameterSetRecommenderAsync: typeof fetchParameterSetRecommenderAsync;
declare const parameterSetRecommendersApi_d_createParameterSetRecommenderAsync: typeof createParameterSetRecommenderAsync;
declare const parameterSetRecommendersApi_d_deleteParameterSetRecommenderAsync: typeof deleteParameterSetRecommenderAsync;
declare const parameterSetRecommendersApi_d_fetchParameterSetRecommendationsAsync: typeof fetchParameterSetRecommendationsAsync;
declare const parameterSetRecommendersApi_d_createLinkRegisteredModelAsync: typeof createLinkRegisteredModelAsync;
declare const parameterSetRecommendersApi_d_fetchLinkedRegisteredModelAsync: typeof fetchLinkedRegisteredModelAsync;
declare const parameterSetRecommendersApi_d_invokeParameterSetRecommenderAsync: typeof invokeParameterSetRecommenderAsync;
declare const parameterSetRecommendersApi_d_fetchInvokationLogsAsync: typeof fetchInvokationLogsAsync;
declare const parameterSetRecommendersApi_d_fetchTargetVariablesAsync: typeof fetchTargetVariablesAsync;
declare const parameterSetRecommendersApi_d_createTargetVariableAsync: typeof createTargetVariableAsync;
declare const parameterSetRecommendersApi_d_updateErrorHandlingAsync: typeof updateErrorHandlingAsync;
declare const parameterSetRecommendersApi_d_setSettingsAsync: typeof setSettingsAsync;
declare const parameterSetRecommendersApi_d_fetchRecommenderTrackedUserActionsAsync: typeof fetchRecommenderTrackedUserActionsAsync;
declare const parameterSetRecommendersApi_d_setArgumentsAsync: typeof setArgumentsAsync;
declare const parameterSetRecommendersApi_d_fetchDestinationsAsync: typeof fetchDestinationsAsync;
declare const parameterSetRecommendersApi_d_createDestinationAsync: typeof createDestinationAsync;
declare const parameterSetRecommendersApi_d_removeDestinationAsync: typeof removeDestinationAsync;
declare const parameterSetRecommendersApi_d_fetchTriggerAsync: typeof fetchTriggerAsync;
declare const parameterSetRecommendersApi_d_setTriggerAsync: typeof setTriggerAsync;
declare const parameterSetRecommendersApi_d_fetchLearningFeaturesAsync: typeof fetchLearningFeaturesAsync;
declare const parameterSetRecommendersApi_d_setLearningFeaturesAsync: typeof setLearningFeaturesAsync;
declare namespace parameterSetRecommendersApi_d {
  export {
    parameterSetRecommendersApi_d_fetchParameterSetRecommendersAsync as fetchParameterSetRecommendersAsync,
    parameterSetRecommendersApi_d_fetchParameterSetRecommenderAsync as fetchParameterSetRecommenderAsync,
    parameterSetRecommendersApi_d_createParameterSetRecommenderAsync as createParameterSetRecommenderAsync,
    parameterSetRecommendersApi_d_deleteParameterSetRecommenderAsync as deleteParameterSetRecommenderAsync,
    parameterSetRecommendersApi_d_fetchParameterSetRecommendationsAsync as fetchParameterSetRecommendationsAsync,
    parameterSetRecommendersApi_d_createLinkRegisteredModelAsync as createLinkRegisteredModelAsync,
    parameterSetRecommendersApi_d_fetchLinkedRegisteredModelAsync as fetchLinkedRegisteredModelAsync,
    parameterSetRecommendersApi_d_invokeParameterSetRecommenderAsync as invokeParameterSetRecommenderAsync,
    parameterSetRecommendersApi_d_fetchInvokationLogsAsync as fetchInvokationLogsAsync,
    parameterSetRecommendersApi_d_fetchTargetVariablesAsync as fetchTargetVariablesAsync,
    parameterSetRecommendersApi_d_createTargetVariableAsync as createTargetVariableAsync,
    parameterSetRecommendersApi_d_updateErrorHandlingAsync as updateErrorHandlingAsync,
    parameterSetRecommendersApi_d_setSettingsAsync as setSettingsAsync,
    parameterSetRecommendersApi_d_fetchRecommenderTrackedUserActionsAsync as fetchRecommenderTrackedUserActionsAsync,
    parameterSetRecommendersApi_d_setArgumentsAsync as setArgumentsAsync,
    parameterSetRecommendersApi_d_fetchDestinationsAsync as fetchDestinationsAsync,
    parameterSetRecommendersApi_d_createDestinationAsync as createDestinationAsync,
    parameterSetRecommendersApi_d_removeDestinationAsync as removeDestinationAsync,
    parameterSetRecommendersApi_d_fetchTriggerAsync as fetchTriggerAsync,
    parameterSetRecommendersApi_d_setTriggerAsync as setTriggerAsync,
    parameterSetRecommendersApi_d_fetchLearningFeaturesAsync as fetchLearningFeaturesAsync,
    parameterSetRecommendersApi_d_setLearningFeaturesAsync as setLearningFeaturesAsync,
  };
}

declare function setMetadataAsync({ token, metadata }: {
    token: any;
    metadata: any;
}): Promise<any>;
declare function getMetadataAsync({ token }: {
    token: any;
}): Promise<any>;

declare const profileApi_d_setMetadataAsync: typeof setMetadataAsync;
declare const profileApi_d_getMetadataAsync: typeof getMetadataAsync;
declare namespace profileApi_d {
  export {
    profileApi_d_setMetadataAsync as setMetadataAsync,
    profileApi_d_getMetadataAsync as getMetadataAsync,
  };
}

declare function fetchAuth0ConfigurationAsync(): Promise<any>;

declare const reactConfigApi_d_fetchAuth0ConfigurationAsync: typeof fetchAuth0ConfigurationAsync;
declare namespace reactConfigApi_d {
  export {
    reactConfigApi_d_fetchAuth0ConfigurationAsync as fetchAuth0ConfigurationAsync,
  };
}

declare const fetchItemsAsync: ({ token, page, searchTerm, }: EntitySearchRequest) => Promise<PaginateResponse<RecommendableItem>>;
declare const fetchItemAsync: ({ token, id }: EntityRequest) => Promise<any>;
interface CreateItemRequest extends AuthenticatedRequest {
    item: {
        commonId: string;
        name: string;
        listPrice: number;
        firectCost: number;
        description: number;
        properties: any;
    };
}
declare const createItemAsync: ({ token, item, }: CreateItemRequest) => Promise<RecommendableItem>;
interface UpdateItemRequest extends EntityRequest {
    item: {
        name: string;
        description: string;
        properties: any;
    };
}
declare const updateItemAsync: ({ token, id, item, }: UpdateItemRequest) => Promise<RecommendableItem>;
declare const deleteItemAsync: ({ token, id, }: DeleteRequest) => Promise<DeleteResponse>;
declare const getPropertiesAsync: ({ token, id }: EntityRequest) => Promise<any>;
declare const setPropertiesAsync: ({ token, id, properties, }: SetpropertiesRequest) => Promise<any>;

declare const recommendableItemsApi_d_fetchItemsAsync: typeof fetchItemsAsync;
declare const recommendableItemsApi_d_fetchItemAsync: typeof fetchItemAsync;
declare const recommendableItemsApi_d_createItemAsync: typeof createItemAsync;
declare const recommendableItemsApi_d_updateItemAsync: typeof updateItemAsync;
declare const recommendableItemsApi_d_deleteItemAsync: typeof deleteItemAsync;
declare const recommendableItemsApi_d_getPropertiesAsync: typeof getPropertiesAsync;
declare const recommendableItemsApi_d_setPropertiesAsync: typeof setPropertiesAsync;
declare namespace recommendableItemsApi_d {
  export {
    recommendableItemsApi_d_fetchItemsAsync as fetchItemsAsync,
    recommendableItemsApi_d_fetchItemAsync as fetchItemAsync,
    recommendableItemsApi_d_createItemAsync as createItemAsync,
    recommendableItemsApi_d_updateItemAsync as updateItemAsync,
    recommendableItemsApi_d_deleteItemAsync as deleteItemAsync,
    recommendableItemsApi_d_getPropertiesAsync as getPropertiesAsync,
    recommendableItemsApi_d_setPropertiesAsync as setPropertiesAsync,
  };
}

declare function fetchReportsAsync({ token }: {
    token: any;
}): Promise<any>;
declare function downloadReportAsync({ token, reportName }: {
    token: any;
    reportName: any;
}): Promise<any>;

declare const reportsApi_d_fetchReportsAsync: typeof fetchReportsAsync;
declare const reportsApi_d_downloadReportAsync: typeof downloadReportAsync;
declare namespace reportsApi_d {
  export {
    reportsApi_d_fetchReportsAsync as fetchReportsAsync,
    reportsApi_d_downloadReportAsync as downloadReportAsync,
  };
}

declare function fetchRewardSelectorsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function fetchRewardSelectorAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function deleteRewardSelectorAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createRewardSelectorAsync({ token, entity }: {
    token: any;
    entity: any;
}): Promise<any>;

declare const rewardSelectorsApi_d_fetchRewardSelectorsAsync: typeof fetchRewardSelectorsAsync;
declare const rewardSelectorsApi_d_fetchRewardSelectorAsync: typeof fetchRewardSelectorAsync;
declare const rewardSelectorsApi_d_deleteRewardSelectorAsync: typeof deleteRewardSelectorAsync;
declare const rewardSelectorsApi_d_createRewardSelectorAsync: typeof createRewardSelectorAsync;
declare namespace rewardSelectorsApi_d {
  export {
    rewardSelectorsApi_d_fetchRewardSelectorsAsync as fetchRewardSelectorsAsync,
    rewardSelectorsApi_d_fetchRewardSelectorAsync as fetchRewardSelectorAsync,
    rewardSelectorsApi_d_deleteRewardSelectorAsync as deleteRewardSelectorAsync,
    rewardSelectorsApi_d_createRewardSelectorAsync as createRewardSelectorAsync,
  };
}

declare function fetchSegmentsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function fetchSegmentAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createSegmentAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;

declare const segmentsApi_d_fetchSegmentsAsync: typeof fetchSegmentsAsync;
declare const segmentsApi_d_fetchSegmentAsync: typeof fetchSegmentAsync;
declare const segmentsApi_d_createSegmentAsync: typeof createSegmentAsync;
declare namespace segmentsApi_d {
  export {
    segmentsApi_d_fetchSegmentsAsync as fetchSegmentsAsync,
    segmentsApi_d_fetchSegmentAsync as fetchSegmentAsync,
    segmentsApi_d_createSegmentAsync as createSegmentAsync,
  };
}

declare function fetchTouchpointsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function fetchTouchpointAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createTouchpointMetadataAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
declare function fetchTrackedUserTouchpointsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchTrackedUsersInTouchpointAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createTrackedUserTouchpointAsync({ token, id, touchpointCommonId, payload, }: {
    token: any;
    id: any;
    touchpointCommonId: any;
    payload: any;
}): Promise<any>;
declare function fetchTrackedUserTouchpointValuesAsync({ token, id, touchpointCommonId, }: {
    token: any;
    id: any;
    touchpointCommonId: any;
}): Promise<any>;

declare const touchpointsApi_d_fetchTouchpointsAsync: typeof fetchTouchpointsAsync;
declare const touchpointsApi_d_fetchTouchpointAsync: typeof fetchTouchpointAsync;
declare const touchpointsApi_d_createTouchpointMetadataAsync: typeof createTouchpointMetadataAsync;
declare const touchpointsApi_d_fetchTrackedUserTouchpointsAsync: typeof fetchTrackedUserTouchpointsAsync;
declare const touchpointsApi_d_fetchTrackedUsersInTouchpointAsync: typeof fetchTrackedUsersInTouchpointAsync;
declare const touchpointsApi_d_createTrackedUserTouchpointAsync: typeof createTrackedUserTouchpointAsync;
declare const touchpointsApi_d_fetchTrackedUserTouchpointValuesAsync: typeof fetchTrackedUserTouchpointValuesAsync;
declare namespace touchpointsApi_d {
  export {
    touchpointsApi_d_fetchTouchpointsAsync as fetchTouchpointsAsync,
    touchpointsApi_d_fetchTouchpointAsync as fetchTouchpointAsync,
    touchpointsApi_d_createTouchpointMetadataAsync as createTouchpointMetadataAsync,
    touchpointsApi_d_fetchTrackedUserTouchpointsAsync as fetchTrackedUserTouchpointsAsync,
    touchpointsApi_d_fetchTrackedUsersInTouchpointAsync as fetchTrackedUsersInTouchpointAsync,
    touchpointsApi_d_createTrackedUserTouchpointAsync as createTrackedUserTouchpointAsync,
    touchpointsApi_d_fetchTrackedUserTouchpointValuesAsync as fetchTrackedUserTouchpointValuesAsync,
  };
}

declare const fetchTrackedUsersAsync: ({ token, page, searchTerm }: {
    token: any;
    page: any;
    searchTerm: any;
}) => Promise<any>;
declare const updateMergePropertiesAsync: ({ token, id, properties }: {
    token: any;
    id: any;
    properties: any;
}) => Promise<any>;
declare const fetchTrackedUserAsync: ({ token, id, useInternalId }: {
    token: any;
    id: any;
    useInternalId: any;
}) => Promise<any>;
declare const fetchUniqueTrackedUserActionGroupsAsync: ({ token, id }: {
    token: any;
    id: any;
}) => Promise<any>;
declare const fetchLatestRecommendationsAsync: ({ token, id }: {
    token: any;
    id: any;
}) => Promise<any>;
declare const fetchTrackedUserActionAsync: ({ token, id, category, actionName, }: {
    token: any;
    id: any;
    category: any;
    actionName: any;
}) => Promise<any>;
declare const uploadUserDataAsync: ({ token, payload }: {
    token: any;
    payload: any;
}) => Promise<any[] | {
    error: any;
} | undefined>;
declare const createOrUpdateTrackedUserAsync: ({ token, customer, user, }: {
    token: any;
    customer: any;
    user: any;
}) => Promise<any>;
declare const fetchTrackedUsersActionsAsync: ({ token, page, id, revenueOnly, }: {
    token: any;
    page: any;
    id: any;
    revenueOnly: any;
}) => Promise<any>;

declare const trackedUsersApi_d_fetchTrackedUsersAsync: typeof fetchTrackedUsersAsync;
declare const trackedUsersApi_d_updateMergePropertiesAsync: typeof updateMergePropertiesAsync;
declare const trackedUsersApi_d_fetchTrackedUserAsync: typeof fetchTrackedUserAsync;
declare const trackedUsersApi_d_fetchUniqueTrackedUserActionGroupsAsync: typeof fetchUniqueTrackedUserActionGroupsAsync;
declare const trackedUsersApi_d_fetchLatestRecommendationsAsync: typeof fetchLatestRecommendationsAsync;
declare const trackedUsersApi_d_fetchTrackedUserActionAsync: typeof fetchTrackedUserActionAsync;
declare const trackedUsersApi_d_uploadUserDataAsync: typeof uploadUserDataAsync;
declare const trackedUsersApi_d_createOrUpdateTrackedUserAsync: typeof createOrUpdateTrackedUserAsync;
declare const trackedUsersApi_d_fetchTrackedUsersActionsAsync: typeof fetchTrackedUsersActionsAsync;
declare namespace trackedUsersApi_d {
  export {
    trackedUsersApi_d_fetchTrackedUsersAsync as fetchTrackedUsersAsync,
    trackedUsersApi_d_updateMergePropertiesAsync as updateMergePropertiesAsync,
    trackedUsersApi_d_fetchTrackedUserAsync as fetchTrackedUserAsync,
    trackedUsersApi_d_fetchUniqueTrackedUserActionGroupsAsync as fetchUniqueTrackedUserActionGroupsAsync,
    trackedUsersApi_d_fetchLatestRecommendationsAsync as fetchLatestRecommendationsAsync,
    trackedUsersApi_d_fetchTrackedUserActionAsync as fetchTrackedUserActionAsync,
    trackedUsersApi_d_uploadUserDataAsync as uploadUserDataAsync,
    trackedUsersApi_d_createOrUpdateTrackedUserAsync as createOrUpdateTrackedUserAsync,
    trackedUsersApi_d_fetchTrackedUsersActionsAsync as fetchTrackedUsersActionsAsync,
  };
}

declare const setBaseUrl: (baseUrl: string) => void;

declare function setDefaultEnvironmentId(e: any): void;
declare function setDefaultApiKey(k: any): void;

declare function setErrorResponseHandler(errorHandler: any): void;
declare function handleErrorResponse(response: any): Promise<{
    error: any;
} | undefined>;
declare function handleErrorFetch(ex: any): void;
declare function setErrorFetchHandler(handler: any): void;

declare const errorHandling_d_setErrorResponseHandler: typeof setErrorResponseHandler;
declare const errorHandling_d_handleErrorResponse: typeof handleErrorResponse;
declare const errorHandling_d_handleErrorFetch: typeof handleErrorFetch;
declare const errorHandling_d_setErrorFetchHandler: typeof setErrorFetchHandler;
declare namespace errorHandling_d {
  export {
    errorHandling_d_setErrorResponseHandler as setErrorResponseHandler,
    errorHandling_d_handleErrorResponse as handleErrorResponse,
    errorHandling_d_handleErrorFetch as handleErrorFetch,
    errorHandling_d_setErrorFetchHandler as setErrorFetchHandler,
  };
}

export { actionsApi_d as actions, apiKeyApi_d as apiKeys, customersApi_d as customers, dataSummaryApi_d as dataSummary, deploymentApi_d as deployment, environmentsApi_d as environments, errorHandling_d as errorHandling, eventsApi_d as events, featureGeneratorsApi_d as featureGenerators, featuresApi_d as features, integratedSystemsApi_d as integratedSystems, itemsRecommendersApi_d as itemsRecommenders, modelRegistrationsApi_d as modelRegistrations, index_d as models, parameterSetRecommendersApi_d as parameterSetRecommenders, parametersApi_d as parameters, profileApi_d as profile, reactConfigApi_d as reactConfig, recommendableItemsApi_d as recommendableItems, reportsApi_d as reports, rewardSelectorsApi_d as rewardSelectors, segmentsApi_d as segments, setBaseUrl, setDefaultApiKey, setDefaultEnvironmentId, touchpointsApi_d as touchpoints, trackedUsersApi_d as trackedUsers };
