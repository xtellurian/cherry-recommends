(function (global, factory) {
  typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports) :
  typeof define === 'function' && define.amd ? define(['exports'], factory) :
  (global = typeof globalThis !== 'undefined' ? globalThis : global || self, factory((global.signalbox = global.signalbox || {}, global.signalbox.js = {})));
}(this, (function (exports) { 'use strict';

  const pageQuery = (page) => `p.page=${page || 1}`;

  var paging = /*#__PURE__*/Object.freeze({
    __proto__: null,
    pageQuery: pageQuery
  });

  let storedBaseUrl = "";

  const setBaseUrl = (baseUrl) => {
    storedBaseUrl = baseUrl.trim("/");
  };

  const getUrl = (path) => `${storedBaseUrl}/${path}`;

  const defaultHeaders$9 = { "Content-Type": "application/json" };

  const headers = (token) =>
    !token
      ? defaultHeaders$9
      : { ...defaultHeaders$9, Authorization: `Bearer ${token}` };

  const fetchApiKeys = async ({ success, error, token, page }) => {
    const url = getUrl("api/apiKeys");
    let path = `${url}?${pageQuery(page)}`;

    const response = await fetch(path, {
      headers: headers(token),
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const createApiKey = async ({ success, error, token, name }) => {
    const url = getUrl("api/apiKeys/create");
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify({ name }),
    });
    if (response.ok) {
      const data = await response.json();
      success(data);
    } else {
      error(await response.json());
    }
  };

  const exchangeApiKeyAsync = async ({ apiKey }) => {
    const url = getUrl("api/apiKeys/exchange");
    const response = await fetch(url, {
      headers: headers(),
      method: "post",
      body: JSON.stringify({ apiKey }),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const exchangeApiKey = ({ success, error, apiKey }) => {
    exchangeApiKeyAsync({ apiKey }).then(success).catch(error);
  };

  var apiKeyApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchApiKeys: fetchApiKeys,
    createApiKey: createApiKey,
    exchangeApiKeyAsync: exchangeApiKeyAsync,
    exchangeApiKey: exchangeApiKey
  });

  const fetchEventSummary = async ({ success, error, token }) => {
    const url = getUrl("api/datasummary/events");
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const fetchEventTimeline = async ({
    success,
    error,
    token,
    kind,
    eventType,
  }) => {
    const url = getUrl(`api/datasummary/events/timeline/${kind}/${eventType}`);

    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const fetchDashboard = async ({ success, error, token, scope }) => {
    const url = getUrl(`api/datasummary/dashboard`);

    const response = await fetch(`${url}?scope=${scope}`, {
      headers: headers(token),
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const fetchLatestActionsAsync = async ({ token }) => {
    const url = getUrl(`api/datasummary/actions`);

    const response = await fetch(`${url}`, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  var dataSummaryApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchEventSummary: fetchEventSummary,
    fetchEventTimeline: fetchEventTimeline,
    fetchDashboard: fetchDashboard,
    fetchLatestActionsAsync: fetchLatestActionsAsync
  });

  const defaultHeaders$8 = { "Content-Type": "application/json" };

  const fetchDeploymentConfiguration = async ({
    success,
    error,
    token,
  }) => {
    const url = getUrl(`api/deployment/configuration`);

    const result = await fetch(url, {
      headers: !token
        ? defaultHeaders$8
        : { ...defaultHeaders$8, Authorization: `Bearer ${token}` },
    });
    if (result.ok) {
      success(await result.json());
    } else {
      error(await result.json());
    }
  };

  var deploymentApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchDeploymentConfiguration: fetchDeploymentConfiguration
  });

  const fetchEventAsync = async ({ id, token }) => {
    const url = getUrl(`api/events/${id}`);

    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const createEventsAsync = async ({ token, events }) => {
    const url = getUrl(`api/events`);

    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(events),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchTrackedUsersEventsAsync = async ({ token, id }) => {
    const url = getUrl(`api/TrackedUsers/${id}/events`);

    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };
  const fetchUserEvents = async ({
    success,
    error,
    token,
    commonUserId,
  }) => {
    fetchTrackedUsersEventsAsync({ id: commonUserId, token })
      .then(success)
      .catch(error);
  };

  const logUserEvents = async ({ success, error, token, events }) => {
    const url = getUrl("api/events");
    if (events.some((e) => !e.commonUserId)) {
      error({
        title: "Every Event requires a commonUserId",
      });
      return;
    }
    if (events.some((e) => !e.eventId)) {
      error({
        title: "Every Event requires a unique eventId",
      });
      return;
    }
    if (events.some((e) => !e.eventType)) {
      error({
        title: "Every Event requires an eventType",
      });
      return;
    }
    if (events.some((e) => !e.kind)) {
      error({
        title: "Every Event requires a kind",
      });
      return;
    }

    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(events),
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  var eventsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchEventAsync: fetchEventAsync,
    createEventsAsync: createEventsAsync,
    fetchTrackedUsersEventsAsync: fetchTrackedUsersEventsAsync,
    fetchUserEvents: fetchUserEvents,
    logUserEvents: logUserEvents
  });

  const defaultHeaders$7 = { "Content-Type": "application/json" };

  const fetchExperiments = async ({ success, error, token, page }) => {
    const url = getUrl("api/experiments");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(response.json());
    }
  };

  const createExperiment = async ({ success, error, token, payload }) => {
    const url = getUrl("api/experiments");
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$7
        : { ...defaultHeaders$7, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify(payload),
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const fetchExperimentResults = async ({ success, error, token, id }) => {
    const url = getUrl(`api/experiments/${id}/results`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$7
        : { ...defaultHeaders$7, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const data = await response.json();
      success(data);
    } else {
      error(response.json());
    }
  };

  const fetchRecommendation = async ({
    success,
    error,
    token,
    experimentId,
    features,
    userId,
  }) => {
    const url = getUrl(`api/experiments/${experimentId}/recommendation`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$7
        : { ...defaultHeaders$7, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify({ commonUserId: userId, features }),
    });
    if (response.ok) {
      const data = await response.json();
      success(data);
    } else {
      error(await response.json());
    }
  };

  var experimentsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchExperiments: fetchExperiments,
    createExperiment: createExperiment,
    fetchExperimentResults: fetchExperimentResults,
    fetchRecommendation: fetchRecommendation
  });

  const fetchFeatureGeneratorsAsync = async ({ page, token }) => {
    const url = getUrl(`api/FeatureGenerators?${pageQuery(page)}`);
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const createFeatureGeneratorAsync = async ({ token, payload }) => {
    const url = getUrl(`api/FeatureGenerators`);
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(payload),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const deleteFeatureGeneratorAsync = async ({ token, id }) => {
    const url = getUrl(`api/FeatureGenerators/${id}`);
    const response = await fetch(url, {
      headers: headers(token),
      method: "delete",
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  var featureGeneratorsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchFeatureGeneratorsAsync: fetchFeatureGeneratorsAsync,
    createFeatureGeneratorAsync: createFeatureGeneratorAsync,
    deleteFeatureGeneratorAsync: deleteFeatureGeneratorAsync
  });

  const searchEntities$2 = (term) => {
      if (term) {
        return `q.term=${term}`;
      } else {
        return "";
      }
    };

  const fetchFeaturesAsync = async ({ token, page, searchTerm }) => {
    let url = getUrl("api/features");
    url = `${url}?${pageQuery(page)}`;
    if (searchTerm) {
      url = `${url}&${searchEntities$2(searchTerm)}`;
    }
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchFeatureAsync = async ({ token, id }) => {
    const url = getUrl(`api/features/${id}`);
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchFeatureTrackedUsersAsync = async ({ token, page, id }) => {
    const url = getUrl(`api/Features/${id}/TrackedUsers?${pageQuery(page)}`);
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const createFeatureAsync = async ({ token, feature }) => {
    const url = getUrl(`api/features/`);
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(feature),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const deleteFeatureAsync = async ({ token, id }) => {
    const url = getUrl(`api/features/${id}`);
    const response = await fetch(url, {
      headers: headers(token),
      method: "delete",
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchTrackedUserFeaturesAsync = async ({ token, id }) => {
    const url = getUrl(`api/TrackedUsers/${id}/features`);
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchTrackedUserFeatureValuesAsync = async ({
    token,
    id,
    feature,
    version,
  }) => {
    const url = getUrl(
      `api/TrackedUsers/${id}/features/${feature}?version=${version || ""}`
    );
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  var featuresApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchFeaturesAsync: fetchFeaturesAsync,
    fetchFeatureAsync: fetchFeatureAsync,
    fetchFeatureTrackedUsersAsync: fetchFeatureTrackedUsersAsync,
    createFeatureAsync: createFeatureAsync,
    deleteFeatureAsync: deleteFeatureAsync,
    fetchTrackedUserFeaturesAsync: fetchTrackedUserFeaturesAsync,
    fetchTrackedUserFeatureValuesAsync: fetchTrackedUserFeatureValuesAsync
  });

  const fetchIntegratedSystems = async ({
    success,
    error,
    token,
    page,
  }) => {
    const url = getUrl("api/integratedSystems");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: headers(token),
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const fetchIntegratedSystemAsync = async ({ token, id }) => {
    const url = getUrl(`api/integratedSystems/${id}`);
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };
  const fetchIntegratedSystem = async ({ success, error, token, id }) => {
    fetchIntegratedSystemAsync({ id, token }).then(success).catch(error);
  };

  const renameAsync = async ({ token, id, name }) => {
    const url = getUrl(`api/integratedSystems/${id}/name?name=${name}`);
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const createIntegratedSystemAsync = async ({ token, payload }) => {
    const url = getUrl("api/integratedSystems");
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(payload),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchWebhookReceivers = async ({ success, error, token, id }) => {
    const url = getUrl(`api/integratedSystems/${id}/webhookreceivers`);
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      const data = await response.json();
      success(data);
    } else {
      error(await response.json());
    }
  };

  const createIntegratedSystem = ({ success, error, token, payload }) => {
    createIntegratedSystemAsync({ token, payload }).then(success).catch(error);
  };

  const createWebhookReceiver = async ({
    success,
    error,
    token,
    id,
    useSharedSecret,
  }) => {
    const url = getUrl(`api/integratedSystems/${id}/webhookreceivers`);
    const response = await fetch(`${url}?useSharedSecret=${useSharedSecret}`, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify({}), // body is just empty for this
    });
    if (response.ok) {
      const data = await response.json();
      success(data);
    } else {
      error(await response.json());
    }
  };

  var integratedSystemsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchIntegratedSystems: fetchIntegratedSystems,
    fetchIntegratedSystemAsync: fetchIntegratedSystemAsync,
    fetchIntegratedSystem: fetchIntegratedSystem,
    renameAsync: renameAsync,
    createIntegratedSystemAsync: createIntegratedSystemAsync,
    fetchWebhookReceivers: fetchWebhookReceivers,
    createIntegratedSystem: createIntegratedSystem,
    createWebhookReceiver: createWebhookReceiver
  });

  const fetchModelRegistrations = async ({
    success,
    error,
    token,
    page,
  }) => {
    const url = getUrl("api/ModelRegistrations");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: headers(token),
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const fetchModelRegistration = async ({ success, error, token, id }) => {
    const url = getUrl(`api/ModelRegistrations/${id}`);
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const deleteModelRegistration = async ({
    success,
    error,
    token,
    id,
  }) => {
    const url = getUrl(`api/ModelRegistrations/${id}`);
    const response = await fetch(url, {
      headers: headers(token),
      method: "delete",
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const createModelRegistrationAsync = async ({ token, payload }) => {
    const url = getUrl("api/ModelRegistrations");
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(payload),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const createModelRegistration = ({ success, error, token, payload }) => {
    createModelRegistrationAsync({ token, payload }).then(success).catch(error);
  };

  const invokeModel = async ({
    success,
    error,
    token,
    modelId,
    features,
  }) => {
    const url = getUrl(`api/ModelRegistrations/${modelId}/invoke`);
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(features),
    });
    if (response.ok) {
      const data = await response.json();
      success(data);
    } else {
      error(await response.json());
    }
  };

  var modelRegistrationsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchModelRegistrations: fetchModelRegistrations,
    fetchModelRegistration: fetchModelRegistration,
    deleteModelRegistration: deleteModelRegistration,
    createModelRegistrationAsync: createModelRegistrationAsync,
    createModelRegistration: createModelRegistration,
    invokeModel: invokeModel
  });

  const defaultHeaders$6 = { "Content-Type": "application/json" };

  const invokeGenericModel = async ({
    success,
    error,
    onFinally,
    token,
    id,
    input,
  }) => {
    try {
      const url = getUrl(`api/models/generic/${id}/invoke`);
      const result = await fetch(url, {
        headers: !token
          ? defaultHeaders$6
          : { ...defaultHeaders$6, Authorization: `Bearer ${token}` },
        method: "post",
        body: JSON.stringify(input),
      });
      if (result.ok) {
        success(await result.json());
      } else {
        error(await result.json());
      }
    } finally {
      if (onFinally) {
        onFinally();
      }
    }
  };

  var index = /*#__PURE__*/Object.freeze({
    __proto__: null,
    invokeGenericModel: invokeGenericModel
  });

  const defaultHeaders$5 = { "Content-Type": "application/json" };

  const fetchOffers = async ({ success, error, token, page }) => {
    const url = getUrl("api/offers");
    let path = `${url}?${pageQuery(page)}`;
    const response = await fetch(path, {
      headers: !token
        ? defaultHeaders$5
        : { ...defaultHeaders$5, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const fetchOffer = async ({ success, error, token, id }) => {
    var url = getUrl(`api/offers/${id}`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$5
        : { ...defaultHeaders$5, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const createOffer = async ({ success, error, token, payload }) => {
    var url = getUrl("api/offers");
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$5
        : { ...defaultHeaders$5, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify(payload),
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  var offersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchOffers: fetchOffers,
    fetchOffer: fetchOffer,
    createOffer: createOffer
  });

  const fetchParameters = async ({ success, error, token, page }) => {
    const url = getUrl("api/parameters");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: headers(token),
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const fetchParameter = async ({ success, error, token, id }) => {
    const url = getUrl(`api/parameters/${id}`);
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const deleteParameterAsync = async ({ token, id }) => {
    const url = getUrl(`api/parameters/${id}`);
    const response = await fetch(url, {
      headers: headers(token),
      method: "delete",
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const createParameterAsync = async ({ token, payload }) => {
    const url = getUrl("api/parameters");
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(payload),
    });

    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const createParameter = ({ success, error, token, payload }) => {
    createParameterAsync({ token, payload }).then(success).catch(error);
  };

  var parametersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchParameters: fetchParameters,
    fetchParameter: fetchParameter,
    deleteParameterAsync: deleteParameterAsync,
    createParameterAsync: createParameterAsync,
    createParameter: createParameter
  });

  const fetchLinkedRegisteredModelAsync = async ({
    recommenderApiName,
    token,
    id,
  }) => {
    const url = getUrl(
      `api/recommenders/${recommenderApiName}/${id}/ModelRegistration`
    );
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const createLinkedRegisteredModelAsync = async ({
    recommenderApiName,
    token,
    id,
    modelId,
  }) => {
    const url = getUrl(
      `api/recommenders/${recommenderApiName}/${id}/ModelRegistration`
    );
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify({ modelId }),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const updateErrorHandlingAsync$2 = async ({
    recommenderApiName,
    token,
    id,
    errorHandling,
  }) => {
    const url = getUrl(
      `api/recommenders/${recommenderApiName}/${id}/ErrorHandling`
    );
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(errorHandling),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchRecommenderTargetVariableValuesAsync = async ({
    recommenderApiName,
    name,
    token,
    id,
  }) => {
    const url = getUrl(
      `api/recommenders/${recommenderApiName}/${id}/TargetVariableValues?name=${
      name || ""
    }`
    );
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const createRecommenderTargetVariableValueAsync = async ({
    recommenderApiName,
    targetVariableValue,
    token,
    id,
  }) => {
    const url = getUrl(
      `api/recommenders/${recommenderApiName}/${id}/TargetVariableValues`
    );
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(targetVariableValue),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchRecommenderInvokationLogsAsync = async ({
    recommenderApiName,
    token,
    id,
    page
  }) => {
    const url = getUrl(
      `api/recommenders/${recommenderApiName}/${id}/InvokationLogs?${pageQuery(
      page
    )}`
    );
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchParameterSetRecommendersAsync = async ({ token, page }) => {
    const url = getUrl("api/recommenders/ParameterSetRecommenders");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };
  const fetchParameterSetRecommenders = async ({
    success,
    error,
    token,
    page,
  }) => {
    fetchParameterSetRecommendersAsync({ token, page })
      .then(success)
      .catch(error);
  };

  const fetchParameterSetRecommenderAsync = async ({
    token,
    id,
    searchTerm,
  }) => {
    let url = getUrl(`api/recommenders/ParameterSetRecommenders/${id}`);
    if (searchTerm) {
      url = url + `?${searchEntities(searchTerm)}`;
    }
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };
  const fetchParameterSetRecommender = async ({
    success,
    error,
    token,
    id,
  }) => {
    fetchParameterSetRecommenderAsync({ id, token }).then(success).catch(error);
  };

  const createParameterSetRecommenderAsync = async ({
    token,
    payload,
  }) => {
    const url = getUrl("api/recommenders/ParameterSetRecommenders");
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(payload),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };
  const createParameterSetRecommender = async ({
    success,
    error,
    token,
    payload,
  }) => {
    createParameterSetRecommenderAsync({ token, payload })
      .then(success)
      .catch(error);
  };

  const deleteParameterSetRecommender = async ({
    success,
    error,
    token,
    id,
  }) => {
    const url = getUrl(`api/recommenders/ParameterSetRecommenders/${id}`);
    const response = await fetch(url, {
      headers: headers(token),
      method: "delete",
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const fetchParameterSetRecommendationsAsync = async ({
    token,
    page,
    id,
  }) => {
    const url = getUrl(
      `api/recommenders/ParameterSetRecommenders/${id}/recommendations`
    );
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const createLinkRegisteredModel$1 = async ({
    success,
    error,
    token,
    id,
    modelId,
  }) => {
    createLinkedRegisteredModelAsync({
      recommenderApiName: "ParameterSetRecommenders",
      id,
      modelId,
      token,
    })
      .then(success)
      .catch(error);
  };

  const fetchLinkedRegisteredModel$1 = async ({
    success,
    error,
    token,
    id,
  }) => {
    fetchLinkedRegisteredModelAsync({
      recommenderApiName: "ParameterSetRecommenders",
      id,
      token,
    })
      .then(success)
      .catch(error);
  };

  const invokeParameterSetRecommenderAsync = async ({
    token,
    id,
    version,
    input,
  }) => {
    const url = getUrl(`api/recommenders/ParameterSetRecommenders/${id}/invoke`);
    const result = await fetch(`${url}?version=${version || "default"}`, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(input),
    });
    if (result.ok) {
      return await result.json();
    } else {
      throw await result.json();
    }
  };
  const invokeParameterSetRecommender = ({
    success,
    error,
    onFinally,
    token,
    id,
    version,
    input,
  }) => {
    invokeParameterSetRecommenderAsync({ token, id, version, input })
      .then(success)
      .catch(error)
      .finally(onFinally);
  };

  const fetchInvokationLogsAsync$1 = async ({ id, token, page }) => {
    return await fetchRecommenderInvokationLogsAsync({
      recommenderApiName: "ParameterSetRecommenders",
      id,
      token,
      page,
    });
  };

  const fetchTargetVariablesAsync$1 = async ({ id, token, name }) => {
    return await fetchRecommenderTargetVariableValuesAsync({
      recommenderApiName: "ParameterSetRecommenders",
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
      recommenderApiName: "ParameterSetRecommenders",
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
      recommenderApiName: "ParameterSetRecommenders",
      id,
      token,
      errorHandling,
    });
  };

  var parameterSetRecommendersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchParameterSetRecommendersAsync: fetchParameterSetRecommendersAsync,
    fetchParameterSetRecommenders: fetchParameterSetRecommenders,
    fetchParameterSetRecommenderAsync: fetchParameterSetRecommenderAsync,
    fetchParameterSetRecommender: fetchParameterSetRecommender,
    createParameterSetRecommenderAsync: createParameterSetRecommenderAsync,
    createParameterSetRecommender: createParameterSetRecommender,
    deleteParameterSetRecommender: deleteParameterSetRecommender,
    fetchParameterSetRecommendationsAsync: fetchParameterSetRecommendationsAsync,
    createLinkRegisteredModel: createLinkRegisteredModel$1,
    fetchLinkedRegisteredModel: fetchLinkedRegisteredModel$1,
    invokeParameterSetRecommenderAsync: invokeParameterSetRecommenderAsync,
    invokeParameterSetRecommender: invokeParameterSetRecommender,
    fetchInvokationLogsAsync: fetchInvokationLogsAsync$1,
    fetchTargetVariablesAsync: fetchTargetVariablesAsync$1,
    createTargetVariableAsync: createTargetVariableAsync$1,
    updateErrorHandlingAsync: updateErrorHandlingAsync$1
  });

  const fetchProductRecommendersAsync = async ({ token, page }) => {
    const url = getUrl("api/recommenders/ProductRecommenders");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };
  const fetchProductRecommenders = async ({
    success,
    error,
    token,
    page,
  }) => {
    fetchProductRecommendersAsync({ page, token }).then(success).catch(error);
  };

  const fetchProductRecommender = async ({
    success,
    error,
    token,
    id,
  }) => {
    const url = getUrl(`api/recommenders/ProductRecommenders/${id}`);
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const fetchProductRecommendations = async ({
    success,
    error,
    token,
    page,
    id,
  }) => {
    const url = getUrl(
      `api/recommenders/ProductRecommenders/${id}/recommendations`
    );
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: headers(token),
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const deleteProductRecommender = async ({
    success,
    error,
    token,
    id,
  }) => {
    const url = getUrl(`api/recommenders/ProductRecommenders/${id}`);
    const response = await fetch(url, {
      headers: headers(token),
      method: "delete",
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const createProductRecommenderAsync = async ({ token, payload }) => {
    const url = getUrl("api/recommenders/ProductRecommenders");
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(payload),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };
  const createProductRecommender = async ({
    success,
    error,
    token,
    payload,
    onFinally,
  }) => {
    createProductRecommenderAsync({ token, payload })
      .then(success)
      .catch(error)
      .finally(onFinally);
  };

  const createLinkRegisteredModel = async ({
    success,
    error,
    token,
    id,
    modelId,
  }) => {
    createLinkedRegisteredModelAsync({
      recommenderApiName: "ProductRecommenders",
      id,
      modelId,
      token,
    })
      .then(success)
      .then(error);
  };

  const setDefaultProductAsync = async ({ token, id, productId }) => {
    const url = getUrl(
      `api/recommenders/ProductRecommenders/${id}/DefaultProduct`
    );
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify({ productId }),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const getDefaultProductAsync = async ({ token, id }) => {
    const url = getUrl(
      `api/recommenders/ProductRecommenders/${id}/DefaultProduct`
    );
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchLinkedRegisteredModel = async ({
    success,
    error,
    token,
    id,
  }) => {
    fetchLinkedRegisteredModelAsync({
      recommenderApiName: "ProductRecommenders",
      id,
      token,
    })
      .then(success)
      .catch(error);
  };

  const invokeProductRecommenderAsync = async ({
    token,
    id,
    version,
    input,
  }) => {
    const url = getUrl(`api/recommenders/ProductRecommenders/${id}/invoke`);
    const result = await fetch(`${url}?version=${version || "default"}`, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(input),
    });
    if (result.ok) {
      return await result.json();
    } else {
      throw await result.json();
    }
  };

  const invokeProductRecommender = async ({
    success,
    error,
    onFinally,
    token,
    id,
    version,
    input,
  }) => {
    invokeProductRecommenderAsync({ token, id, version, input })
      .then(success)
      .catch(error)
      .finally(onFinally || (() => console.log()));
  };

  const fetchInvokationLogsAsync = async ({ id, token, page }) => {
    return await fetchRecommenderInvokationLogsAsync({
      recommenderApiName: "ProductRecommenders",
      id,
      token,
      page,
    });
  };

  const fetchTargetVariablesAsync = async ({ id, token, name }) => {
    return await fetchRecommenderTargetVariableValuesAsync({
      recommenderApiName: "ProductRecommenders",
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
      recommenderApiName: "ProductRecommenders",
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
      recommenderApiName: "ProductRecommenders",
      id,
      token,
      errorHandling,
    });
  };

  var productRecommendersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchProductRecommendersAsync: fetchProductRecommendersAsync,
    fetchProductRecommenders: fetchProductRecommenders,
    fetchProductRecommender: fetchProductRecommender,
    fetchProductRecommendations: fetchProductRecommendations,
    deleteProductRecommender: deleteProductRecommender,
    createProductRecommenderAsync: createProductRecommenderAsync,
    createProductRecommender: createProductRecommender,
    createLinkRegisteredModel: createLinkRegisteredModel,
    setDefaultProductAsync: setDefaultProductAsync,
    getDefaultProductAsync: getDefaultProductAsync,
    fetchLinkedRegisteredModel: fetchLinkedRegisteredModel,
    invokeProductRecommenderAsync: invokeProductRecommenderAsync,
    invokeProductRecommender: invokeProductRecommender,
    fetchInvokationLogsAsync: fetchInvokationLogsAsync,
    fetchTargetVariablesAsync: fetchTargetVariablesAsync,
    createTargetVariableAsync: createTargetVariableAsync,
    updateErrorHandlingAsync: updateErrorHandlingAsync
  });

  const defaultHeaders$4 = { "Content-Type": "application/json" };

  const fetchProducts = async ({ success, error, token, page }) => {
    const url = getUrl("api/products");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: !token
        ? defaultHeaders$4
        : { ...defaultHeaders$4, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const fetchProduct = async ({ success, error, token, id }) => {
    const url = getUrl(`api/products/${id}`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$4
        : { ...defaultHeaders$4, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const createProduct = async ({ success, error, token, product }) => {
    const url = getUrl(`api/products/`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$4
        : { ...defaultHeaders$4, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify(product),
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const deleteProduct = async ({ success, error, token, id }) => {
    const url = getUrl(`api/products/${id}`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$4
        : { ...defaultHeaders$4, Authorization: `Bearer ${token}` },
      method: "delete",
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  var productsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchProducts: fetchProducts,
    fetchProduct: fetchProduct,
    createProduct: createProduct,
    deleteProduct: deleteProduct
  });

  const defaultHeaders$3 = { "Content-Type": "application/json" };

  let config = null; // caches this because it rarely change
  const fetchAuth0Configuration = async () => {
    if (!config) {
      console.log("fetching auth0 from server...");
      const result = await fetch(getUrl("api/reactConfig/auth0"), {
        headers: defaultHeaders$3,
      });
      config = await result.json();
      console.log(config);
    }
    return config;
  };

  var reactConfigApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchAuth0Configuration: fetchAuth0Configuration
  });

  const defaultHeaders$2 = { "Content-Type": "application/json" };

  const fetchReports = async ({ success, error, token }) => {
    const url = getUrl("api/reports");
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$2
        : { ...defaultHeaders$2, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const downloadReport = async ({ success, error, token, reportName }) => {
    const url = getUrl("api/reports/download");
    let path = `${url}?report=${reportName}`;

    const response = await fetch(path, {
      headers: !token
        ? defaultHeaders$2
        : { ...defaultHeaders$2, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.blob();
      success(results);
    } else {
      error(await response.json());
    }
  };

  var reportsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchReports: fetchReports,
    downloadReport: downloadReport
  });

  const defaultHeaders$1 = { "Content-Type": "application/json" };

  const fetchSegments = async ({ success, error, token, page }) => {
    const url = getUrl("api/segments");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: !token
        ? defaultHeaders$1
        : { ...defaultHeaders$1, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const fetchSegment = async ({ success, error, token, id }) => {
    const url = getUrl(`api/segments/${id}`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$1
        : { ...defaultHeaders$1, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const createSegment = async ({ success, error, token, payload }) => {
    const url = getUrl("api/segments");
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$1
        : { ...defaultHeaders$1, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify(payload),
    });

    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  var segmentsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchSegments: fetchSegments,
    fetchSegment: fetchSegment,
    createSegment: createSegment
  });

  const defaultHeaders = { "Content-Type": "application/json" };

  const fetchTouchpoints = async ({
    success,
    error,
    token,
    page,
    onFinally,
  }) => {
    try {
      const url = getUrl("api/touchpoints");
      const response = await fetch(`${url}?${pageQuery(page)}`, {
        headers: !token
          ? defaultHeaders
          : { ...defaultHeaders, Authorization: `Bearer ${token}` },
      });
      if (response.ok) {
        const results = await response.json();
        success(results);
      } else {
        error(await response.json());
      }
    } finally {
      if (onFinally) {
        onFinally();
      }
    }
  };

  const fetchTouchpoint = async ({ success, error, token, id }) => {
    const url = getUrl(`api/touchpoints/${id}`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders
        : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const createTouchpointMetadata = async ({
    success,
    error,
    token,
    payload,
    onFinally,
  }) => {
    try {
      const url = getUrl("api/touchpoints");
      const response = await fetch(url, {
        headers: !token
          ? defaultHeaders
          : { ...defaultHeaders, Authorization: `Bearer ${token}` },
        method: "post",
        body: JSON.stringify(payload),
      });
      if (response.ok) {
        const results = await response.json();
        success(results);
      } else {
        error(await response.json());
      }
    } finally {
      if (onFinally) {
        onFinally();
      }
    }
  };

  const fetchTrackedUserTouchpoints = async ({
    success,
    error,
    token,
    id,
  }) => {
    if (!id) {
      error({
        title: "Tracked User ID is required.",
      });
      return;
    }
    const url = getUrl(`api/trackedusers/${id}/touchpoints`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders
        : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const fetchTrackedUsersInTouchpoint = async ({
    success,
    error,
    token,
    id,
  }) => {
    if (!id) {
      error({
        title: "Touchpoint ID is required.",
      });
      return;
    }
    const url = getUrl(`api/touchpoints/${id}/trackedusers`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders
        : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const createTrackedUserTouchpoint = async ({
    success,
    error,
    token,
    id,
    touchpointCommonId,
    payload,
  }) => {
    if (!id || !touchpointCommonId) {
      error({
        title: "Tracked User ID and touchpoint ID are required.",
      });
      return;
    }
    const url = getUrl(
      `api/trackedusers/${id}/touchpoints/${touchpointCommonId}`
    );
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders
        : { ...defaultHeaders, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify(payload),
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const fetchTrackedUserTouchpointValues = async ({
    success,
    error,
    token,
    id,
    touchpointCommonId,
    version,
  }) => {
    if (!id || !touchpointCommonId) {
      error({
        title: "Tracked User ID and touchpoint ID are required.",
      });
      return;
    }

    const url = getUrl(
      `api/trackedusers/${id}/touchpoints/${touchpointCommonId}`
    );
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders
        : { ...defaultHeaders, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  var touchpointsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchTouchpoints: fetchTouchpoints,
    fetchTouchpoint: fetchTouchpoint,
    createTouchpointMetadata: createTouchpointMetadata,
    fetchTrackedUserTouchpoints: fetchTrackedUserTouchpoints,
    fetchTrackedUsersInTouchpoint: fetchTrackedUsersInTouchpoint,
    createTrackedUserTouchpoint: createTrackedUserTouchpoint,
    fetchTrackedUserTouchpointValues: fetchTrackedUserTouchpointValues
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

  const searchEntities$1 = (term) => {
    if (term) {
      return `&q.term=${term}`;
    } else {
      return "";
    }
  };
  const fetchTrackedUsersAsync = async ({ token, page, searchTerm }) => {
    const url = getUrl("api/trackedUsers");
    const response = await fetch(
      `${url}?${pageQuery(page)}${searchEntities$1(searchTerm)}`,
      {
        headers: headers(token),
      }
    );
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };
  const fetchTrackedUsers = async ({
    success,
    error,
    token,
    page,
    searchTerm,
  }) => {
    fetchTrackedUsersAsync({ token, page, searchTerm })
      .then(success)
      .catch(error);
  };

  const updateMergePropertiesAsync = async ({ token, id, properties }) => {
    const url = getUrl(`api/trackedUsers/${id}/properties`);
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(properties),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchTrackedUser = async ({ success, error, token, id }) => {
    const url = getUrl(`api/trackedUsers/${id}`);
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      const trackedUser = await response.json();
      success(trackedUser);
    } else {
      error(await response.json());
    }
  };

  const fetchUniqueTrackedUserActionGroupsAsync = async ({
    token,
    id,
  }) => {
    const url = getUrl(`api/trackedUsers/${id}/action-groups`);
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchLatestRecommendationsAsync = async ({ token, id }) => {
    const url = getUrl(`api/trackedUsers/${id}/latest-recommendations`);
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchTrackedUserActionAsync = async ({
    token,
    id,
    category,
    actionName,
  }) => {
    let url = getUrl(`api/trackedUsers/${id}/actions/${category}`);
    if (actionName) {
      url = url + `?actionName=${actionName}`;
    }
    const response = await fetch(url, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };
  const fetchTrackedUserAction = async ({
    success,
    error,
    token,
    id,
    category,
    actionName,
  }) => {
    fetchTrackedUserActionAsync({ id, token, category, actionName })
      .then(success)
      .catch(error);
  };

  const uploadUserData = async ({ success, error, token, payload }) => {
    const url = getUrl(`api/trackedUsers`);
    const payloads = chunkArray(payload.users, MAX_ARRAY).map((users) => ({
      users,
    }));
    const responses = [];
    for (const p of payloads) {
      const response = await fetch(url, {
        headers: headers(token),
        method: "put",
        body: JSON.stringify(p),
      });
      if (response.ok) {
        responses.push(await response.json());
      } else {
        error(await response.json());
      }
    }
    success(responses);
  };

  const createOrUpdateTrackedUser = async ({
    success,
    error,
    token,
    user,
  }) => {
    const url = getUrl(`api/trackedUsers/`);
    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(user),
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  var trackedUsersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchTrackedUsersAsync: fetchTrackedUsersAsync,
    fetchTrackedUsers: fetchTrackedUsers,
    updateMergePropertiesAsync: updateMergePropertiesAsync,
    fetchTrackedUser: fetchTrackedUser,
    fetchUniqueTrackedUserActionGroupsAsync: fetchUniqueTrackedUserActionGroupsAsync,
    fetchLatestRecommendationsAsync: fetchLatestRecommendationsAsync,
    fetchTrackedUserActionAsync: fetchTrackedUserActionAsync,
    fetchTrackedUserAction: fetchTrackedUserAction,
    uploadUserData: uploadUserData,
    createOrUpdateTrackedUser: createOrUpdateTrackedUser
  });

  const fetchRewardSelectorsAsync = async ({ token, page }) => {
    const url = getUrl("api/RewardSelectors");
    let path = `${url}?${pageQuery(page)}`;

    const response = await fetch(path, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchRewardSelectorAsync = async ({ token, page, id }) => {
    const url = getUrl(`api/RewardSelectors/${id}`);
    let path = `${url}?${pageQuery(page)}`;

    const response = await fetch(path, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const deleteRewardSelectorAsync = async ({ token, page, id }) => {
    const url = getUrl(`api/RewardSelectors/${id}`);
    let path = `${url}?${pageQuery(page)}`;

    const response = await fetch(path, {
      headers: headers(token),
      method: "delete",
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const createRewardSelectorAsync = async ({ token, entity }) => {
    const url = getUrl("api/RewardSelectors");

    const response = await fetch(url, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(entity),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  var rewardSelectorsApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchRewardSelectorsAsync: fetchRewardSelectorsAsync,
    fetchRewardSelectorAsync: fetchRewardSelectorAsync,
    deleteRewardSelectorAsync: deleteRewardSelectorAsync,
    createRewardSelectorAsync: createRewardSelectorAsync
  });

  const fetchUniqueActionNamesAsync = async ({ token, page, term }) => {
    const url = getUrl("api/actions/distinct-groups");
    let path = `${url}?${pageQuery(page)}`;
    if (term) {
      path = path + `&term=${term}`;
    }

    const response = await fetch(path, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
  };

  const fetchDistinctGroupsAsync = async ({ token, page, term }) => {
    const url = getUrl(
      `api/actions/distinct-groups?${term ? "term=" + term : ""}`
    );
    let path = `${url}${pageQuery(page)}`;

    const response = await fetch(path, {
      headers: headers(token),
    });
    if (response.ok) {
      return await response.json();
    } else {
      throw await response.json();
    }
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
  exports.events = eventsApi;
  exports.experiments = experimentsApi;
  exports.featureGenerators = featureGeneratorsApi;
  exports.features = featuresApi;
  exports.integratedSystems = integratedSystemsApi;
  exports.modelRegistrations = modelRegistrationsApi;
  exports.models = index;
  exports.offers = offersApi;
  exports.paging = paging;
  exports.parameterSetRecommenders = parameterSetRecommendersApi;
  exports.parameters = parametersApi;
  exports.productRecommenders = productRecommendersApi;
  exports.products = productsApi;
  exports.reactConfig = reactConfigApi;
  exports.reports = reportsApi;
  exports.rewardSelectors = rewardSelectorsApi;
  exports.segments = segmentsApi;
  exports.setBaseUrl = setBaseUrl;
  exports.touchpoints = touchpointsApi;
  exports.trackedUsers = trackedUsersApi;

  Object.defineProperty(exports, '__esModule', { value: true });

})));
