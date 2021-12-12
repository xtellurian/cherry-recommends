(function (global, factory) {
  typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports) :
  typeof define === 'function' && define.amd ? define(['exports'], factory) :
  (global = typeof globalThis !== 'undefined' ? globalThis : global || self, factory((global.cherry = global.cherry || {}, global.cherry.ai = {})));
}(this, (function (exports) { 'use strict';

  let storedBaseUrl = "";

  const setBaseUrl = (baseUrl) => {
    storedBaseUrl = baseUrl.trim("/");
  };

  const getUrl = (path) => `${storedBaseUrl}/${path}`;

  let defaltEnvironmentId = null;
  const setDefaultEnvironmentId$1 = (e) => {
    defaltEnvironmentId = e;
  };
  let defaultApiKey = null;
  const setDefaultApiKey = (k) => {
    defaultApiKey = k;
  };

  const defaultHeaders$1 = { "Content-Type": "application/json" };

  const headers = (token, apiKey) => {
    let headers = { ...defaultHeaders$1 };
    if (token) {
      headers.Authorization = `Bearer ${token}`;
    }
    if (apiKey) {
      headers["x-api-key"] = `${apiKey}`; // ensure its a string
    } else if (defaultApiKey) {
      headers["x-api-key"] = `${defaultApiKey}`; // ensure its a string
    }
    if (defaltEnvironmentId) {
      headers["x-environment"] = defaltEnvironmentId;
    }
    return headers;
  };

  // type ErrorHandler = (response: Response) => Promise<any>;

  const defaultErrorResponseHandler = async (response) => {
    const json = await response.json();
    console.log(`Server responded: ${response.statusText}`);
    console.log(json);
    if (response.status >= 500) {
      return { error: json };
    } else if (response.status >= 400) {
      throw json;
    }
  };

  let errorResponseHandler = defaultErrorResponseHandler;

  const setErrorResponseHandler = (errorHandler) => {
    errorResponseHandler = errorHandler;
  };

  // this function is called in api.js functions.
  const handleErrorResponse = async (response) => {
    console.log("SDK is handling an error response");
    return await errorResponseHandler(response);
  };

  // the below all function as handlers of a fetch promise rejected
  const defaultErrorFetchHandler = (ex) => {
    throw ex;
  };

  let errorFetchHandler = defaultErrorFetchHandler;

  const handleErrorFetch = (ex) => {
    errorFetchHandler(ex);
  };

  const setErrorFetchHandler = (handler) => {
    errorFetchHandler = handler;
  };

  var errorHandling = /*#__PURE__*/Object.freeze({
    __proto__: null,
    setErrorResponseHandler: setErrorResponseHandler,
    handleErrorResponse: handleErrorResponse,
    handleErrorFetch: handleErrorFetch,
    setErrorFetchHandler: setErrorFetchHandler
  });

  const info = (message) => {
    console.log(`INFO: ${message}`);
  };
  const error = (error) => {
    console.log(`ERROR: ${error}`);
  };

  var logger = {
    info,
    error,
  };

  const executeFetch = async ({
    token,
    apiKey,
    path,
    page,
    body,
    method,
    query,
  }) => {
    const url = getUrl(path);
    const q = new URLSearchParams();
    for (const [key, value] of Object.entries(query || {})) {
      if (key && value) {
        q.append(key, value);
      }
    }
    if (page) {
      q.append("p.page", `${page}`);
    }
    if (apiKey) {
      q.append("apiKey", `${apiKey}`);
    }
    const qs = q.toString();
    const fullUrl = `${url}?${qs}`;
    logger.info(`Executing Fetch ${fullUrl}`);
    let response;
    try {
      response = await fetch(fullUrl, {
        headers: headers(token),
        method: method || "get",
        body: JSON.stringify(body),
      });
    } catch (ex) {
      return handleErrorFetch(ex);
    }
    if (response.ok) {
      return await response.json();
    } else {
      logger.error("Response was not OK.");
      return await handleErrorResponse(response);
    }
  };

  const fetchApiKeysAsync = async ({ token, page }) => {
    return await executeFetch({
      path: "api/apiKeys",
      token,
      page,
    });
  };

  const createApiKeyAsync = async ({ token, payload }) => {
    return await executeFetch({
      path: "api/apiKeys",
      token,
      method: "post",
      body: payload,
    });
  };

  const exchangeApiKeyAsync = async ({ apiKey }) => {
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

  const fetchEventSummaryAsync = async ({ token }) => {
    return await executeFetch({
      path: "api/datasummary/events",
      token,
    });
  };

  const fetchEventTimelineAsync = async ({ token, kind, eventType }) => {
    return await executeFetch({
      path: `api/datasummary/events/timeline/${kind}/${eventType}`,
      token,
    });
  };

  const fetchDashboardAsync = async ({ token, scope }) => {
    return await executeFetch({
      path: "api/datasummary/dashboard",
      token,
      query: { scope },
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
    fetchEventTimelineAsync: fetchEventTimelineAsync,
    fetchDashboardAsync: fetchDashboardAsync,
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

  const ConsumeRecommendation = "ConsumeRecommendation";

  const fetchEventAsync = async ({ id, token }) => {
    return await executeFetch({
      token,
      path: `api/events/${id}`,
    });
  };

  const createEventsAsync = async ({ apiKey, token, events }) => {
    return await executeFetch({
      path: "api/events",
      method: "post",
      token,
      apiKey,
      body: events,
    });
  };

  const fetchTrackedUsersEventsAsync = async ({
    token,
    id,
    useInternalId,
  }) => {
    return await executeFetch({
      path: `api/TrackedUsers/${id}/events`,
      token,
      query: {
        useInternalId,
      },
    });
  };

  // useful extension methods to create certain event kinds
  const createRecommendationConsumedEventAsync = async ({
    token,
    commonUserId,
    correlatorId,
  }) => {
    const payload = {
      commonUserId,
      eventId: `recommendation-${correlatorId}-${new Date().getTime()}`,
      recommendationCorrelatorId: correlatorId,
      kind: ConsumeRecommendation,
      eventType: "generated",
    };
    return await createEventsAsync({ token, events: [payload] });
  };

  var index$1 = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchEventAsync: fetchEventAsync,
    createEventsAsync: createEventsAsync,
    fetchTrackedUsersEventsAsync: fetchTrackedUsersEventsAsync,
    createRecommendationConsumedEventAsync: createRecommendationConsumedEventAsync
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

  const fetchFeatureTrackedUserFeaturesAsync = async ({
    token,
    page,
    id,
  }) => {
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

  const fetchTrackedUserFeatureValuesAsync = async ({
    token,
    id,
    feature,
    version,
  }) => {
    return await executeFetch({
      path: `api/TrackedUsers/${id}/features/${feature}`,
      token,
      query: {
        version,
      },
    });
  };

  const fetchDestinationsAsync$3 = async ({ token, id }) => {
    return await executeFetch({
      path: `api/features/${id}/Destinations`,
      token,
    });
  };

  const createDestinationAsync$3 = async ({ token, id, destination }) => {
    return await executeFetch({
      path: `api/features/${id}/Destinations`,
      token,
      method: "post",
      body: destination,
    });
  };

  const deleteDestinationAsync = async ({ token, id, destinationId }) => {
    return await executeFetch({
      path: `api/features/${id}/Destinations/${destinationId}`,
      token,
      method: "delete",
    });
  };

  const fetchGeneratorsAsync = async ({ token, id }) => {
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
    fetchDestinationsAsync: fetchDestinationsAsync$3,
    createDestinationAsync: createDestinationAsync$3,
    deleteDestinationAsync: deleteDestinationAsync,
    fetchGeneratorsAsync: fetchGeneratorsAsync
  });

  const fetchIntegratedSystemsAsync = async ({ token, page }) => {
    return await executeFetch({
      path: "api/IntegratedSystems",
      token,
      page,
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

  const createWebhookReceiverAsync = async ({
    token,
    id,
    useSharedSecret,
  }) => {
    return await executeFetch({
      path: `api/integratedSystems/${id}/webhookreceivers`,
      token,
      method: "post",
      body: {}, // empty, no args
      query: {
        useSharedSecret,
      },
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
    createWebhookReceiverAsync: createWebhookReceiverAsync
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

  const invokeModelAsync = async ({ token, modelId, features }) => {
    return await executeFetch({
      path: `api/ModelRegistrations/${modelId}/invoke`,
      token,
      method: "post",
      body: features,
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

  const fetchLinkedRegisteredModelAsync$2 = async ({
    recommenderApiName,
    token,
    id,
  }) => {
    return await executeFetch({
      path: `api/recommenders/${recommenderApiName}/${id}/ModelRegistration`,
      token,
    });
  };

  const createLinkedRegisteredModelAsync = async ({
    recommenderApiName,
    token,
    id,
    modelId,
  }) => {
    return await executeFetch({
      path: `api/recommenders/${recommenderApiName}/${id}/ModelRegistration`,
      token,
      method: "post",
      body: { modelId },
    });
  };

  const fetchRecommenderTrackedUserActionsAsync$2 = async ({
    recommenderApiName,
    revenueOnly,
    token,
    id,
    page,
  }) => {
    return await executeFetch({
      path: `api/recommenders/${recommenderApiName}/${id}/TrackedUserActions`,
      token,
      page,
      query: {
        revenueOnly,
      },
    });
  };

  const updateErrorHandlingAsync$2 = async ({
    recommenderApiName,
    token,
    id,
    errorHandling,
  }) => {
    return await executeFetch({
      path: `api/recommenders/${recommenderApiName}/${id}/ErrorHandling`,
      token,
      method: "post",
      body: errorHandling,
    });
  };

  const fetchRecommenderTargetVariableValuesAsync = async ({
    recommenderApiName,
    name,
    token,
    id,
  }) => {
    return await executeFetch({
      path: `api/recommenders/${recommenderApiName}/${id}/TargetVariableValues`,
      token,
      query: {
        name,
      },
    });
  };

  const createRecommenderTargetVariableValueAsync = async ({
    recommenderApiName,
    targetVariableValue,
    token,
    id,
  }) => {
    return await executeFetch({
      path: `api/recommenders/${recommenderApiName}/${id}/TargetVariableValues`,
      token,
      method: "post",
      body: targetVariableValue,
    });
  };

  const fetchRecommenderInvokationLogsAsync = async ({
    recommenderApiName,
    token,
    id,
    page,
  }) => {
    return await executeFetch({
      path: `api/recommenders/${recommenderApiName}/${id}/InvokationLogs`,
      page,
      token,
    });
  };

  const setArgumentsAsync$2 = async ({
    recommenderApiName,
    token,
    id,
    args,
  }) => {
    return await executeFetch({
      path: `api/recommenders/${recommenderApiName}/${id}/Arguments`,
      token,
      method: "post",
      body: args,
    });
  };

  const setSettingsAsync$2 = async ({
    recommenderApiName,
    token,
    id,
    settings,
  }) => {
    return await executeFetch({
      path: `api/recommenders/${recommenderApiName}/${id}/Settings`,
      token,
      method: "post",
      body: settings,
    });
  };

  const fetchDestinationsAsync$2 = async ({
    recommenderApiName,
    token,
    id,
  }) => {
    return await executeFetch({
      token,
      path: `api/recommenders/${recommenderApiName}/${id}/Destinations`,
    });
  };

  const createDestinationAsync$2 = async ({
    recommenderApiName,
    token,
    id,
    destination,
  }) => {
    return await executeFetch({
      token,
      path: `api/recommenders/${recommenderApiName}/${id}/Destinations`,
      method: "post",
      body: destination,
    });
  };

  const removeDestinationAsync$2 = async ({
    recommenderApiName,
    token,
    id,
    destinationId,
  }) => {
    return await executeFetch({
      path: `api/recommenders/${recommenderApiName}/${id}/Destinations/${destinationId}`,
      token,
      method: "delete",
    });
  };

  const setTriggerAsync$2 = async ({
    recommenderApiName,
    token,
    id,
    trigger,
  }) => {
    return await executeFetch({
      path: `api/recommenders/${recommenderApiName}/${id}/TriggerCollection`,
      token,
      method: "post",
      body: trigger,
    });
  };

  const fetchTriggerAsync$2 = async ({ recommenderApiName, token, id }) => {
    return await executeFetch({
      token,
      path: `api/recommenders/${recommenderApiName}/${id}/TriggerCollection`,
    });
  };

  const fetchLearningFeaturesAsync$2 = async ({
    recommenderApiName,
    token,
    id,
    useInternalId,
  }) => {
    return await executeFetch({
      token,
      query: { useInternalId },
      path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
    });
  };

  const setLearningFeaturesAsync$2 = async ({
    recommenderApiName,
    token,
    id,
    useInternalId,
    featureIds,
  }) => {
    return await executeFetch({
      path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
      token,
      method: "post",
      query: { useInternalId },
      body: { featureIds, useInternalId },
    });
  };

  const recommenderApiName$1 = "ParameterSetRecommenders";

  const fetchParameterSetRecommendersAsync = async ({ token, page }) => {
    return await executeFetch({
      path: "api/recommenders/ParameterSetRecommenders",
      token,
      page,
    });
  };

  const fetchParameterSetRecommenderAsync = async ({
    token,
    id,
    searchTerm,
  }) => {
    return await executeFetch({
      path: `api/recommenders/ParameterSetRecommenders/${id}`,
      token,
      query: {
        "q.term": searchTerm,
      },
    });
  };

  const createParameterSetRecommenderAsync = async ({
    token,
    payload,
  }) => {
    return await executeFetch({
      path: "api/recommenders/ParameterSetRecommenders",
      token,
      method: "post",
      body: payload,
    });
  };

  const deleteParameterSetRecommenderAsync = async ({ token, id }) => {
    return await executeFetch({
      path: `api/recommenders/ParameterSetRecommenders/${id}`,
      token,
      method: "delete",
    });
  };

  const fetchParameterSetRecommendationsAsync = async ({
    token,
    page,
    id,
  }) => {
    return await executeFetch({
      path: `api/recommenders/ParameterSetRecommenders/${id}/recommendations`,
      token,
      page,
    });
  };

  const createLinkRegisteredModelAsync$1 = async ({
    token,
    id,
    modelId,
  }) => {
    return await createLinkedRegisteredModelAsync({
      recommenderApiName: recommenderApiName$1,
      id,
      modelId,
      token,
    });
  };

  const fetchLinkedRegisteredModelAsync$1 = async ({ token, id }) => {
    return await fetchLinkedRegisteredModelAsync$2({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
    });
  };

  const invokeParameterSetRecommenderAsync = async ({
    token,
    id,
    input,
  }) => {
    return await executeFetch({
      path: `api/recommenders/ParameterSetRecommenders/${id}/invoke`,
      token,
      method: "post",
      body: input,
    });
  };

  const fetchInvokationLogsAsync$1 = async ({ id, token, page }) => {
    return await fetchRecommenderInvokationLogsAsync({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
      page,
    });
  };

  const fetchTargetVariablesAsync$1 = async ({ id, token, name }) => {
    return await fetchRecommenderTargetVariableValuesAsync({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
      name,
    });
  };

  const createTargetVariableAsync$1 = async ({
    id,
    token,
    targetVariableValue,
  }) => {
    return await createRecommenderTargetVariableValueAsync({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
      targetVariableValue,
    });
  };

  const updateErrorHandlingAsync$1 = async ({
    id,
    token,
    errorHandling,
  }) => {
    return await updateErrorHandlingAsync$2({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
      errorHandling,
    });
  };

  const setSettingsAsync$1 = async ({ id, token, settings }) => {
    return await setSettingsAsync$2({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
      settings,
    });
  };

  const fetchRecommenderTrackedUserActionsAsync$1 = async ({
    id,
    token,
    page,
    revenueOnly,
  }) => {
    return await fetchRecommenderTrackedUserActionsAsync$2({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
      page,
      revenueOnly,
    });
  };

  const setArgumentsAsync$1 = async ({ id, token, args }) => {
    return await setArgumentsAsync$2({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
      args,
    });
  };

  const fetchDestinationsAsync$1 = async ({ id, token }) => {
    return await fetchDestinationsAsync$2({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
    });
  };

  const createDestinationAsync$1 = async ({ id, token, destination }) => {
    return await createDestinationAsync$2({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
      destination,
    });
  };

  const removeDestinationAsync$1 = async ({ id, token, destinationId }) => {
    return await removeDestinationAsync$2({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
      destinationId,
    });
  };

  const fetchTriggerAsync$1 = async ({ id, token }) => {
    return await fetchTriggerAsync$2({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
    });
  };

  const setTriggerAsync$1 = async ({ id, token, trigger }) => {
    return await setTriggerAsync$2({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
      trigger,
    });
  };

  const fetchLearningFeaturesAsync$1 = async ({
    id,
    token,
    useInternalId,
  }) => {
    return await fetchLearningFeaturesAsync$2({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
      useInternalId,
    });
  };

  const setLearningFeaturesAsync$1 = async ({
    id,
    token,
    featureIds,
    useInternalId,
  }) => {
    return await setLearningFeaturesAsync$2({
      recommenderApiName: recommenderApiName$1,
      id,
      token,
      useInternalId,
      featureIds,
    });
  };

  var parameterSetRecommendersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchParameterSetRecommendersAsync: fetchParameterSetRecommendersAsync,
    fetchParameterSetRecommenderAsync: fetchParameterSetRecommenderAsync,
    createParameterSetRecommenderAsync: createParameterSetRecommenderAsync,
    deleteParameterSetRecommenderAsync: deleteParameterSetRecommenderAsync,
    fetchParameterSetRecommendationsAsync: fetchParameterSetRecommendationsAsync,
    createLinkRegisteredModelAsync: createLinkRegisteredModelAsync$1,
    fetchLinkedRegisteredModelAsync: fetchLinkedRegisteredModelAsync$1,
    invokeParameterSetRecommenderAsync: invokeParameterSetRecommenderAsync,
    fetchInvokationLogsAsync: fetchInvokationLogsAsync$1,
    fetchTargetVariablesAsync: fetchTargetVariablesAsync$1,
    createTargetVariableAsync: createTargetVariableAsync$1,
    updateErrorHandlingAsync: updateErrorHandlingAsync$1,
    setSettingsAsync: setSettingsAsync$1,
    fetchRecommenderTrackedUserActionsAsync: fetchRecommenderTrackedUserActionsAsync$1,
    setArgumentsAsync: setArgumentsAsync$1,
    fetchDestinationsAsync: fetchDestinationsAsync$1,
    createDestinationAsync: createDestinationAsync$1,
    removeDestinationAsync: removeDestinationAsync$1,
    fetchTriggerAsync: fetchTriggerAsync$1,
    setTriggerAsync: setTriggerAsync$1,
    fetchLearningFeaturesAsync: fetchLearningFeaturesAsync$1,
    setLearningFeaturesAsync: setLearningFeaturesAsync$1
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

  const recommenderApiName = "ItemsRecommenders";

  const fetchItemsRecommendersAsync = async ({ token, page }) => {
    return await executeFetch({
      token,
      path: "api/recommenders/ItemsRecommenders",
      page,
    });
  };

  const fetchItemsRecommenderAsync = async ({ token, id }) => {
    return await executeFetch({
      path: `api/recommenders/ItemsRecommenders/${id}`,
      token,
    });
  };

  const fetchItemsRecommendationsAsync = async ({ token, page, id }) => {
    return await executeFetch({
      token,
      path: `api/recommenders/ItemsRecommenders/${id}/Recommendations`,
      page,
    });
  };

  const deleteItemsRecommenderAsync = async ({ token, id }) => {
    return await executeFetch({
      path: `api/recommenders/ItemsRecommenders/${id}`,
      token,
      method: "delete",
    });
  };

  const createItemsRecommenderAsync = async ({ token, payload }) => {
    return await executeFetch({
      path: "api/recommenders/ItemsRecommenders",
      token,
      method: "post",
      body: payload,
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

  const removeItemAsync = async ({ token, id, itemId }) => {
    return await executeFetch({
      path: `api/recommenders/ItemsRecommenders/${id}/Items/${itemId}`,
      token,
      method: "post",
    });
  };

  const setDefaultItemAsync = async ({ token, id, itemId }) => {
    return await executeFetch({
      path: `api/recommenders/ItemsRecommenders/${id}/DefaultItem`,
      token,
      method: "post",
      body: { itemId },
    });
  };

  const getDefaultItemAsync = async ({ token, id }) => {
    return await executeFetch({
      path: `api/recommenders/ItemsRecommenders/${id}/DefaultItem`,
      token,
    });
  };

  const createLinkRegisteredModelAsync = async ({
    token,
    id,
    modelId,
  }) => {
    return await createLinkedRegisteredModelAsync({
      recommenderApiName,
      id,
      modelId,
      token,
    });
  };

  const fetchLinkedRegisteredModelAsync = async ({ token, id }) => {
    return await fetchLinkedRegisteredModelAsync$2({
      recommenderApiName,
      id,
      token,
    });
  };

  const invokeItemsRecommenderAsync = async ({ token, id, input }) => {
    return await executeFetch({
      path: `api/recommenders/ItemsRecommenders/${id}/Invoke`,
      token,
      method: "post",
      body: input,
    });
  };

  const fetchInvokationLogsAsync = async ({ id, token, page }) => {
    return await fetchRecommenderInvokationLogsAsync({
      recommenderApiName,
      id,
      token,
      page,
    });
  };

  const fetchTargetVariablesAsync = async ({ id, token, name }) => {
    return await fetchRecommenderTargetVariableValuesAsync({
      recommenderApiName,
      id,
      token,
      name,
    });
  };

  const createTargetVariableAsync = async ({
    id,
    token,
    targetVariableValue,
  }) => {
    return await createRecommenderTargetVariableValueAsync({
      recommenderApiName,
      id,
      token,
      targetVariableValue,
    });
  };

  const updateErrorHandlingAsync = async ({
    id,
    token,
    errorHandling,
  }) => {
    return await updateErrorHandlingAsync$2({
      recommenderApiName,
      id,
      token,
      errorHandling,
    });
  };

  const setSettingsAsync = async ({ id, token, settings }) => {
    return await setSettingsAsync$2({
      recommenderApiName,
      id,
      token,
      settings,
    });
  };

  const fetchRecommenderTrackedUserActionsAsync = async ({
    id,
    token,
    page,
    revenueOnly,
  }) => {
    return await fetchRecommenderTrackedUserActionsAsync$2({
      recommenderApiName,
      id,
      token,
      page,
      revenueOnly,
    });
  };

  const setArgumentsAsync = async ({ id, token, args }) => {
    return await setArgumentsAsync$2({
      recommenderApiName,
      id,
      token,
      args,
    });
  };

  const fetchDestinationsAsync = async ({ id, token }) => {
    return await fetchDestinationsAsync$2({
      recommenderApiName,
      id,
      token,
    });
  };

  const createDestinationAsync = async ({ id, token, destination }) => {
    return await createDestinationAsync$2({
      recommenderApiName,
      id,
      token,
      destination,
    });
  };

  const removeDestinationAsync = async ({ id, token, destinationId }) => {
    return await removeDestinationAsync$2({
      recommenderApiName,
      id,
      token,
      destinationId,
    });
  };

  const fetchTriggerAsync = async ({ id, token }) => {
    return await fetchTriggerAsync$2({
      recommenderApiName,
      id,
      token,
    });
  };

  const setTriggerAsync = async ({ id, token, trigger }) => {
    return await setTriggerAsync$2({
      recommenderApiName,
      id,
      token,
      trigger,
    });
  };

  const fetchLearningFeaturesAsync = async ({
    id,
    token,
    useInternalId,
  }) => {
    return await fetchLearningFeaturesAsync$2({
      recommenderApiName,
      id,
      token,
      useInternalId,
    });
  };

  const setLearningFeaturesAsync = async ({
    id,
    token,
    featureIds,
    useInternalId,
  }) => {
    return await setLearningFeaturesAsync$2({
      recommenderApiName,
      id,
      token,
      useInternalId,
      featureIds,
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
    setDefaultItemAsync: setDefaultItemAsync,
    getDefaultItemAsync: getDefaultItemAsync,
    createLinkRegisteredModelAsync: createLinkRegisteredModelAsync,
    fetchLinkedRegisteredModelAsync: fetchLinkedRegisteredModelAsync,
    invokeItemsRecommenderAsync: invokeItemsRecommenderAsync,
    fetchInvokationLogsAsync: fetchInvokationLogsAsync,
    fetchTargetVariablesAsync: fetchTargetVariablesAsync,
    createTargetVariableAsync: createTargetVariableAsync,
    updateErrorHandlingAsync: updateErrorHandlingAsync,
    setSettingsAsync: setSettingsAsync,
    fetchRecommenderTrackedUserActionsAsync: fetchRecommenderTrackedUserActionsAsync,
    setArgumentsAsync: setArgumentsAsync,
    fetchDestinationsAsync: fetchDestinationsAsync,
    createDestinationAsync: createDestinationAsync,
    removeDestinationAsync: removeDestinationAsync,
    fetchTriggerAsync: fetchTriggerAsync,
    setTriggerAsync: setTriggerAsync,
    fetchLearningFeaturesAsync: fetchLearningFeaturesAsync,
    setLearningFeaturesAsync: setLearningFeaturesAsync
  });

  const defaultHeaders = { "Content-Type": "application/json" };

  let config = null; // caches this because it rarely change
  const fetchAuth0ConfigurationAsync = async () => {
    if (!config) {
      console.log("fetching auth0 from server...");
      const result = await fetch(getUrl("api/reactConfig/auth0"), {
        headers: defaultHeaders,
      });
      config = await result.json();
      console.log(config);
    }
    return config;
  };

  var reactConfigApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchAuth0ConfigurationAsync: fetchAuth0ConfigurationAsync
  });

  const getPropertiesAsync$1 = async ({ api, token, id }) => {
    return await executeFetch({
      path: `api/${api}/${id}/Properties`,
      token,
    });
  };

  const setPropertiesAsync$1 = async ({ api, token, id, properties }) => {
    return await executeFetch({
      path: `api/${api}/${id}/Properties`,
      token,
      method: "post",
      body: properties,
    });
  };

  const fetchItemsAsync = async ({ token, page, searchTerm }) => {
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

  const createItemAsync = async ({ token, item }) => {
    return await executeFetch({
      path: "api/RecommendableItems",
      token,
      method: "post",
      body: item,
    });
  };

  const updateItemAsync = async ({ token, id, item }) => {
    return await executeFetch({
      path: `api/RecommendableItems/${id}`,
      token,
      method: "post",
      body: item,
    });
  };

  const deleteItemAsync = async ({ token, id }) => {
    return await executeFetch({
      path: `api/RecommendableItems/${id}`,
      token,
      method: "delete",
    });
  };

  const getPropertiesAsync = async ({ token, id }) => {
    return await getPropertiesAsync$1({
      token,
      id,
      api: "RecommendableItems",
    });
  };

  const setPropertiesAsync = async ({ token, id, properties }) => {
    return await setPropertiesAsync$1({
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

  var segmentsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchSegmentsAsync: fetchSegmentsAsync,
    fetchSegmentAsync: fetchSegmentAsync,
    createSegmentAsync: createSegmentAsync
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

  const createTrackedUserTouchpointAsync = async ({
    token,
    id,
    touchpointCommonId,
    payload,
  }) => {
    return await executeFetch({
      path: `api/trackedusers/${id}/touchpoints/${touchpointCommonId}`,
      token,
      method: "post",
      body: payload,
    });
  };

  const fetchTrackedUserTouchpointValuesAsync = async ({
    token,
    id,
    touchpointCommonId,
  }) => {
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

  const fetchTrackedUsersAsync = async ({ token, page, searchTerm }) => {
    return await executeFetch({
      path: "api/trackedUsers",
      token,
      page,
      query: {
        "q.term": searchTerm,
      },
    });
  };

  const updateMergePropertiesAsync = async ({ token, id, properties }) => {
    return await executeFetch({
      token,
      path: `api/trackedUsers/${id}/properties`,
      method: "post",
      body: properties,
    });
  };

  const fetchTrackedUserAsync = async ({ token, id, useInternalId }) => {
    return await executeFetch({
      path: `api/trackedUsers/${id}`,
      token,
      query: {
        useInternalId,
      },
    });
  };

  const fetchUniqueTrackedUserActionGroupsAsync = async ({
    token,
    id,
  }) => {
    return await executeFetch({
      token,
      path: `api/trackedUsers/${id}/action-groups`,
    });
  };

  const fetchLatestRecommendationsAsync = async ({ token, id }) => {
    return await executeFetch({
      token,
      path: `api/trackedUsers/${id}/latest-recommendations`,
    });
  };

  const fetchTrackedUserActionAsync = async ({
    token,
    id,
    category,
    actionName,
  }) => {
    return await executeFetch({
      path: `api/trackedUsers/${id}/actions/${category}`,
      token,
      query: {
        actionName,
      },
    });
  };

  const uploadUserDataAsync = async ({ token, payload }) => {
    const payloads = chunkArray(payload.users, MAX_ARRAY).map((users) => ({
      users,
    }));
    const responses = [];
    for (const p of payloads) {
      const response = await executeFetch({
        token,
        path: "api/trackedUsers",
        method: "put",
        body: p,
      });
      if (response.ok) {
        responses.push(await response.json());
      } else {
        return await handleErrorResponse(response);
      }
    }
    return responses;
  };

  const createOrUpdateTrackedUserAsync = async ({ token, user }) => {
    return await executeFetch({
      path: "api/trackedUsers",
      method: "post",
      body: user,
      token
    });
  };

  const fetchTrackedUsersActionsAsync = async ({
    token,
    page,
    id,
    revenueOnly,
  }) => {
    return await executeFetch({
      path: `api/TrackedUsers/${id}/Actions`,
      token,
      page,
      query: {
        revenueOnly: !!revenueOnly,
      },
    });
  };

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
    fetchTrackedUsersActionsAsync: fetchTrackedUsersActionsAsync
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

  const fetchUniqueActionNamesAsync = async ({ token, page, term }) => {
    return await executeFetch({
      path: "api/actions/distinct-groups",
      token,
      page,
      query: {
        term,
      },
    });
  };

  const fetchDistinctGroupsAsync = async ({ token, page, term }) => {
    return await executeFetch({
      path: "api/actions/distinct-groups",
      token,
      page,
      query: {
        term,
      },
    });
  };

  var actionsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchUniqueActionNamesAsync: fetchUniqueActionNamesAsync,
    fetchDistinctGroupsAsync: fetchDistinctGroupsAsync
  });

  // fix missing fetch is node environments
  const fetch$1 = require("node-fetch");
  if (typeof globalThis === "object") {
    // See: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/globalThis
    globalThis.fetch = fetch$1;
  } else if (typeof global === "object") {
    // For Node <12
    global.fetch = fetch$1;
  } else {
    // Everything else is not supported
    throw new Error("Unknown JavaScript environment: Not supported");
  }

  exports.actions = actionsApi;
  exports.apiKeys = apiKeyApi;
  exports.dataSummary = dataSummaryApi;
  exports.deployment = deploymentApi;
  exports.environments = environmentsApi;
  exports.errorHandling = errorHandling;
  exports.events = index$1;
  exports.featureGenerators = featureGeneratorsApi;
  exports.features = featuresApi;
  exports.integratedSystems = integratedSystemsApi;
  exports.itemsRecommenders = itemsRecommendersApi;
  exports.modelRegistrations = modelRegistrationsApi;
  exports.models = index;
  exports.parameterSetRecommenders = parameterSetRecommendersApi;
  exports.parameters = parametersApi;
  exports.profile = profileApi;
  exports.reactConfig = reactConfigApi;
  exports.recommendableItems = recommendableItemsApi;
  exports.reports = reportsApi;
  exports.rewardSelectors = rewardSelectorsApi;
  exports.segments = segmentsApi;
  exports.setBaseUrl = setBaseUrl;
  exports.setDefaultApiKey = setDefaultApiKey;
  exports.setDefaultEnvironmentId = setDefaultEnvironmentId$1;
  exports.touchpoints = touchpointsApi;
  exports.trackedUsers = trackedUsersApi;

  Object.defineProperty(exports, '__esModule', { value: true });

})));
