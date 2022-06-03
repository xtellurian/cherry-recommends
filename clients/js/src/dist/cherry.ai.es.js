let storedBaseUrl = "";
const setBaseUrl = (baseUrl) => {
    storedBaseUrl = baseUrl.substr(-1) === "/" ? baseUrl.slice(0, -1) : baseUrl;
};
const getBaseUrl = () => storedBaseUrl;

const axios = require("axios");
let currentInstance = null;
let currentConfig = null;
const defaultConfig = {
    baseUrl: getBaseUrl(),
};
// sets the current config
const initialise = (config = defaultConfig) => {
    currentConfig = config;
    return axios.create({
        baseURL: config.baseUrl,
        timeout: config.timeout,
        headers: config.tenant ? { "x-tenant": config.tenant } : {},
        validateStatus: function (status) {
            // return status < 500; // Resolve only if the status code is less than 500
            return true; // always resolve the promise
        },
    });
};
const current = (config) => {
    // if current instance is null or undefined
    if (!currentInstance) {
        currentInstance = initialise(config);
    }
    else if (!config) {
        currentInstance = initialise();
    }
    // if config isn't exactly the same object
    else if (config !== currentConfig) {
        if (config.baseUrl !== (currentConfig === null || currentConfig === void 0 ? void 0 : currentConfig.baseUrl) ||
            config.tenant !== currentConfig.tenant ||
            config.timeout !== currentConfig.timeout) {
            // then create a new instance and config
            currentInstance = initialise(config);
        }
    }
    return currentInstance;
};

var axiosInstance = /*#__PURE__*/Object.freeze({
    __proto__: null,
    current: current
});

// tenant
let storedTenant = null;
const setTenant = (tenant) => {
    console.debug(`Setting tenant: ${tenant}`);
    storedTenant = tenant;
};
// environment
let defaltEnvironmentId = null;
const setDefaultEnvironmentId$1 = (e) => {
    defaltEnvironmentId = e;
};
let defaultApiKey = null;
const setDefaultApiKey = (k) => {
    defaultApiKey = k;
};
const defaultHeaders = {
    "Content-Type": "application/json",
};
const headers = (token, apiKey) => {
    let headers = { ...defaultHeaders };
    if (storedTenant) {
        headers["x-tenant"] = storedTenant;
    }
    if (token) {
        headers.Authorization = `Bearer ${token}`;
    }
    if (apiKey) {
        headers["x-api-key"] = `${apiKey}`; // ensure its a string
    }
    else if (defaultApiKey) {
        headers["x-api-key"] = `${defaultApiKey}`; // ensure its a string
    }
    if (defaltEnvironmentId) {
        headers["x-environment"] = defaltEnvironmentId;
    }
    return headers;
};

const defaultErrorResponseHandler = async (response) => {
    console.debug(response);
    // is not a promise
    if (response.status >= 500) {
        console.error(`Server responded: ${response.statusText}`);
        console.error(response.data);
        return { error: response.data };
    }
    else if (response.status >= 400) {
        console.warn(`Server responded: ${response.statusText}`);
        console.warn(response.data);
        throw response.data;
    }
};
let errorResponseHandler = defaultErrorResponseHandler;
const setErrorResponseHandler = (errorHandler) => {
    errorResponseHandler = errorHandler;
};
// this function is called in api.js functions.
const handleErrorResponse = async (response) => {
    console.debug("SDK is handling an error response");
    return await errorResponseHandler(response);
};

var errorHandling = /*#__PURE__*/Object.freeze({
    __proto__: null,
    setErrorResponseHandler: setErrorResponseHandler,
    handleErrorResponse: handleErrorResponse
});

const executeFetch = async ({ token, apiKey, path, page, pageSize, body, method, query, responseType, }) => {
    const baseUrl = getBaseUrl();
    const client = current({ baseUrl: baseUrl });
    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(query || {})) {
        if (key && value) {
            params.append(key, value);
        }
    }
    if (page) {
        params.append("p.page", `${page}`);
    }
    if (pageSize) {
        params.append("p.pageSize", `${pageSize}`);
    }
    if (apiKey) {
        params.append("apiKey", `${apiKey}`);
    }
    let response;
    try {
        response = await client({
            method,
            url: path,
            params,
            headers: headers(token, null),
            data: body,
            responseType: responseType,
        });
    }
    catch (ex) {
        // something failed. ther server responded outside of 2xx
        // if we always resolve the promise, then this is always true
        console.error(ex);
        throw ex;
    }
    // happy path
    if (response.status <= 299) {
        return response.data;
    }
    else {
        console.debug(response);
        return await handleErrorResponse(response);
    }
};

const fetchActivityFeedEntitiesAsync = async ({ token, page, }) => {
    return await executeFetch({
        path: "api/ActivityFeed",
        token,
        page,
    });
};

var activityFeedApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchActivityFeedEntitiesAsync: fetchActivityFeedEntitiesAsync
});

const fetchApiKeysAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/apiKeys",
        token,
        page,
    });
};
var ApiKeyType;
(function (ApiKeyType) {
    ApiKeyType["Server"] = "Server";
    ApiKeyType["Web"] = "Web";
})(ApiKeyType || (ApiKeyType = {}));
const createApiKeyAsync = async ({ token, payload, }) => {
    return await executeFetch({
        path: "api/apiKeys",
        token,
        method: "post",
        body: payload,
    });
};
const exchangeApiKeyAsync = async ({ apiKey, }) => {
    return await executeFetch({
        path: "api/apiKeys/exchange",
        method: "post",
        body: { apiKey },
    });
};
const deleteApiKeyAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/apiKeys/${id}`,
        token,
        method: "delete",
    });
};

var apiKeyApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchApiKeysAsync: fetchApiKeysAsync,
    createApiKeyAsync: createApiKeyAsync,
    exchangeApiKeyAsync: exchangeApiKeyAsync,
    deleteApiKeyAsync: deleteApiKeyAsync
});

const fetchBusinessesAsync = async ({ token, page, searchTerm, }) => {
    return await executeFetch({
        path: "api/Businesses",
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
const fetchBusinessAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}`,
        token,
    });
};
const deleteBusinessAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Businesses/${id}`,
        token,
        method: "delete",
    });
};
const createBusinessAsync = async ({ token, business, }) => {
    return await executeFetch({
        path: "api/Businesses",
        token,
        method: "post",
        body: business,
    });
};
const updateBusinessPropertiesAsync = async ({ token, id, properties, }) => {
    return await executeFetch({
        token,
        path: `api/Businesses/${id}/Properties`,
        method: "post",
        body: properties,
    });
};
const fetchBusinessMembersAsync = async ({ token, id, page, searchTerm, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/Members`,
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
const deleteBusinessMemberAsync = async ({ token, id, customerId, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/Members/${customerId}`,
        token,
        method: "delete",
    });
};
const addBusinessMemberAsync = async ({ token, id, customer, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/Members`,
        token,
        method: "post",
        body: customer,
    });
};
const fetchRecommendationsAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/businesses/${id}/recommendations`,
    });
};

var businessesApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchBusinessesAsync: fetchBusinessesAsync,
    fetchBusinessAsync: fetchBusinessAsync,
    deleteBusinessAsync: deleteBusinessAsync,
    createBusinessAsync: createBusinessAsync,
    updateBusinessPropertiesAsync: updateBusinessPropertiesAsync,
    fetchBusinessMembersAsync: fetchBusinessMembersAsync,
    deleteBusinessMemberAsync: deleteBusinessMemberAsync,
    addBusinessMemberAsync: addBusinessMemberAsync,
    fetchRecommendationsAsync: fetchRecommendationsAsync
});

const fetchChannelsAsync = async ({ token, page, }) => {
    return await executeFetch({
        path: "api/Channels",
        token,
        page,
    });
};
const createChannelAsync = async ({ token, channel, }) => {
    return await executeFetch({
        path: "api/Channels",
        token,
        method: "post",
        body: channel,
    });
};
const fetchChannelAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Channels/${id}`,
        token,
    });
};
const deleteChannelAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Channels/${id}`,
        token,
        method: "delete",
    });
};
const updateChannelEndpointAsync = async ({ token, id, endpoint, }) => {
    return await executeFetch({
        token,
        path: `api/Channels/${id}/Endpoint`,
        method: "post",
        body: endpoint,
    });
};
const updateChannelPropertiesAsync = async ({ token, id, properties, }) => {
    return await executeFetch({
        token,
        path: `api/Channels/${id}/WebProperties`,
        method: "post",
        body: properties,
    });
};
const updateEmailChannelTriggerAsync = async ({ token, id, listTrigger, }) => {
    return await executeFetch({
        token,
        path: `api/Channels/${id}/EmailTrigger`,
        method: "post",
        body: listTrigger,
    });
};

var channelsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchChannelsAsync: fetchChannelsAsync,
    createChannelAsync: createChannelAsync,
    fetchChannelAsync: fetchChannelAsync,
    deleteChannelAsync: deleteChannelAsync,
    updateChannelEndpointAsync: updateChannelEndpointAsync,
    updateChannelPropertiesAsync: updateChannelPropertiesAsync,
    updateEmailChannelTriggerAsync: updateEmailChannelTriggerAsync
});

/**
 * Returns an array with arrays of the given size.
 *
 * @param myArray {Array} array to split
 * @param chunk_size {Integer} Size of every group
 */
function chunkArray(myArray, chunk_size) {
    let index = 0;
    const arrayLength = myArray.length;
    const tempArray = [];
    for (index = 0; index < arrayLength; index += chunk_size) {
        const myChunk = myArray.slice(index, index + chunk_size);
        // Do something if you want with the group
        tempArray.push(myChunk);
    }
    return tempArray;
}

const MAX_ARRAY = 5000;
const basePath = "api/Customers";
const fetchCustomersAsync = async ({ token, page, searchTerm }) => {
    return await executeFetch({
        path: basePath,
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
const updateMergePropertiesAsync$1 = async ({ token, id, properties }) => {
    return await executeFetch({
        token,
        path: `${basePath}/${id}/properties`,
        method: "post",
        body: properties,
    });
};
const fetchCustomerAsync = async ({ token, id, useInternalId }) => {
    return await executeFetch({
        path: `${basePath}/${id}`,
        token,
        query: {
            useInternalId,
        },
    });
};
const fetchUniqueCustomerActionGroupsAsync = async ({ token, id }) => {
    return await executeFetch({
        token,
        path: `${basePath}/${id}/action-groups`,
    });
};
const fetchLatestRecommendationsAsync$1 = async ({ token, id }) => {
    return await executeFetch({
        token,
        path: `${basePath}/${id}/latest-recommendations`,
    });
};
const fetchCustomerActionAsync = async ({ token, id, category, actionName, }) => {
    return await executeFetch({
        path: `${basePath}/${id}/actions/${category}`,
        token,
        query: {
            actionName,
        },
    });
};
const uploadUserDataAsync$1 = async ({ token, payload }) => {
    const payloads = chunkArray(payload.users, MAX_ARRAY).map((users) => ({
        users,
    }));
    const responses = [];
    for (const p of payloads) {
        const response = await executeFetch({
            token,
            path: basePath,
            method: "put",
            body: p,
        });
        if (response.ok) {
            responses.push(await response.json());
        }
        else {
            return await handleErrorResponse(response);
        }
    }
    return responses;
};
const createOrUpdateCustomerAsync = async ({ token, customer, user, }) => {
    if (user) {
        console.warn("user is a deprecated property in createOrUpdateCustomerAsync(). use 'customer'.");
    }
    return await executeFetch({
        path: basePath,
        method: "post",
        body: customer || user,
        token,
    });
};
const fetchCustomersActionsAsync = async ({ token, page, id, revenueOnly, }) => {
    return await executeFetch({
        path: `${basePath}/${id}/Actions`,
        token,
        page,
        query: {
            revenueOnly: !!revenueOnly,
        },
    });
};
const setCustomerMetricAsync = async ({ token, id, metricId, useInternalId, value, }) => {
    return await executeFetch({
        path: `${basePath}/${id}/Metrics/${metricId}`,
        method: "post",
        token,
        query: {
            useInternalId,
        },
        body: { value },
    });
};
const deleteCustomerAsync$1 = async ({ token, id }) => {
    return await executeFetch({
        path: `${basePath}/${id}`,
        token,
        method: "delete",
    });
};
const fetchCustomerSegmentsAsync = async ({ token, id }) => {
    return await executeFetch({
        token,
        path: `${basePath}/${id}/segments`,
    });
};

var customersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchCustomersAsync: fetchCustomersAsync,
    updateMergePropertiesAsync: updateMergePropertiesAsync$1,
    fetchCustomerAsync: fetchCustomerAsync,
    fetchUniqueCustomerActionGroupsAsync: fetchUniqueCustomerActionGroupsAsync,
    fetchLatestRecommendationsAsync: fetchLatestRecommendationsAsync$1,
    fetchCustomerActionAsync: fetchCustomerActionAsync,
    uploadUserDataAsync: uploadUserDataAsync$1,
    createOrUpdateCustomerAsync: createOrUpdateCustomerAsync,
    fetchCustomersActionsAsync: fetchCustomersActionsAsync,
    setCustomerMetricAsync: setCustomerMetricAsync,
    deleteCustomerAsync: deleteCustomerAsync$1,
    fetchCustomerSegmentsAsync: fetchCustomerSegmentsAsync
});

const fetchEventSummaryAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/datasummary/events",
        token,
    });
};
const fetchEventKindNamesAsync = async ({ token }) => {
    return await executeFetch({
        path: `api/datasummary/event-kind-names`,
        token,
    });
};
const fetchEventKindSummaryAsync = async ({ token, kind }) => {
    return await executeFetch({
        path: `api/datasummary/event-kind/${kind}`,
        token,
    });
};
const fetchEventTimelineAsync = async ({ token, kind, eventType }) => {
    return await executeFetch({
        path: `api/datasummary/events/timeline/${kind}/${eventType}`,
        token,
    });
};
const fetchGeneralSummaryAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/DataSummary/GeneralSummary",
        token,
    });
};
const fetchLatestActionsAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/datasummary/actions",
        token,
    });
};

var dataSummaryApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchEventSummaryAsync: fetchEventSummaryAsync,
    fetchEventKindNamesAsync: fetchEventKindNamesAsync,
    fetchEventKindSummaryAsync: fetchEventKindSummaryAsync,
    fetchEventTimelineAsync: fetchEventTimelineAsync,
    fetchGeneralSummaryAsync: fetchGeneralSummaryAsync,
    fetchLatestActionsAsync: fetchLatestActionsAsync
});

const fetchDeploymentConfigurationAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/deployment/configuration",
        token,
    });
};

var deploymentApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchDeploymentConfigurationAsync: fetchDeploymentConfigurationAsync
});

const fetchEventAsync = async ({ id, token }) => {
    return await executeFetch({
        token,
        path: `api/events/${id}`,
    });
};
const eventKinds = {
    custom: "custom",
    propertyUpdate: "propertyUpdate",
    behaviour: "behaviour",
    pageView: "pageView",
    identify: "identify",
    consumeRecommendation: "consumeRecommendation",
    addToBusiness: "addToBusiness",
    purchase: "purchase",
    usePromotion: "usePromotion",
    promotionPresented: "promotionPresented",
};
const createEventsAsync = async ({ apiKey, token, events, }) => {
    return await executeFetch({
        path: "api/events",
        method: "post",
        token,
        apiKey,
        body: events,
    });
};
const fetchCustomersEventsAsync = async ({ token, id, page, pageSize, useInternalId, }) => {
    return await executeFetch({
        path: `api/Customers/${id}/events`,
        token,
        page,
        pageSize,
        query: {
            useInternalId,
        },
    });
};
const fetchTrackedUsersEventsAsync = fetchCustomersEventsAsync;
// useful extension methods to create certain event kinds
const createRecommendationConsumedEventAsync = async ({ token, commonUserId, customerId, correlatorId, properties, }) => {
    const payload = {
        commonUserId,
        customerId,
        eventId: `recommendation-${correlatorId}-${new Date().getTime()}`,
        recommendationCorrelatorId: correlatorId,
        kind: "consumeRecommendation",
        eventType: "generated",
        properties,
    };
    return await createEventsAsync({ token, events: [payload] });
};
const fetchBusinessEventsAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/events`,
        token,
    });
};

var eventsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchEventAsync: fetchEventAsync,
    eventKinds: eventKinds,
    createEventsAsync: createEventsAsync,
    fetchCustomersEventsAsync: fetchCustomersEventsAsync,
    fetchTrackedUsersEventsAsync: fetchTrackedUsersEventsAsync,
    createRecommendationConsumedEventAsync: createRecommendationConsumedEventAsync,
    fetchBusinessEventsAsync: fetchBusinessEventsAsync
});

const fetchEnvironmentsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/Environments",
        token,
        page,
    });
};
const createEnvironmentAsync = async ({ token, environment }) => {
    return await executeFetch({
        path: "api/Environments",
        token,
        method: "post",
        body: environment,
    });
};
const deleteEnvironmentAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Environments/${id}`,
        token,
        method: "delete",
    });
};
const setDefaultEnvironmentId = setDefaultEnvironmentId$1;

var environmentsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchEnvironmentsAsync: fetchEnvironmentsAsync,
    createEnvironmentAsync: createEnvironmentAsync,
    deleteEnvironmentAsync: deleteEnvironmentAsync,
    setDefaultEnvironmentId: setDefaultEnvironmentId
});

console.warn("Deprecation Notice: Features are replaced by Metrics.");
const fetchFeatureGeneratorsAsync = async ({ page, token }) => {
    return await executeFetch({
        path: "api/FeatureGenerators",
        token,
        page,
    });
};
const createFeatureGeneratorAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/FeatureGenerators",
        token,
        method: "post",
        body: payload,
    });
};
const deleteFeatureGeneratorAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/FeatureGenerators/${id}`,
        token,
        method: "delete",
    });
};
const manualTriggerFeatureGeneratorsAsync = async ({ token, id }) => {
    return await executeFetch({
        token,
        path: `api/FeatureGenerators/${id}/Trigger`,
        method: "post",
        body: {},
    });
};

var featureGeneratorsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchFeatureGeneratorsAsync: fetchFeatureGeneratorsAsync,
    createFeatureGeneratorAsync: createFeatureGeneratorAsync,
    deleteFeatureGeneratorAsync: deleteFeatureGeneratorAsync,
    manualTriggerFeatureGeneratorsAsync: manualTriggerFeatureGeneratorsAsync
});

console.warn("Deprecation Notice: Feature Generators are replaced by Metric Generators.");
const fetchFeaturesAsync = async ({ token, page, searchTerm }) => {
    return await executeFetch({
        path: "api/Features",
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
const fetchFeatureAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/features/${id}`,
        token,
    });
};
const fetchFeatureTrackedUsersAsync = async ({ token, page, id }) => {
    return await executeFetch({
        path: `api/Features/${id}/TrackedUsers`,
        token,
        page,
    });
};
const fetchFeatureTrackedUserFeaturesAsync = async ({ token, page, id, }) => {
    return await executeFetch({
        path: `api/Features/${id}/TrackedUserFeatures`,
        token,
        page,
    });
};
const createFeatureAsync = async ({ token, feature }) => {
    return await executeFetch({
        path: "api/Features",
        token,
        method: "post",
        body: feature,
    });
};
const deleteFeatureAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/features/${id}`,
        token,
        method: "delete",
    });
};
const fetchTrackedUserFeaturesAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/TrackedUsers/${id}/features`,
        token,
    });
};
const fetchTrackedUserFeatureValuesAsync = async ({ token, id, feature, version, }) => {
    return await executeFetch({
        path: `api/TrackedUsers/${id}/features/${feature}`,
        token,
        query: {
            version,
        },
    });
};
const fetchDestinationsAsync$8 = async ({ token, id }) => {
    return await executeFetch({
        path: `api/features/${id}/Destinations`,
        token,
    });
};
const createDestinationAsync$8 = async ({ token, id, destination }) => {
    return await executeFetch({
        path: `api/features/${id}/Destinations`,
        token,
        method: "post",
        body: destination,
    });
};
const deleteDestinationAsync$1 = async ({ token, id, destinationId }) => {
    return await executeFetch({
        path: `api/features/${id}/Destinations/${destinationId}`,
        token,
        method: "delete",
    });
};
const fetchGeneratorsAsync$1 = async ({ token, id }) => {
    return await executeFetch({
        path: `api/features/${id}/Generators`,
        token,
    });
};

var featuresApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchFeaturesAsync: fetchFeaturesAsync,
    fetchFeatureAsync: fetchFeatureAsync,
    fetchFeatureTrackedUsersAsync: fetchFeatureTrackedUsersAsync,
    fetchFeatureTrackedUserFeaturesAsync: fetchFeatureTrackedUserFeaturesAsync,
    createFeatureAsync: createFeatureAsync,
    deleteFeatureAsync: deleteFeatureAsync,
    fetchTrackedUserFeaturesAsync: fetchTrackedUserFeaturesAsync,
    fetchTrackedUserFeatureValuesAsync: fetchTrackedUserFeatureValuesAsync,
    fetchDestinationsAsync: fetchDestinationsAsync$8,
    createDestinationAsync: createDestinationAsync$8,
    deleteDestinationAsync: deleteDestinationAsync$1,
    fetchGeneratorsAsync: fetchGeneratorsAsync$1
});

const fetchMetricsAsync = async ({ token, page, scope, searchTerm, }) => {
    return await executeFetch({
        path: "api/Metrics",
        token,
        page,
        query: {
            "q.term": searchTerm,
            "q.scope": scope,
        },
    });
};
const fetchMetricAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}`,
        token,
    });
};
const fetchMetricCustomersAsync = async ({ token, page, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Customers`,
        token,
        page,
    });
};
const fetchMetricCustomerMetricsAsync = async ({ token, page, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/CustomerMetrics`,
        token,
        page,
    });
};
const createMetricAsync = async ({ token, metric, }) => {
    return await executeFetch({
        path: "api/Metrics",
        token,
        method: "post",
        body: metric,
    });
};
const deleteMetricAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Metrics/${id}`,
        token,
        method: "delete",
    });
};
const fetchCustomersMetricsAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Customers/${id}/Metrics`,
        token,
    });
};
const fetchCustomersMetricAsync = async ({ token, id, metricId, version, }) => {
    return await executeFetch({
        path: `api/Customers/${id}/Metrics/${metricId}`,
        token,
        query: {
            version,
        },
    });
};
const fetchAggregateMetricValuesNumericAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/AggregateMetricValuesNumeric`,
        token,
    });
};
const fetchAggregateMetricValuesStringAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/AggregateMetricValuesString`,
        token,
    });
};
const fetchDestinationsAsync$7 = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Destinations`,
        token,
    });
};
const fetchExportCustomers = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/ExportCustomers`,
        token,
        responseType: "blob",
    });
};
const fetchMetricBinValuesNumericAsync = async ({ token, id, binCount, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/NumericMetricBinValues`,
        token,
        query: {
            binCount,
        },
    });
};
const fetchMetricBinValuesStringAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/CategoricalMetricBinValues`,
        token,
    });
};
const createDestinationAsync$7 = async ({ token, id, destination, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Destinations`,
        token,
        method: "post",
        body: destination,
    });
};
const deleteDestinationAsync = async ({ token, id, destinationId, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Destinations/${destinationId}`,
        token,
        method: "delete",
    });
};
const fetchGeneratorsAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Generators`,
        token,
    });
};
const fetchBusinessMetricsAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/Metrics`,
        token,
    });
};
const fetchBusinessMetricAsync = async ({ token, id, metricId, version, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/Metrics/${metricId}`,
        token,
        query: {
            version,
        },
    });
};

var metricsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchMetricsAsync: fetchMetricsAsync,
    fetchMetricAsync: fetchMetricAsync,
    fetchMetricCustomersAsync: fetchMetricCustomersAsync,
    fetchMetricCustomerMetricsAsync: fetchMetricCustomerMetricsAsync,
    createMetricAsync: createMetricAsync,
    deleteMetricAsync: deleteMetricAsync,
    fetchCustomersMetricsAsync: fetchCustomersMetricsAsync,
    fetchCustomersMetricAsync: fetchCustomersMetricAsync,
    fetchAggregateMetricValuesNumericAsync: fetchAggregateMetricValuesNumericAsync,
    fetchAggregateMetricValuesStringAsync: fetchAggregateMetricValuesStringAsync,
    fetchDestinationsAsync: fetchDestinationsAsync$7,
    fetchExportCustomers: fetchExportCustomers,
    fetchMetricBinValuesNumericAsync: fetchMetricBinValuesNumericAsync,
    fetchMetricBinValuesStringAsync: fetchMetricBinValuesStringAsync,
    createDestinationAsync: createDestinationAsync$7,
    deleteDestinationAsync: deleteDestinationAsync,
    fetchGeneratorsAsync: fetchGeneratorsAsync,
    fetchBusinessMetricsAsync: fetchBusinessMetricsAsync,
    fetchBusinessMetricAsync: fetchBusinessMetricAsync
});

const fetchMetricGeneratorsAsync = async ({ page, token, }) => {
    return await executeFetch({
        path: "api/MetricGenerators",
        token,
        page,
    });
};
const createMetricGeneratorAsync = async ({ token, generator, }) => {
    return await executeFetch({
        path: "api/MetricGenerators",
        token,
        method: "post",
        body: generator,
    });
};
const deleteMetricGeneratorAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/MetricGenerators/${id}`,
        token,
        method: "delete",
    });
};
const manualTriggerMetricGeneratorsAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/MetricGenerators/${id}/Trigger`,
        method: "post",
        body: {},
    });
};

var metricGeneratorsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchMetricGeneratorsAsync: fetchMetricGeneratorsAsync,
    createMetricGeneratorAsync: createMetricGeneratorAsync,
    deleteMetricGeneratorAsync: deleteMetricGeneratorAsync,
    manualTriggerMetricGeneratorsAsync: manualTriggerMetricGeneratorsAsync
});

const fetchIntegratedSystemsAsync = async ({ token, page, systemType, }) => {
    return await executeFetch({
        path: "api/IntegratedSystems",
        token,
        page,
        query: {
            "q.scope": systemType,
        },
    });
};
const fetchIntegratedSystemAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/IntegratedSystems/${id}`,
        token,
    });
};
const renameAsync = async ({ token, id, name }) => {
    return await executeFetch({
        path: `api/integratedSystems/${id}/name`,
        token,
        method: "post",
        query: {
            name,
        },
    });
};
const createIntegratedSystemAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/IntegratedSystems",
        token,
        method: "post",
        body: payload,
    });
};
const deleteIntegratedSystemAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/integratedSystems/${id}`,
        token,
        method: "delete",
    });
};
const fetchWebhookReceiversAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/integratedSystems/${id}/webhookreceivers`,
        token,
    });
};
const createWebhookReceiverAsync = async ({ token, id, useSharedSecret, }) => {
    return await executeFetch({
        path: `api/integratedSystems/${id}/webhookreceivers`,
        token,
        method: "post",
        body: {},
        query: {
            useSharedSecret,
        },
    });
};
const setIsDCGeneratorAsync = async ({ token, id, value }) => {
    return await executeFetch({
        path: `api/integratedSystems/${id}/dcgenerator`,
        token,
        method: "post",
        body: value,
    });
};

var integratedSystemsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchIntegratedSystemsAsync: fetchIntegratedSystemsAsync,
    fetchIntegratedSystemAsync: fetchIntegratedSystemAsync,
    renameAsync: renameAsync,
    createIntegratedSystemAsync: createIntegratedSystemAsync,
    deleteIntegratedSystemAsync: deleteIntegratedSystemAsync,
    fetchWebhookReceiversAsync: fetchWebhookReceiversAsync,
    createWebhookReceiverAsync: createWebhookReceiverAsync,
    setIsDCGeneratorAsync: setIsDCGeneratorAsync
});

const fetchModelRegistrationsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/ModelRegistrations",
        token,
        page,
    });
};
const fetchModelRegistrationAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/ModelRegistrations/${id}`,
        token,
    });
};
const deleteModelRegistrationAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/ModelRegistrations/${id}`,
        token,
        method: "delete",
    });
};
const createModelRegistrationAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/ModelRegistrations",
        token,
        method: "post",
        body: payload,
    });
};
const invokeModelAsync = async ({ token, modelId, metrics }) => {
    return await executeFetch({
        path: `api/ModelRegistrations/${modelId}/invoke`,
        token,
        method: "post",
        body: metrics,
    });
};

var modelRegistrationsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchModelRegistrationsAsync: fetchModelRegistrationsAsync,
    fetchModelRegistrationAsync: fetchModelRegistrationAsync,
    deleteModelRegistrationAsync: deleteModelRegistrationAsync,
    createModelRegistrationAsync: createModelRegistrationAsync,
    invokeModelAsync: invokeModelAsync
});

const invokeGenericModelAsync = async ({ token, id, input }) => {
    return await executeFetch({
        path: `api/models/generic/${id}/invoke`,
        body: input,
        method: "post",
        token,
    });
};

var index = /*#__PURE__*/Object.freeze({
    __proto__: null,
    invokeGenericModelAsync: invokeGenericModelAsync
});

const fetchParametersAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/Parameters",
        token,
        page,
    });
};
const fetchParameterAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/parameters/${id}`,
        token,
    });
};
const deleteParameterAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/parameters/${id}`,
        token,
        method: "delete",
    });
};
const createParameterAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/Parameters",
        token,
        method: "post",
        body: payload,
    });
};

var parametersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchParametersAsync: fetchParametersAsync,
    fetchParameterAsync: fetchParameterAsync,
    deleteParameterAsync: deleteParameterAsync,
    createParameterAsync: createParameterAsync
});

const fetchLinkedRegisteredModelAsync$6 = async ({ recommenderApiName, token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ModelRegistration`,
        token,
    });
};
const createLinkedRegisteredModelAsync$1 = async ({ recommenderApiName, token, id, modelId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ModelRegistration`,
        token,
        method: "post",
        body: { modelId },
    });
};

const fetchRecommenderTargetVariableValuesAsync = async ({ recommenderApiName, name, token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/TargetVariableValues`,
        token,
        query: {
            name,
        },
    });
};
const createRecommenderTargetVariableValueAsync = async ({ recommenderApiName, targetVariableValue, token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/TargetVariableValues`,
        token,
        method: "post",
        body: targetVariableValue,
    });
};

const fetchRecommenderInvokationLogsAsync = async ({ recommenderApiName, token, id, page, pageSize, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/InvokationLogs`,
        page,
        pageSize,
        token,
    });
};

const setArgumentsAsync$6 = async ({ recommenderApiName, token, id, args, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Arguments`,
        token,
        method: "post",
        body: args,
    });
};
const fetchArgumentsAsync$5 = async ({ recommenderApiName, token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Arguments`,
        token,
        method: "get",
    });
};
const fetchChoosePromotionArgumentRulesAsync$3 = async ({ recommenderApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChoosePromotionArgumentRules`,
        token,
        method: "get",
        query: {
            useInternalId,
        },
    });
};
const createChoosePromotionArgumentRuleAsync$3 = async ({ recommenderApiName, token, id, useInternalId, rule, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChoosePromotionArgumentRules`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const updateChoosePromotionArgumentRuleAsync$3 = async ({ recommenderApiName, token, id, useInternalId, rule, ruleId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChoosePromotionArgumentRules/${ruleId}`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const fetchChooseSegmentArgumentRulesAsync$3 = async ({ recommenderApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChooseSegmentArgumentRules`,
        token,
        method: "get",
        query: {
            useInternalId,
        },
    });
};
const createChooseSegmentArgumentRuleAsync$3 = async ({ recommenderApiName, token, id, useInternalId, rule, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChooseSegmentArgumentRules`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const updateChooseSegmentArgumentRuleAsync$3 = async ({ recommenderApiName, token, id, useInternalId, rule, ruleId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChooseSegmentArgumentRules/${ruleId}`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const deleteArgumentRuleAsync$3 = async ({ recommenderApiName, token, id, useInternalId, ruleId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ArgumentRules/${ruleId}`,
        token,
        method: "delete",
        query: {
            useInternalId,
        },
    });
};

const setSettingsAsync$6 = async ({ recommenderApiName, token, id, settings, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Settings`,
        token,
        method: "post",
        body: settings,
    });
};

const fetchDestinationsAsync$6 = async ({ recommenderApiName, token, id, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/${recommenderApiName}/${id}/Destinations`,
    });
};
const createDestinationAsync$6 = async ({ recommenderApiName, token, id, destination, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/${recommenderApiName}/${id}/Destinations`,
        method: "post",
        body: destination,
    });
};
const removeDestinationAsync$6 = async ({ recommenderApiName, token, id, destinationId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Destinations/${destinationId}`,
        token,
        method: "delete",
    });
};

const setTriggerAsync$6 = async ({ recommenderApiName, token, id, trigger, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/TriggerCollection`,
        token,
        method: "post",
        body: trigger,
    });
};
const fetchTriggerAsync$6 = async ({ recommenderApiName, token, id }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/${recommenderApiName}/${id}/TriggerCollection`,
    });
};

const fetchLearningFeaturesAsync$6 = async ({ recommenderApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
    });
};
const setLearningFeaturesAsync$6 = async ({ recommenderApiName, token, id, useInternalId, featureIds, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
        token,
        method: "post",
        query: { useInternalId },
        body: { featureIds, useInternalId },
    });
};

const fetchLearningMetricsAsync$6 = async ({ recommenderApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
    });
};
const setLearningMetricsAsync$6 = async ({ recommenderApiName, token, id, useInternalId, metricIds, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
        token,
        method: "post",
        query: { useInternalId },
        body: { metricIds, useInternalId },
    });
};

const fetchReportImageBlobUrlAsync$6 = async ({ recommenderApiName, token, id, useInternalId, }) => {
    console.debug("fetching image for recommender");
    console.debug(`api/recommenders/${recommenderApiName}/${id}/ReportImage`);
    const axios = current();
    let response;
    try {
        response = await axios.get(`api/recommenders/${recommenderApiName}/${id}/ReportImage`, {
            headers: headers(token, null),
            responseType: "blob", // required for axios to understand response
        });
    }
    catch (ex) {
        console.error(ex);
        throw ex;
    }
    if (response.status >= 200 && response.status < 300) {
        return URL.createObjectURL(response.data);
    }
    else {
        handleErrorResponse(response);
    }
};

const recommenderApiName$2 = "ParameterSetRecommenders";
console.warn("Deprecation Notice: Parameter Set Recommenders are replaced by Parameter Set Campaigns.");
const fetchParameterSetRecommendersAsync = async ({ token, page, }) => {
    return await executeFetch({
        path: "api/recommenders/ParameterSetRecommenders",
        token,
        page,
    });
};
const fetchParameterSetRecommenderAsync = async ({ token, id, searchTerm, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}`,
        token,
        query: {
            "q.term": searchTerm,
        },
    });
};
const createParameterSetRecommenderAsync = async ({ token, payload, }) => {
    return await executeFetch({
        path: "api/recommenders/ParameterSetRecommenders",
        token,
        method: "post",
        body: payload,
    });
};
const deleteParameterSetRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}`,
        token,
        method: "delete",
    });
};
const fetchParameterSetRecommendationsAsync$1 = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}/recommendations`,
        token,
        page,
        pageSize,
    });
};
const createLinkRegisteredModelAsync$4 = async ({ token, id, modelId, }) => {
    return await createLinkedRegisteredModelAsync$1({
        recommenderApiName: recommenderApiName$2,
        id,
        modelId,
        token,
    });
};
const fetchLinkedRegisteredModelAsync$5 = async ({ token, id, }) => {
    return await fetchLinkedRegisteredModelAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
    });
};
const invokeParameterSetRecommenderAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}/invoke`,
        token,
        method: "post",
        body: input,
    });
};
const fetchInvokationLogsAsync$4 = async ({ id, token, page, pageSize, }) => {
    return await fetchRecommenderInvokationLogsAsync({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        page,
        pageSize,
    });
};
const fetchTargetVariablesAsync$4 = async ({ id, token, name }) => {
    return await fetchRecommenderTargetVariableValuesAsync({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        name,
    });
};
const createTargetVariableAsync$4 = async ({ id, token, targetVariableValue, }) => {
    return await createRecommenderTargetVariableValueAsync({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        targetVariableValue,
    });
};
const setSettingsAsync$5 = async ({ id, token, settings, }) => {
    return await setSettingsAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        settings,
    });
};
const fetchArgumentsAsync$4 = async ({ id, token }) => {
    return await fetchArgumentsAsync$5({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
    });
};
const setArgumentsAsync$5 = async ({ id, token, args, }) => {
    return await setArgumentsAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        args,
    });
};
const fetchDestinationsAsync$5 = async ({ id, token }) => {
    return await fetchDestinationsAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
    });
};
const createDestinationAsync$5 = async ({ id, token, destination, }) => {
    return await createDestinationAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        destination,
    });
};
const removeDestinationAsync$5 = async ({ id, token, destinationId, }) => {
    return await removeDestinationAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        destinationId,
    });
};
const fetchTriggerAsync$5 = async ({ id, token }) => {
    return await fetchTriggerAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
    });
};
const setTriggerAsync$5 = async ({ id, token, trigger, }) => {
    return await setTriggerAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        trigger,
    });
};
const fetchLearningFeaturesAsync$5 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningFeaturesAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        useInternalId,
    });
};
const setLearningFeaturesAsync$5 = async ({ id, token, featureIds, useInternalId, }) => {
    return await setLearningFeaturesAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        useInternalId,
        featureIds,
    });
};
const fetchLearningMetricsAsync$5 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningMetricsAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        useInternalId,
    });
};
const setLearningMetricsAsync$5 = async ({ id, token, metricIds, useInternalId, }) => {
    return await setLearningMetricsAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        useInternalId,
        metricIds,
    });
};
const fetchStatisticsAsync$4 = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}/Statistics`,
        token,
    });
};
const fetchReportImageBlobUrlAsync$5 = async ({ id, token, useInternalId, }) => {
    return await fetchReportImageBlobUrlAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        useInternalId,
    });
};

var parameterSetRecommendersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchParameterSetRecommendersAsync: fetchParameterSetRecommendersAsync,
    fetchParameterSetRecommenderAsync: fetchParameterSetRecommenderAsync,
    createParameterSetRecommenderAsync: createParameterSetRecommenderAsync,
    deleteParameterSetRecommenderAsync: deleteParameterSetRecommenderAsync,
    fetchParameterSetRecommendationsAsync: fetchParameterSetRecommendationsAsync$1,
    createLinkRegisteredModelAsync: createLinkRegisteredModelAsync$4,
    fetchLinkedRegisteredModelAsync: fetchLinkedRegisteredModelAsync$5,
    invokeParameterSetRecommenderAsync: invokeParameterSetRecommenderAsync,
    fetchInvokationLogsAsync: fetchInvokationLogsAsync$4,
    fetchTargetVariablesAsync: fetchTargetVariablesAsync$4,
    createTargetVariableAsync: createTargetVariableAsync$4,
    setSettingsAsync: setSettingsAsync$5,
    fetchArgumentsAsync: fetchArgumentsAsync$4,
    setArgumentsAsync: setArgumentsAsync$5,
    fetchDestinationsAsync: fetchDestinationsAsync$5,
    createDestinationAsync: createDestinationAsync$5,
    removeDestinationAsync: removeDestinationAsync$5,
    fetchTriggerAsync: fetchTriggerAsync$5,
    setTriggerAsync: setTriggerAsync$5,
    fetchLearningFeaturesAsync: fetchLearningFeaturesAsync$5,
    setLearningFeaturesAsync: setLearningFeaturesAsync$5,
    fetchLearningMetricsAsync: fetchLearningMetricsAsync$5,
    setLearningMetricsAsync: setLearningMetricsAsync$5,
    fetchStatisticsAsync: fetchStatisticsAsync$4,
    fetchReportImageBlobUrlAsync: fetchReportImageBlobUrlAsync$5
});

const fetchLinkedRegisteredModelAsync$4 = async ({ campaignApiName, token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ModelRegistration`,
        token,
    });
};
const createLinkedRegisteredModelAsync = async ({ campaignApiName, token, id, modelId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ModelRegistration`,
        token,
        method: "post",
        body: { modelId },
    });
};

const fetchCampaignTargetVariableValuesAsync = async ({ campaignApiName, name, token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/TargetVariableValues`,
        token,
        query: {
            name,
        },
    });
};
const createCampaignTargetVariableValueAsync = async ({ campaignApiName, targetVariableValue, token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/TargetVariableValues`,
        token,
        method: "post",
        body: targetVariableValue,
    });
};

const fetchCampaignInvokationLogsAsync = async ({ campaignApiName, token, id, page, pageSize, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/InvokationLogs`,
        page,
        pageSize,
        token,
    });
};

const setArgumentsAsync$4 = async ({ campaignApiName, token, id, args, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/Arguments`,
        token,
        method: "post",
        body: args,
    });
};
const fetchArgumentsAsync$3 = async ({ campaignApiName, token, id }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/Arguments`,
        token,
        method: "get",
    });
};
const fetchChoosePromotionArgumentRulesAsync$2 = async ({ campaignApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChoosePromotionArgumentRules`,
        token,
        method: "get",
        query: {
            useInternalId,
        },
    });
};
const createChoosePromotionArgumentRuleAsync$2 = async ({ campaignApiName, token, id, useInternalId, rule, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChoosePromotionArgumentRules`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const updateChoosePromotionArgumentRuleAsync$2 = async ({ campaignApiName, token, id, useInternalId, rule, ruleId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChoosePromotionArgumentRules/${ruleId}`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const fetchChooseSegmentArgumentRulesAsync$2 = async ({ campaignApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChooseSegmentArgumentRules`,
        token,
        method: "get",
        query: {
            useInternalId,
        },
    });
};
const createChooseSegmentArgumentRuleAsync$2 = async ({ campaignApiName, token, id, useInternalId, rule, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChooseSegmentArgumentRules`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const updateChooseSegmentArgumentRuleAsync$2 = async ({ campaignApiName, token, id, useInternalId, rule, ruleId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChooseSegmentArgumentRules/${ruleId}`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const deleteArgumentRuleAsync$2 = async ({ campaignApiName, token, id, useInternalId, ruleId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ArgumentRules/${ruleId}`,
        token,
        method: "delete",
        query: {
            useInternalId,
        },
    });
};

const setSettingsAsync$4 = async ({ campaignApiName, token, id, settings, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/Settings`,
        token,
        method: "post",
        body: settings,
    });
};

const fetchDestinationsAsync$4 = async ({ campaignApiName, token, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/${campaignApiName}/${id}/Destinations`,
    });
};
const createDestinationAsync$4 = async ({ campaignApiName, token, id, destination, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/${campaignApiName}/${id}/Destinations`,
        method: "post",
        body: destination,
    });
};
const removeDestinationAsync$4 = async ({ campaignApiName, token, id, destinationId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/Destinations/${destinationId}`,
        token,
        method: "delete",
    });
};

const setTriggerAsync$4 = async ({ campaignApiName, token, id, trigger, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/TriggerCollection`,
        token,
        method: "post",
        body: trigger,
    });
};
const fetchTriggerAsync$4 = async ({ campaignApiName, token, id }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/${campaignApiName}/${id}/TriggerCollection`,
    });
};

const fetchLearningFeaturesAsync$4 = async ({ campaignApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
    });
};
const setLearningFeaturesAsync$4 = async ({ campaignApiName, token, id, useInternalId, featureIds, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
        token,
        method: "post",
        query: { useInternalId },
        body: { featureIds, useInternalId },
    });
};

const fetchLearningMetricsAsync$4 = async ({ campaignApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
    });
};
const setLearningMetricsAsync$4 = async ({ campaignApiName, token, id, useInternalId, metricIds, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
        token,
        method: "post",
        query: { useInternalId },
        body: { metricIds, useInternalId },
    });
};

const fetchReportImageBlobUrlAsync$4 = async ({ campaignApiName, token, id, useInternalId, }) => {
    console.debug("fetching image for recommender");
    console.debug(`api/campaigns/${campaignApiName}/${id}/ReportImage`);
    const axios = current();
    let response;
    try {
        response = await axios.get(`api/campaigns/${campaignApiName}/${id}/ReportImage`, {
            headers: headers(token, null),
            responseType: "blob", // required for axios to understand response
        });
    }
    catch (ex) {
        console.error(ex);
        throw ex;
    }
    if (response.status >= 200 && response.status < 300) {
        return URL.createObjectURL(response.data);
    }
    else {
        handleErrorResponse(response);
    }
};

const campaignApiName$1 = "ParameterSetCampaigns";
const fetchParameterSetCampaignsAsync = async ({ token, page, }) => {
    return await executeFetch({
        path: "api/campaigns/ParameterSetCampaigns",
        token,
        page,
    });
};
const fetchParameterSetCampaignAsync = async ({ token, id, searchTerm, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}`,
        token,
        query: {
            "q.term": searchTerm,
        },
    });
};
const createParameterSetCampaignAsync = async ({ token, payload, }) => {
    return await executeFetch({
        path: "api/campaigns/ParameterSetCampaigns",
        token,
        method: "post",
        body: payload,
    });
};
const deleteParameterSetCampaignAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}`,
        token,
        method: "delete",
    });
};
const fetchParameterSetRecommendationsAsync = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}/recommendations`,
        token,
        page,
        pageSize,
    });
};
const createLinkRegisteredModelAsync$3 = async ({ token, id, modelId, }) => {
    return await createLinkedRegisteredModelAsync({
        campaignApiName: campaignApiName$1,
        id,
        modelId,
        token,
    });
};
const fetchLinkedRegisteredModelAsync$3 = async ({ token, id, }) => {
    return await fetchLinkedRegisteredModelAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
    });
};
const invokeParameterSetCampaignAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}/invoke`,
        token,
        method: "post",
        body: input,
    });
};
const fetchInvokationLogsAsync$3 = async ({ id, token, page, pageSize, }) => {
    return await fetchCampaignInvokationLogsAsync({
        campaignApiName: campaignApiName$1,
        id,
        token,
        page,
        pageSize,
    });
};
const fetchTargetVariablesAsync$3 = async ({ id, token, name }) => {
    return await fetchCampaignTargetVariableValuesAsync({
        campaignApiName: campaignApiName$1,
        id,
        token,
        name,
    });
};
const createTargetVariableAsync$3 = async ({ id, token, targetVariableValue, }) => {
    return await createCampaignTargetVariableValueAsync({
        campaignApiName: campaignApiName$1,
        id,
        token,
        targetVariableValue,
    });
};
const setSettingsAsync$3 = async ({ id, token, settings, }) => {
    return await setSettingsAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        settings,
    });
};
const fetchArgumentsAsync$2 = async ({ id, token }) => {
    return await fetchArgumentsAsync$3({
        campaignApiName: campaignApiName$1,
        id,
        token,
    });
};
const setArgumentsAsync$3 = async ({ id, token, args, }) => {
    return await setArgumentsAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        args,
    });
};
const fetchDestinationsAsync$3 = async ({ id, token }) => {
    return await fetchDestinationsAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
    });
};
const createDestinationAsync$3 = async ({ id, token, destination, }) => {
    return await createDestinationAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        destination,
    });
};
const removeDestinationAsync$3 = async ({ id, token, destinationId, }) => {
    return await removeDestinationAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        destinationId,
    });
};
const fetchTriggerAsync$3 = async ({ id, token }) => {
    return await fetchTriggerAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
    });
};
const setTriggerAsync$3 = async ({ id, token, trigger, }) => {
    return await setTriggerAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        trigger,
    });
};
const fetchLearningFeaturesAsync$3 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningFeaturesAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        useInternalId,
    });
};
const setLearningFeaturesAsync$3 = async ({ id, token, featureIds, useInternalId, }) => {
    return await setLearningFeaturesAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        useInternalId,
        featureIds,
    });
};
const fetchLearningMetricsAsync$3 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningMetricsAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        useInternalId,
    });
};
const setLearningMetricsAsync$3 = async ({ id, token, metricIds, useInternalId, }) => {
    return await setLearningMetricsAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        useInternalId,
        metricIds,
    });
};
const fetchStatisticsAsync$3 = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}/Statistics`,
        token,
    });
};
const fetchReportImageBlobUrlAsync$3 = async ({ id, token, useInternalId, }) => {
    return await fetchReportImageBlobUrlAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        useInternalId,
    });
};

var parameterSetCampaignsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchParameterSetCampaignsAsync: fetchParameterSetCampaignsAsync,
    fetchParameterSetCampaignAsync: fetchParameterSetCampaignAsync,
    createParameterSetCampaignAsync: createParameterSetCampaignAsync,
    deleteParameterSetCampaignAsync: deleteParameterSetCampaignAsync,
    fetchParameterSetRecommendationsAsync: fetchParameterSetRecommendationsAsync,
    createLinkRegisteredModelAsync: createLinkRegisteredModelAsync$3,
    fetchLinkedRegisteredModelAsync: fetchLinkedRegisteredModelAsync$3,
    invokeParameterSetCampaignAsync: invokeParameterSetCampaignAsync,
    fetchInvokationLogsAsync: fetchInvokationLogsAsync$3,
    fetchTargetVariablesAsync: fetchTargetVariablesAsync$3,
    createTargetVariableAsync: createTargetVariableAsync$3,
    setSettingsAsync: setSettingsAsync$3,
    fetchArgumentsAsync: fetchArgumentsAsync$2,
    setArgumentsAsync: setArgumentsAsync$3,
    fetchDestinationsAsync: fetchDestinationsAsync$3,
    createDestinationAsync: createDestinationAsync$3,
    removeDestinationAsync: removeDestinationAsync$3,
    fetchTriggerAsync: fetchTriggerAsync$3,
    setTriggerAsync: setTriggerAsync$3,
    fetchLearningFeaturesAsync: fetchLearningFeaturesAsync$3,
    setLearningFeaturesAsync: setLearningFeaturesAsync$3,
    fetchLearningMetricsAsync: fetchLearningMetricsAsync$3,
    setLearningMetricsAsync: setLearningMetricsAsync$3,
    fetchStatisticsAsync: fetchStatisticsAsync$3,
    fetchReportImageBlobUrlAsync: fetchReportImageBlobUrlAsync$3
});

const setMetadataAsync = async ({ token, metadata }) => {
    return await executeFetch({
        path: "api/profile/metadata",
        token,
        method: "post",
        body: metadata,
    });
};
const getMetadataAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/profile/metadata",
        token,
    });
};

var profileApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    setMetadataAsync: setMetadataAsync,
    getMetadataAsync: getMetadataAsync
});

const recommenderApiName$1 = "ItemsRecommenders";
console.warn("Deprecation Notice: Items Recommenders are replaced by Promotions Recommenders.");
const fetchItemsRecommendersAsync = async ({ token, page, }) => {
    return await executeFetch({
        token,
        path: "api/recommenders/ItemsRecommenders",
        page,
    });
};
const fetchItemsRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}`,
        token,
    });
};
const fetchItemsRecommendationsAsync = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/ItemsRecommenders/${id}/Recommendations`,
        page,
        pageSize,
    });
};
const deleteItemsRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}`,
        token,
        method: "delete",
    });
};
const createItemsRecommenderAsync = async ({ token, payload, useInternalId, }) => {
    return await executeFetch({
        path: "api/recommenders/ItemsRecommenders",
        token,
        method: "post",
        body: payload,
        query: { useInternalId },
    });
};
const fetchItemsAsync$1 = async ({ token, id }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Items`,
        token,
    });
};
const addItemAsync = async ({ token, id, item }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Items`,
        token,
        method: "post",
        body: item,
    });
};
const removeItemAsync = async ({ token, id, itemId, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Items/${itemId}`,
        token,
        method: "post",
    });
};
const setBaselineItemAsync = async ({ token, id, itemId, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/BaselineItem`,
        token,
        method: "post",
        body: { itemId },
    });
};
const setDefaultItemAsync = setBaselineItemAsync; // backwards compat
const getBaselineItemAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/BaselineItem`,
        token,
    });
};
const getDefaultItemAsync = getBaselineItemAsync; // backwards compat
const createLinkRegisteredModelAsync$2 = async ({ token, id, modelId, }) => {
    return await createLinkedRegisteredModelAsync$1({
        recommenderApiName: recommenderApiName$1,
        id,
        modelId,
        token,
    });
};
const fetchLinkedRegisteredModelAsync$2 = async ({ token, id, }) => {
    return await fetchLinkedRegisteredModelAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
    });
};
const invokeItemsRecommenderAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Invoke`,
        token,
        method: "post",
        body: input,
    });
};
const fetchInvokationLogsAsync$2 = async ({ id, token, page, pageSize, }) => {
    return await fetchRecommenderInvokationLogsAsync({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        page,
        pageSize,
    });
};
const fetchTargetVariablesAsync$2 = async ({ id, token, name }) => {
    return await fetchRecommenderTargetVariableValuesAsync({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        name,
    });
};
const createTargetVariableAsync$2 = async ({ id, token, targetVariableValue, }) => {
    return await createRecommenderTargetVariableValueAsync({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        targetVariableValue,
    });
};
const setSettingsAsync$2 = async ({ id, token, settings, }) => {
    return await setSettingsAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        settings,
    });
};
const setArgumentsAsync$2 = async ({ id, token, args, }) => {
    return await setArgumentsAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        args,
    });
};
const fetchDestinationsAsync$2 = async ({ id, token }) => {
    return await fetchDestinationsAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
    });
};
const createDestinationAsync$2 = async ({ id, token, destination, }) => {
    return await createDestinationAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        destination,
    });
};
const removeDestinationAsync$2 = async ({ id, token, destinationId, }) => {
    return await removeDestinationAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        destinationId,
    });
};
const fetchTriggerAsync$2 = async ({ id, token }) => {
    return await fetchTriggerAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
    });
};
const setTriggerAsync$2 = async ({ id, token, trigger, }) => {
    return await setTriggerAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        trigger,
    });
};
const fetchLearningFeaturesAsync$2 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningFeaturesAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        useInternalId,
    });
};
const setLearningFeaturesAsync$2 = async ({ id, token, featureIds, useInternalId, }) => {
    return await setLearningFeaturesAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        useInternalId,
        featureIds,
    });
};
const fetchLearningMetricsAsync$2 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningMetricsAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        useInternalId,
    });
};
const setLearningMetricsAsync$2 = async ({ id, token, metricIds, useInternalId, }) => {
    return await setLearningMetricsAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        useInternalId,
        metricIds,
    });
};
const fetchStatisticsAsync$2 = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Statistics`,
        token,
    });
};
const fetchReportImageBlobUrlAsync$2 = async ({ id, token, useInternalId, }) => {
    return await fetchReportImageBlobUrlAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        useInternalId,
    });
};
const fetchPerformanceAsync$2 = async ({ token, id, reportId, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/ItemsRecommenders/${id}/Performance/${reportId !== null && reportId !== void 0 ? reportId : "latest"}`,
    });
};

var itemsRecommendersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchItemsRecommendersAsync: fetchItemsRecommendersAsync,
    fetchItemsRecommenderAsync: fetchItemsRecommenderAsync,
    fetchItemsRecommendationsAsync: fetchItemsRecommendationsAsync,
    deleteItemsRecommenderAsync: deleteItemsRecommenderAsync,
    createItemsRecommenderAsync: createItemsRecommenderAsync,
    fetchItemsAsync: fetchItemsAsync$1,
    addItemAsync: addItemAsync,
    removeItemAsync: removeItemAsync,
    setBaselineItemAsync: setBaselineItemAsync,
    setDefaultItemAsync: setDefaultItemAsync,
    getBaselineItemAsync: getBaselineItemAsync,
    getDefaultItemAsync: getDefaultItemAsync,
    createLinkRegisteredModelAsync: createLinkRegisteredModelAsync$2,
    fetchLinkedRegisteredModelAsync: fetchLinkedRegisteredModelAsync$2,
    invokeItemsRecommenderAsync: invokeItemsRecommenderAsync,
    fetchInvokationLogsAsync: fetchInvokationLogsAsync$2,
    fetchTargetVariablesAsync: fetchTargetVariablesAsync$2,
    createTargetVariableAsync: createTargetVariableAsync$2,
    setSettingsAsync: setSettingsAsync$2,
    setArgumentsAsync: setArgumentsAsync$2,
    fetchDestinationsAsync: fetchDestinationsAsync$2,
    createDestinationAsync: createDestinationAsync$2,
    removeDestinationAsync: removeDestinationAsync$2,
    fetchTriggerAsync: fetchTriggerAsync$2,
    setTriggerAsync: setTriggerAsync$2,
    fetchLearningFeaturesAsync: fetchLearningFeaturesAsync$2,
    setLearningFeaturesAsync: setLearningFeaturesAsync$2,
    fetchLearningMetricsAsync: fetchLearningMetricsAsync$2,
    setLearningMetricsAsync: setLearningMetricsAsync$2,
    fetchStatisticsAsync: fetchStatisticsAsync$2,
    fetchReportImageBlobUrlAsync: fetchReportImageBlobUrlAsync$2,
    fetchPerformanceAsync: fetchPerformanceAsync$2
});

const recommenderApiName = "PromotionsRecommenders";
console.warn("Deprecation Notice: Promotions Recommenders are replaced by Promotions Campaigns.");
const fetchPromotionsRecommendersAsync = async ({ token, page, }) => {
    return await executeFetch({
        token,
        path: "api/recommenders/PromotionsRecommenders",
        page,
    });
};
const fetchPromotionsRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}`,
        token,
    });
};
const fetchPromotionsRecommendationsAsync$1 = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/PromotionsRecommenders/${id}/Recommendations`,
        page,
        pageSize,
    });
};
const deletePromotionsRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}`,
        token,
        method: "delete",
    });
};
const createPromotionsRecommenderAsync = async ({ token, payload, useInternalId, }) => {
    return await executeFetch({
        path: "api/recommenders/PromotionsRecommenders",
        token,
        method: "post",
        body: payload,
        query: { useInternalId },
    });
};
const fetchPromotionsAsync$2 = async ({ token, id }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Promotions`,
        token,
    });
};
const fetchAudienceAsync$1 = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Audience`,
        token,
    });
};
const addPromotionAsync$1 = async ({ token, id, promotion, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Promotions`,
        token,
        method: "post",
        body: promotion,
    });
};
const removePromotionAsync$1 = async ({ token, id, promotionId, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Promotions/${promotionId}`,
        token,
        method: "post",
    });
};
const setBaselinePromotionAsync$1 = async ({ token, id, promotionId, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/BaselinePromotion`,
        token,
        method: "post",
        body: { promotionId },
    });
};
const getBaselinePromotionAsync$1 = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/BaselinePromotion`,
        token,
    });
};
const createLinkRegisteredModelAsync$1 = async ({ token, id, modelId, }) => {
    return await createLinkedRegisteredModelAsync$1({
        recommenderApiName,
        id,
        modelId,
        token,
    });
};
const fetchLinkedRegisteredModelAsync$1 = async ({ token, id, }) => {
    return await fetchLinkedRegisteredModelAsync$6({
        recommenderApiName,
        id,
        token,
    });
};
const invokePromotionsRecommenderAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Invoke`,
        token,
        method: "post",
        body: input,
    });
};
const fetchInvokationLogsAsync$1 = async ({ id, token, page, pageSize, }) => {
    return await fetchRecommenderInvokationLogsAsync({
        recommenderApiName,
        id,
        token,
        page,
        pageSize,
    });
};
const fetchTargetVariablesAsync$1 = async ({ id, token, name }) => {
    return await fetchRecommenderTargetVariableValuesAsync({
        recommenderApiName,
        id,
        token,
        name,
    });
};
const createTargetVariableAsync$1 = async ({ id, token, targetVariableValue, }) => {
    return await createRecommenderTargetVariableValueAsync({
        recommenderApiName,
        id,
        token,
        targetVariableValue,
    });
};
const setSettingsAsync$1 = async ({ id, token, settings, }) => {
    return await setSettingsAsync$6({
        recommenderApiName,
        id,
        token,
        settings,
    });
};
const fetchArgumentsAsync$1 = async ({ id, token }) => {
    return await fetchArgumentsAsync$5({
        recommenderApiName,
        id,
        token,
    });
};
const setArgumentsAsync$1 = async ({ id, token, args, }) => {
    return await setArgumentsAsync$6({
        recommenderApiName,
        id,
        token,
        args,
    });
};
const createChoosePromotionArgumentRuleAsync$1 = async ({ id, useInternalId, token, rule, }) => {
    return await createChoosePromotionArgumentRuleAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
        rule,
    });
};
const updateChoosePromotionArgumentRuleAsync$1 = async ({ id, useInternalId, token, rule, ruleId, }) => {
    return await updateChoosePromotionArgumentRuleAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
        rule,
        ruleId,
    });
};
const fetchChoosePromotionArgumentRulesAsync$1 = async ({ id, useInternalId, token, }) => {
    return await fetchChoosePromotionArgumentRulesAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
const createChooseSegmentArgumentRuleAsync$1 = async ({ id, useInternalId, token, rule, }) => {
    return await createChooseSegmentArgumentRuleAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
        rule,
    });
};
const updateChooseSegmentArgumentRuleAsync$1 = async ({ id, useInternalId, token, rule, ruleId, }) => {
    return await updateChooseSegmentArgumentRuleAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
        rule,
        ruleId,
    });
};
const fetchChooseSegmentArgumentRulesAsync$1 = async ({ id, useInternalId, token, }) => {
    return await fetchChooseSegmentArgumentRulesAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
const deleteArgumentRuleAsync$1 = async ({ id, useInternalId, token, ruleId, }) => {
    return await deleteArgumentRuleAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
        ruleId,
    });
};
const fetchDestinationsAsync$1 = async ({ id, token }) => {
    return await fetchDestinationsAsync$6({
        recommenderApiName,
        id,
        token,
    });
};
const createDestinationAsync$1 = async ({ id, token, destination, }) => {
    return await createDestinationAsync$6({
        recommenderApiName,
        id,
        token,
        destination,
    });
};
const removeDestinationAsync$1 = async ({ id, token, destinationId, }) => {
    return await removeDestinationAsync$6({
        recommenderApiName,
        id,
        token,
        destinationId,
    });
};
const fetchTriggerAsync$1 = async ({ id, token }) => {
    return await fetchTriggerAsync$6({
        recommenderApiName,
        id,
        token,
    });
};
const setTriggerAsync$1 = async ({ id, token, trigger, }) => {
    return await setTriggerAsync$6({
        recommenderApiName,
        id,
        token,
        trigger,
    });
};
const fetchLearningFeaturesAsync$1 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningFeaturesAsync$6({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
const setLearningFeaturesAsync$1 = async ({ id, token, featureIds, useInternalId, }) => {
    return await setLearningFeaturesAsync$6({
        recommenderApiName,
        id,
        token,
        useInternalId,
        featureIds,
    });
};
const fetchLearningMetricsAsync$1 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningMetricsAsync$6({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
const setLearningMetricsAsync$1 = async ({ id, token, metricIds, useInternalId, }) => {
    return await setLearningMetricsAsync$6({
        recommenderApiName,
        id,
        token,
        useInternalId,
        metricIds,
    });
};
const fetchStatisticsAsync$1 = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Statistics`,
        token,
    });
};
const fetchReportImageBlobUrlAsync$1 = async ({ id, token, useInternalId, }) => {
    return await fetchReportImageBlobUrlAsync$6({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
const fetchPerformanceAsync$1 = async ({ token, id, reportId, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/PromotionsRecommenders/${id}/Performance/${reportId !== null && reportId !== void 0 ? reportId : "latest"}`,
    });
};
const fetchPromotionOptimiserAsync$1 = async ({ token, useInternalId, id, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/PromotionsRecommenders/${id}/Optimiser/`,
    });
};
const setAllPromotionOptimiserWeightsAsync$1 = async ({ token, useInternalId, id, weights, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/PromotionsRecommenders/${id}/Optimiser/Weights/`,
        method: "post",
        body: weights,
    });
};
const setPromotionOptimiserWeightAsync$1 = async ({ token, useInternalId, id, weightId, weight, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/PromotionsRecommenders/${id}/Optimiser/Weights/${weightId}`,
        method: "post",
        body: { weight },
    });
};
const setUseOptimiserAsync$1 = async ({ token, useInternalId, id, useOptimiser, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/PromotionsRecommenders/${id}/UseOptimiser`,
        method: "post",
        body: { useOptimiser },
    });
};
const fetchRecommenderChannelsAsync = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Channels`,
        token,
    });
};
const addRecommenderChannelAsync = async ({ token, id, channel, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Channels`,
        token,
        method: "post",
        body: channel,
    });
};
const removeRecommenderChannelAsync = async ({ id, token, channelId, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Channels/${channelId}`,
        token,
        method: "delete",
    });
};
const fetchPromotionsRecommendationAsync$1 = async ({ token, recommendationId, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/PromotionsRecommenders/Recommendations/${recommendationId}`,
    });
};
const fetchOffersAsync$1 = async ({ token, page, pageSize, id, offerState, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/PromotionsRecommenders/${id}/Offers`,
        page,
        pageSize,
        query: { offerState },
    });
};

var promotionsRecommendersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchPromotionsRecommendersAsync: fetchPromotionsRecommendersAsync,
    fetchPromotionsRecommenderAsync: fetchPromotionsRecommenderAsync,
    fetchPromotionsRecommendationsAsync: fetchPromotionsRecommendationsAsync$1,
    deletePromotionsRecommenderAsync: deletePromotionsRecommenderAsync,
    createPromotionsRecommenderAsync: createPromotionsRecommenderAsync,
    fetchPromotionsAsync: fetchPromotionsAsync$2,
    fetchAudienceAsync: fetchAudienceAsync$1,
    addPromotionAsync: addPromotionAsync$1,
    removePromotionAsync: removePromotionAsync$1,
    setBaselinePromotionAsync: setBaselinePromotionAsync$1,
    getBaselinePromotionAsync: getBaselinePromotionAsync$1,
    createLinkRegisteredModelAsync: createLinkRegisteredModelAsync$1,
    fetchLinkedRegisteredModelAsync: fetchLinkedRegisteredModelAsync$1,
    invokePromotionsRecommenderAsync: invokePromotionsRecommenderAsync,
    fetchInvokationLogsAsync: fetchInvokationLogsAsync$1,
    fetchTargetVariablesAsync: fetchTargetVariablesAsync$1,
    createTargetVariableAsync: createTargetVariableAsync$1,
    setSettingsAsync: setSettingsAsync$1,
    fetchArgumentsAsync: fetchArgumentsAsync$1,
    setArgumentsAsync: setArgumentsAsync$1,
    createChoosePromotionArgumentRuleAsync: createChoosePromotionArgumentRuleAsync$1,
    updateChoosePromotionArgumentRuleAsync: updateChoosePromotionArgumentRuleAsync$1,
    fetchChoosePromotionArgumentRulesAsync: fetchChoosePromotionArgumentRulesAsync$1,
    createChooseSegmentArgumentRuleAsync: createChooseSegmentArgumentRuleAsync$1,
    updateChooseSegmentArgumentRuleAsync: updateChooseSegmentArgumentRuleAsync$1,
    fetchChooseSegmentArgumentRulesAsync: fetchChooseSegmentArgumentRulesAsync$1,
    deleteArgumentRuleAsync: deleteArgumentRuleAsync$1,
    fetchDestinationsAsync: fetchDestinationsAsync$1,
    createDestinationAsync: createDestinationAsync$1,
    removeDestinationAsync: removeDestinationAsync$1,
    fetchTriggerAsync: fetchTriggerAsync$1,
    setTriggerAsync: setTriggerAsync$1,
    fetchLearningFeaturesAsync: fetchLearningFeaturesAsync$1,
    setLearningFeaturesAsync: setLearningFeaturesAsync$1,
    fetchLearningMetricsAsync: fetchLearningMetricsAsync$1,
    setLearningMetricsAsync: setLearningMetricsAsync$1,
    fetchStatisticsAsync: fetchStatisticsAsync$1,
    fetchReportImageBlobUrlAsync: fetchReportImageBlobUrlAsync$1,
    fetchPerformanceAsync: fetchPerformanceAsync$1,
    fetchPromotionOptimiserAsync: fetchPromotionOptimiserAsync$1,
    setAllPromotionOptimiserWeightsAsync: setAllPromotionOptimiserWeightsAsync$1,
    setPromotionOptimiserWeightAsync: setPromotionOptimiserWeightAsync$1,
    setUseOptimiserAsync: setUseOptimiserAsync$1,
    fetchRecommenderChannelsAsync: fetchRecommenderChannelsAsync,
    addRecommenderChannelAsync: addRecommenderChannelAsync,
    removeRecommenderChannelAsync: removeRecommenderChannelAsync,
    fetchPromotionsRecommendationAsync: fetchPromotionsRecommendationAsync$1,
    fetchOffersAsync: fetchOffersAsync$1
});

const campaignApiName = "PromotionsCampaigns";
const fetchPromotionsCampaignsAsync = async ({ token, page, }) => {
    return await executeFetch({
        token,
        path: "api/campaigns/PromotionsCampaigns",
        page,
    });
};
const fetchPromotionsCampaignAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}`,
        token,
    });
};
const fetchPromotionsRecommendationsAsync = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/Recommendations`,
        page,
        pageSize,
    });
};
const deletePromotionsCampaignAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}`,
        token,
        method: "delete",
    });
};
const createPromotionsCampaignAsync = async ({ token, payload, useInternalId, }) => {
    return await executeFetch({
        path: "api/campaigns/PromotionsCampaigns",
        token,
        method: "post",
        body: payload,
        query: { useInternalId },
    });
};
const fetchPromotionsAsync$1 = async ({ token, id }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Promotions`,
        token,
    });
};
const fetchAudienceAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Audience`,
        token,
    });
};
const addPromotionAsync = async ({ token, id, promotion, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Promotions`,
        token,
        method: "post",
        body: promotion,
    });
};
const removePromotionAsync = async ({ token, id, promotionId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Promotions/${promotionId}`,
        token,
        method: "delete",
    });
};
const setBaselinePromotionAsync = async ({ token, id, promotionId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/BaselinePromotion`,
        token,
        method: "post",
        body: { promotionId },
    });
};
const getBaselinePromotionAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/BaselinePromotion`,
        token,
    });
};
const createLinkRegisteredModelAsync = async ({ token, id, modelId, }) => {
    return await createLinkedRegisteredModelAsync({
        campaignApiName,
        id,
        modelId,
        token,
    });
};
const fetchLinkedRegisteredModelAsync = async ({ token, id, }) => {
    return await fetchLinkedRegisteredModelAsync$4({
        campaignApiName,
        id,
        token,
    });
};
const invokePromotionsCampaignAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Invoke`,
        token,
        method: "post",
        body: input,
    });
};
const fetchInvokationLogsAsync = async ({ id, token, page, pageSize, }) => {
    return await fetchCampaignInvokationLogsAsync({
        campaignApiName,
        id,
        token,
        page,
        pageSize,
    });
};
const fetchTargetVariablesAsync = async ({ id, token, name }) => {
    return await fetchCampaignTargetVariableValuesAsync({
        campaignApiName,
        id,
        token,
        name,
    });
};
const createTargetVariableAsync = async ({ id, token, targetVariableValue, }) => {
    return await createCampaignTargetVariableValueAsync({
        campaignApiName,
        id,
        token,
        targetVariableValue,
    });
};
const setSettingsAsync = async ({ id, token, settings, }) => {
    return await setSettingsAsync$4({
        campaignApiName,
        id,
        token,
        settings,
    });
};
const fetchArgumentsAsync = async ({ id, token }) => {
    return await fetchArgumentsAsync$3({
        campaignApiName,
        id,
        token,
    });
};
const setArgumentsAsync = async ({ id, token, args, }) => {
    return await setArgumentsAsync$4({
        campaignApiName,
        id,
        token,
        args,
    });
};
const createChoosePromotionArgumentRuleAsync = async ({ id, useInternalId, token, rule, }) => {
    return await createChoosePromotionArgumentRuleAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
        rule,
    });
};
const updateChoosePromotionArgumentRuleAsync = async ({ id, useInternalId, token, rule, ruleId, }) => {
    return await updateChoosePromotionArgumentRuleAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
        rule,
        ruleId,
    });
};
const fetchChoosePromotionArgumentRulesAsync = async ({ id, useInternalId, token, }) => {
    return await fetchChoosePromotionArgumentRulesAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
const createChooseSegmentArgumentRuleAsync = async ({ id, useInternalId, token, rule, }) => {
    return await createChooseSegmentArgumentRuleAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
        rule,
    });
};
const updateChooseSegmentArgumentRuleAsync = async ({ id, useInternalId, token, rule, ruleId, }) => {
    return await updateChooseSegmentArgumentRuleAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
        rule,
        ruleId,
    });
};
const fetchChooseSegmentArgumentRulesAsync = async ({ id, useInternalId, token, }) => {
    return await fetchChooseSegmentArgumentRulesAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
const deleteArgumentRuleAsync = async ({ id, useInternalId, token, ruleId, }) => {
    return await deleteArgumentRuleAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
        ruleId,
    });
};
const fetchDestinationsAsync = async ({ id, token }) => {
    return await fetchDestinationsAsync$4({
        campaignApiName,
        id,
        token,
    });
};
const createDestinationAsync = async ({ id, token, destination, }) => {
    return await createDestinationAsync$4({
        campaignApiName,
        id,
        token,
        destination,
    });
};
const removeDestinationAsync = async ({ id, token, destinationId, }) => {
    return await removeDestinationAsync$4({
        campaignApiName,
        id,
        token,
        destinationId,
    });
};
const fetchTriggerAsync = async ({ id, token }) => {
    return await fetchTriggerAsync$4({
        campaignApiName,
        id,
        token,
    });
};
const setTriggerAsync = async ({ id, token, trigger, }) => {
    return await setTriggerAsync$4({
        campaignApiName,
        id,
        token,
        trigger,
    });
};
const fetchLearningFeaturesAsync = async ({ id, token, useInternalId, }) => {
    return await fetchLearningFeaturesAsync$4({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
const setLearningFeaturesAsync = async ({ id, token, featureIds, useInternalId, }) => {
    return await setLearningFeaturesAsync$4({
        campaignApiName,
        id,
        token,
        useInternalId,
        featureIds,
    });
};
const fetchLearningMetricsAsync = async ({ id, token, useInternalId, }) => {
    return await fetchLearningMetricsAsync$4({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
const setLearningMetricsAsync = async ({ id, token, metricIds, useInternalId, }) => {
    return await setLearningMetricsAsync$4({
        campaignApiName,
        id,
        token,
        useInternalId,
        metricIds,
    });
};
const fetchStatisticsAsync = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Statistics`,
        token,
    });
};
const fetchReportImageBlobUrlAsync = async ({ id, token, useInternalId, }) => {
    return await fetchReportImageBlobUrlAsync$4({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
const fetchPerformanceAsync = async ({ token, id, reportId, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/Performance/${reportId !== null && reportId !== void 0 ? reportId : "latest"}`,
    });
};
const fetchPromotionOptimiserAsync = async ({ token, useInternalId, id, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/`,
    });
};
const setAllPromotionOptimiserWeightsAsync = async ({ token, useInternalId, id, weights, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Weights/`,
        method: "post",
        body: weights,
    });
};
const setPromotionOptimiserWeightAsync = async ({ token, useInternalId, id, weightId, weight, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Weights/${weightId}`,
        method: "post",
        body: { weight },
    });
};
const setUseOptimiserAsync = async ({ token, useInternalId, id, useOptimiser, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/UseOptimiser`,
        method: "post",
        body: { useOptimiser },
    });
};
const fetchCampaignChannelsAsync = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Channels`,
        token,
    });
};
const addCampaignChannelAsync = async ({ token, id, channel, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Channels`,
        token,
        method: "post",
        body: channel,
    });
};
const removeCampaignChannelAsync = async ({ id, token, channelId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Channels/${channelId}`,
        token,
        method: "delete",
    });
};
const fetchPromotionsRecommendationAsync = async ({ token, recommendationId, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/Recommendations/${recommendationId}`,
    });
};
const fetchOffersAsync = async ({ token, page, pageSize, id, offerState, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/Offers`,
        page,
        pageSize,
        query: { offerState },
    });
};
const fetchARPOReportAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/ARPOReport`,
    });
};
const fetchOfferConversionRateReportAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/ConversionRateReport`,
    });
};

var promotionsCampaignsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchPromotionsCampaignsAsync: fetchPromotionsCampaignsAsync,
    fetchPromotionsCampaignAsync: fetchPromotionsCampaignAsync,
    fetchPromotionsRecommendationsAsync: fetchPromotionsRecommendationsAsync,
    deletePromotionsCampaignAsync: deletePromotionsCampaignAsync,
    createPromotionsCampaignAsync: createPromotionsCampaignAsync,
    fetchPromotionsAsync: fetchPromotionsAsync$1,
    fetchAudienceAsync: fetchAudienceAsync,
    addPromotionAsync: addPromotionAsync,
    removePromotionAsync: removePromotionAsync,
    setBaselinePromotionAsync: setBaselinePromotionAsync,
    getBaselinePromotionAsync: getBaselinePromotionAsync,
    createLinkRegisteredModelAsync: createLinkRegisteredModelAsync,
    fetchLinkedRegisteredModelAsync: fetchLinkedRegisteredModelAsync,
    invokePromotionsCampaignAsync: invokePromotionsCampaignAsync,
    fetchInvokationLogsAsync: fetchInvokationLogsAsync,
    fetchTargetVariablesAsync: fetchTargetVariablesAsync,
    createTargetVariableAsync: createTargetVariableAsync,
    setSettingsAsync: setSettingsAsync,
    fetchArgumentsAsync: fetchArgumentsAsync,
    setArgumentsAsync: setArgumentsAsync,
    createChoosePromotionArgumentRuleAsync: createChoosePromotionArgumentRuleAsync,
    updateChoosePromotionArgumentRuleAsync: updateChoosePromotionArgumentRuleAsync,
    fetchChoosePromotionArgumentRulesAsync: fetchChoosePromotionArgumentRulesAsync,
    createChooseSegmentArgumentRuleAsync: createChooseSegmentArgumentRuleAsync,
    updateChooseSegmentArgumentRuleAsync: updateChooseSegmentArgumentRuleAsync,
    fetchChooseSegmentArgumentRulesAsync: fetchChooseSegmentArgumentRulesAsync,
    deleteArgumentRuleAsync: deleteArgumentRuleAsync,
    fetchDestinationsAsync: fetchDestinationsAsync,
    createDestinationAsync: createDestinationAsync,
    removeDestinationAsync: removeDestinationAsync,
    fetchTriggerAsync: fetchTriggerAsync,
    setTriggerAsync: setTriggerAsync,
    fetchLearningFeaturesAsync: fetchLearningFeaturesAsync,
    setLearningFeaturesAsync: setLearningFeaturesAsync,
    fetchLearningMetricsAsync: fetchLearningMetricsAsync,
    setLearningMetricsAsync: setLearningMetricsAsync,
    fetchStatisticsAsync: fetchStatisticsAsync,
    fetchReportImageBlobUrlAsync: fetchReportImageBlobUrlAsync,
    fetchPerformanceAsync: fetchPerformanceAsync,
    fetchPromotionOptimiserAsync: fetchPromotionOptimiserAsync,
    setAllPromotionOptimiserWeightsAsync: setAllPromotionOptimiserWeightsAsync,
    setPromotionOptimiserWeightAsync: setPromotionOptimiserWeightAsync,
    setUseOptimiserAsync: setUseOptimiserAsync,
    fetchCampaignChannelsAsync: fetchCampaignChannelsAsync,
    addCampaignChannelAsync: addCampaignChannelAsync,
    removeCampaignChannelAsync: removeCampaignChannelAsync,
    fetchPromotionsRecommendationAsync: fetchPromotionsRecommendationAsync,
    fetchOffersAsync: fetchOffersAsync,
    fetchARPOReportAsync: fetchARPOReportAsync,
    fetchOfferConversionRateReportAsync: fetchOfferConversionRateReportAsync
});

let authConfig = undefined; // caches this because it rarely change
const fetchAuth0ConfigurationAsync = async () => {
    if (!authConfig) {
        const result = await executeFetch({
            path: "api/reactConfig/auth0",
        });
        authConfig = result;
    }
    return authConfig;
};
let config = undefined;
const fetchConfigurationAsync = async () => {
    if (!config) {
        const result = await executeFetch({
            token: "",
            path: "api/reactConfig",
        });
        config = result;
    }
    return config;
};
const fetchHostingAsync$1 = async () => {
    return await executeFetch({
        path: "api/reactConfig/hosting",
        method: "get",
    });
};

var reactConfigApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchAuth0ConfigurationAsync: fetchAuth0ConfigurationAsync,
    fetchConfigurationAsync: fetchConfigurationAsync,
    fetchHostingAsync: fetchHostingAsync$1
});

const getPropertiesAsync$2 = async ({ api, token, id }) => {
    return await executeFetch({
        path: `api/${api}/${id}/Properties`,
        token,
    });
};
const setPropertiesAsync$2 = async ({ api, token, id, properties }) => {
    return await executeFetch({
        path: `api/${api}/${id}/Properties`,
        token,
        method: "post",
        body: properties,
    });
};

console.warn("Deprecation Notice: Recommendable Items are replaced by Promotions.");
const fetchItemsAsync = async ({ token, page, searchTerm, }) => {
    return await executeFetch({
        path: "api/RecommendableItems",
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
const fetchItemAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/RecommendableItems/${id}`,
        token,
    });
};
const createItemAsync = async ({ token, item, }) => {
    return await executeFetch({
        path: "api/RecommendableItems",
        token,
        method: "post",
        body: item,
    });
};
const updateItemAsync = async ({ token, id, item, }) => {
    return await executeFetch({
        path: `api/RecommendableItems/${id}`,
        token,
        method: "post",
        body: item,
    });
};
const deleteItemAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/RecommendableItems/${id}`,
        token,
        method: "delete",
    });
};
const getPropertiesAsync$1 = async ({ token, id }) => {
    return await getPropertiesAsync$2({
        token,
        id,
        api: "RecommendableItems",
    });
};
const setPropertiesAsync$1 = async ({ token, id, properties, }) => {
    return await setPropertiesAsync$2({
        token,
        id,
        properties,
        api: "RecommendableItems",
    });
};

var recommendableItemsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchItemsAsync: fetchItemsAsync,
    fetchItemAsync: fetchItemAsync,
    createItemAsync: createItemAsync,
    updateItemAsync: updateItemAsync,
    deleteItemAsync: deleteItemAsync,
    getPropertiesAsync: getPropertiesAsync$1,
    setPropertiesAsync: setPropertiesAsync$1
});

const fetchPromotionsAsync = async ({ token, page, searchTerm, promotionType, benefitType, weeksAgo, }) => {
    return await executeFetch({
        path: "api/Promotions",
        token,
        page,
        query: {
            "q.term": searchTerm,
            "q.weeksAgo": weeksAgo,
            promotionType: promotionType,
            benefitType: benefitType,
        },
    });
};
const fetchPromotionAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Promotions/${id}`,
        token,
    });
};
const createPromotionAsync = async ({ token, promotion, }) => {
    return await executeFetch({
        path: "api/Promotions",
        token,
        method: "post",
        body: promotion,
    });
};
const updatePromotionAsync = async ({ token, id, promotion, }) => {
    return await executeFetch({
        path: `api/Promotions/${id}`,
        token,
        method: "post",
        body: promotion,
    });
};
const deletePromotionAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Promotions/${id}`,
        token,
        method: "delete",
    });
};
const getPropertiesAsync = async ({ token, id }) => {
    return await getPropertiesAsync$2({
        token,
        id,
        api: "Promotions",
    });
};
const setPropertiesAsync = async ({ token, id, properties, }) => {
    return await setPropertiesAsync$2({
        token,
        id,
        properties,
        api: "Promotions",
    });
};

var promotionsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchPromotionsAsync: fetchPromotionsAsync,
    fetchPromotionAsync: fetchPromotionAsync,
    createPromotionAsync: createPromotionAsync,
    updatePromotionAsync: updatePromotionAsync,
    deletePromotionAsync: deletePromotionAsync,
    getPropertiesAsync: getPropertiesAsync,
    setPropertiesAsync: setPropertiesAsync
});

const fetchReportsAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/Reports",
        token,
    });
};
const downloadReportAsync = async ({ token, reportName }) => {
    return await executeFetch({
        path: "api/reports/download",
        token,
        query: {
            report: reportName,
        },
    });
};

var reportsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchReportsAsync: fetchReportsAsync,
    downloadReportAsync: downloadReportAsync
});

const fetchSegmentsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/Segments",
        token,
        page,
    });
};
const fetchSegmentAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/segments/${id}`,
        token,
    });
};
const createSegmentAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/Segments",
        token,
        method: "post",
        body: payload,
    });
};
const deleteSegmentAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Segments/${id}`,
        token,
        method: "delete",
    });
};
const addCustomerAsync = async ({ token, id, customerId }) => {
    return await executeFetch({
        path: `api/Segments/${id}/Customers/${customerId}`,
        token,
        method: "post",
    });
};
const removeCustomerAsync = async ({ token, id, customerId }) => {
    return await executeFetch({
        path: `api/Segments/${id}/Customers/${customerId}`,
        token,
        method: "delete",
    });
};
const fetchSegmentCustomersAsync = async ({ token, page, id, searchTerm, weeksAgo, }) => {
    return await executeFetch({
        path: `api/Segments/${id}/Customers`,
        token,
        page,
        query: {
            "q.term": searchTerm,
            "q.weeksAgo": weeksAgo,
        },
    });
};
const fetchSegmentEnrolmentRulesAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Segments/${id}/MetricEnrolmentRules`,
        token,
    });
};
const addSegmentEnrolmentRuleAsync = async ({ token, id, payload }) => {
    return await executeFetch({
        path: `api/Segments/${id}/MetricEnrolmentRules`,
        token,
        method: "post",
        body: payload,
    });
};
const removeSegmentEnrolmentRuleAsync = async ({ token, id, ruleId, }) => {
    return await executeFetch({
        path: `api/Segments/${id}/MetricEnrolmentRules/${ruleId}`,
        token,
        method: "delete",
    });
};

var segmentsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchSegmentsAsync: fetchSegmentsAsync,
    fetchSegmentAsync: fetchSegmentAsync,
    createSegmentAsync: createSegmentAsync,
    deleteSegmentAsync: deleteSegmentAsync,
    addCustomerAsync: addCustomerAsync,
    removeCustomerAsync: removeCustomerAsync,
    fetchSegmentCustomersAsync: fetchSegmentCustomersAsync,
    fetchSegmentEnrolmentRulesAsync: fetchSegmentEnrolmentRulesAsync,
    addSegmentEnrolmentRuleAsync: addSegmentEnrolmentRuleAsync,
    removeSegmentEnrolmentRuleAsync: removeSegmentEnrolmentRuleAsync
});

const fetchTouchpointsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/Touchpoints",
        token,
        page,
    });
};
const fetchTouchpointAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/touchpoints/${id}`,
        token,
    });
};
const createTouchpointMetadataAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/Touchpoints",
        token,
        method: "post",
        body: payload,
    });
};
const fetchTrackedUserTouchpointsAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/trackedusers/${id}/touchpoints`,
        token,
    });
};
const fetchTrackedUsersInTouchpointAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/touchpoints/${id}/trackedusers`,
        token,
    });
};
const createTrackedUserTouchpointAsync = async ({ token, id, touchpointCommonId, payload, }) => {
    return await executeFetch({
        path: `api/trackedusers/${id}/touchpoints/${touchpointCommonId}`,
        token,
        method: "post",
        body: payload,
    });
};
const fetchTrackedUserTouchpointValuesAsync = async ({ token, id, touchpointCommonId, }) => {
    return await executeFetch({
        path: `api/trackedusers/${id}/touchpoints/${touchpointCommonId}`,
        token,
    });
};

var touchpointsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchTouchpointsAsync: fetchTouchpointsAsync,
    fetchTouchpointAsync: fetchTouchpointAsync,
    createTouchpointMetadataAsync: createTouchpointMetadataAsync,
    fetchTrackedUserTouchpointsAsync: fetchTrackedUserTouchpointsAsync,
    fetchTrackedUsersInTouchpointAsync: fetchTrackedUsersInTouchpointAsync,
    createTrackedUserTouchpointAsync: createTrackedUserTouchpointAsync,
    fetchTrackedUserTouchpointValuesAsync: fetchTrackedUserTouchpointValuesAsync
});

// backwards compatible shim TrackedUser => Customer
const fetchTrackedUsersAsync = fetchCustomersAsync;
const updateMergePropertiesAsync = updateMergePropertiesAsync$1;
const fetchTrackedUserAsync = fetchCustomerAsync;
const fetchUniqueTrackedUserActionGroupsAsync = fetchUniqueCustomerActionGroupsAsync;
const fetchLatestRecommendationsAsync = fetchLatestRecommendationsAsync$1;
const fetchTrackedUserActionAsync = fetchCustomerActionAsync;
const uploadUserDataAsync = uploadUserDataAsync$1;
const createOrUpdateTrackedUserAsync = createOrUpdateCustomerAsync;
const fetchTrackedUsersActionsAsync = fetchCustomersActionsAsync;
const deleteCustomerAsync = deleteCustomerAsync$1;

var trackedUsersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchTrackedUsersAsync: fetchTrackedUsersAsync,
    updateMergePropertiesAsync: updateMergePropertiesAsync,
    fetchTrackedUserAsync: fetchTrackedUserAsync,
    fetchUniqueTrackedUserActionGroupsAsync: fetchUniqueTrackedUserActionGroupsAsync,
    fetchLatestRecommendationsAsync: fetchLatestRecommendationsAsync,
    fetchTrackedUserActionAsync: fetchTrackedUserActionAsync,
    uploadUserDataAsync: uploadUserDataAsync,
    createOrUpdateTrackedUserAsync: createOrUpdateTrackedUserAsync,
    fetchTrackedUsersActionsAsync: fetchTrackedUsersActionsAsync,
    deleteCustomerAsync: deleteCustomerAsync
});

const fetchCurrentTenantAsync = async ({ token, }) => {
    return await executeFetch({
        path: "api/tenants/current",
        token,
        method: "get",
    });
};
const fetchAccountAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Tenants/${id}/Account`,
        token,
        method: "get",
    });
};
const fetchHostingAsync = async ({ token, }) => {
    return await executeFetch({
        path: "api/Tenants/Hosting",
        token,
        method: "get",
    });
};
const fetchCurrentTenantMembershipsAsync = async ({ token, }) => {
    return await executeFetch({
        path: "api/tenants/current/memberships",
        token,
        method: "get",
    });
};
const createTenantMembershipAsync = async ({ token, email, }) => {
    return await executeFetch({
        path: "api/tenants/current/memberships",
        token,
        method: "post",
        body: { email },
    });
};

var tenantsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchCurrentTenantAsync: fetchCurrentTenantAsync,
    fetchAccountAsync: fetchAccountAsync,
    fetchHostingAsync: fetchHostingAsync,
    fetchCurrentTenantMembershipsAsync: fetchCurrentTenantMembershipsAsync,
    createTenantMembershipAsync: createTenantMembershipAsync
});

const fetchRewardSelectorsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/RewardSelectors",
        token,
        page,
    });
};
const fetchRewardSelectorAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/RewardSelectors/${id}`,
        token,
        page,
    });
};
const deleteRewardSelectorAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/RewardSelectors/${id}`,
        token,
        method: "delete",
    });
};
const createRewardSelectorAsync = async ({ token, entity }) => {
    return await executeFetch({
        path: "api/RewardSelectors",
        token,
        method: "post",
        body: entity,
    });
};

var rewardSelectorsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchRewardSelectorsAsync: fetchRewardSelectorsAsync,
    fetchRewardSelectorAsync: fetchRewardSelectorAsync,
    deleteRewardSelectorAsync: deleteRewardSelectorAsync,
    createRewardSelectorAsync: createRewardSelectorAsync
});

export { activityFeedApi as activityFeed, apiKeyApi as apiKeys, axiosInstance, businessesApi as businesses, channelsApi as channels, customersApi as customers, dataSummaryApi as dataSummary, deploymentApi as deployment, environmentsApi as environments, errorHandling, eventsApi as events, featureGeneratorsApi as featureGenerators, featuresApi as features, integratedSystemsApi as integratedSystems, itemsRecommendersApi as itemsRecommenders, metricGeneratorsApi as metricGenerators, metricsApi as metrics, modelRegistrationsApi as modelRegistrations, index as models, parameterSetCampaignsApi as parameterSetCampaigns, parameterSetRecommendersApi as parameterSetRecommenders, parametersApi as parameters, profileApi as profile, promotionsApi as promotions, promotionsCampaignsApi as promotionsCampaigns, promotionsRecommendersApi as promotionsRecommenders, reactConfigApi as reactConfig, recommendableItemsApi as recommendableItems, reportsApi as reports, rewardSelectorsApi as rewardSelectors, segmentsApi as segments, setBaseUrl, setDefaultApiKey, setDefaultEnvironmentId$1 as setDefaultEnvironmentId, setTenant, tenantsApi as tenants, touchpointsApi as touchpoints, trackedUsersApi as trackedUsers };
