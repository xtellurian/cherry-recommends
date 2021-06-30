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

  const defaultHeaders$g = { "Content-Type": "application/json" };

  const fetchApiKeys = async ({ success, error, token, page }) => {
    const url = getUrl("api/apiKeys");
    let path = `${url}?${pageQuery(page)}`;

    const response = await fetch(path, {
      headers: !token
        ? defaultHeaders$g
        : { ...defaultHeaders$g, Authorization: `Bearer ${token}` },
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
      headers: !token
        ? defaultHeaders$g
        : { ...defaultHeaders$g, Authorization: `Bearer ${token}` },
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

  const exchangeApiKey = async ({ success, error, token, apiKey }) => {
    const url = getUrl("api/apiKeys/exchange");
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$g
        : { ...defaultHeaders$g, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify({ apiKey }),
    });
    if (response.ok) {
      const data = await response.json();
      success(data);
    } else {
      error(await response.json());
    }
  };

  var apiKeyApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchApiKeys: fetchApiKeys,
    createApiKey: createApiKey,
    exchangeApiKey: exchangeApiKey
  });

  const defaultHeaders$f = { "Content-Type": "application/json" };

  const fetchEventSummary = async ({ success, error, token }) => {
    const url = getUrl("api/datasummary/events");
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$f
        : { ...defaultHeaders$f, Authorization: `Bearer ${token}` },
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
      headers: !token
        ? defaultHeaders$f
        : { ...defaultHeaders$f, Authorization: `Bearer ${token}` },
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
      headers: !token
        ? defaultHeaders$f
        : { ...defaultHeaders$f, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  var dataSummaryApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchEventSummary: fetchEventSummary,
    fetchEventTimeline: fetchEventTimeline,
    fetchDashboard: fetchDashboard
  });

  const defaultHeaders$e = { "Content-Type": "application/json" };

  const fetchDeploymentConfiguration = async ({
    success,
    error,
    token,
  }) => {
    const url = getUrl(`api/deployment/configuration`);

    const result = await fetch(url, {
      headers: !token
        ? defaultHeaders$e
        : { ...defaultHeaders$e, Authorization: `Bearer ${token}` },
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

  const defaultHeaders$d = { "Content-Type": "application/json" };

  const fetchUserEvents = async ({
    success,
    error,
    token,
    commonUserId,
  }) => {
    const url = getUrl("api/events");
    let path = `${url}?commonUserId=${commonUserId}`;

    const response = await fetch(path, {
      headers: !token
        ? defaultHeaders$d
        : { ...defaultHeaders$d, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
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
      headers: !token
        ? defaultHeaders$d
        : { ...defaultHeaders$d, Authorization: `Bearer ${token}` },
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
    fetchUserEvents: fetchUserEvents,
    logUserEvents: logUserEvents
  });

  const defaultHeaders$c = { "Content-Type": "application/json" };

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
        ? defaultHeaders$c
        : { ...defaultHeaders$c, Authorization: `Bearer ${token}` },
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
        ? defaultHeaders$c
        : { ...defaultHeaders$c, Authorization: `Bearer ${token}` },
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
        ? defaultHeaders$c
        : { ...defaultHeaders$c, Authorization: `Bearer ${token}` },
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

  const defaultHeaders$b = { "Content-Type": "application/json" };

  const fetchIntegratedSystems = async ({
    success,
    error,
    token,
    page,
  }) => {
    const url = getUrl("api/integratedSystems");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: !token
        ? defaultHeaders$b
        : { ...defaultHeaders$b, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const fetchIntegratedSystem = async ({ success, error, token, id }) => {
    const url = getUrl(`api/integratedSystems/${id}`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$b
        : { ...defaultHeaders$b, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const createIntegratedSystem = async ({
    success,
    error,
    token,
    payload,
  }) => {
    const url = getUrl("api/integratedSystems");
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$b
        : { ...defaultHeaders$b, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify(payload),
    });
    if (response.ok) {
      const data = await response.json();
      success(data);
    } else {
      error(await response.json());
    }
  };

  const fetchWebhookReceivers = async ({
    success,
    error,
    token,
    id,
  }) => {
    const url = getUrl(`api/integratedSystems/${id}/webhookreceivers`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$b
        : { ...defaultHeaders$b, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const data = await response.json();
      success(data);
    } else {
      error(await response.json());
    }
  };

  const createWebhookReceiver = async ({
    success,
    error,
    token,
    id,
    useSharedSecret
  }) => {
    const url = getUrl(`api/integratedSystems/${id}/webhookreceivers`);
    const response = await fetch(`${url}?useSharedSecret=${useSharedSecret}`, {
      headers: !token
        ? defaultHeaders$b
        : { ...defaultHeaders$b, Authorization: `Bearer ${token}` },
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
    fetchIntegratedSystem: fetchIntegratedSystem,
    createIntegratedSystem: createIntegratedSystem,
    fetchWebhookReceivers: fetchWebhookReceivers,
    createWebhookReceiver: createWebhookReceiver
  });

  const defaultHeaders$a = { "Content-Type": "application/json" };

  const fetchModelRegistrations = async ({
    success,
    error,
    token,
    page,
  }) => {
    const url = getUrl("api/ModelRegistrations");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: !token
        ? defaultHeaders$a
        : { ...defaultHeaders$a, Authorization: `Bearer ${token}` },
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
      headers: !token
        ? defaultHeaders$a
        : { ...defaultHeaders$a, Authorization: `Bearer ${token}` },
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
      headers: !token
        ? defaultHeaders$a
        : { ...defaultHeaders$a, Authorization: `Bearer ${token}` },
      method: "delete",
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const createModelRegistration = async ({
    success,
    error,
    token,
    payload,
  }) => {
    const url = getUrl("api/ModelRegistrations");
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$a
        : { ...defaultHeaders$a, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify(payload),
    });
    if (response.ok) {
      const data = await response.json();
      success(data);
    } else {
      error(await response.json());
    }
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
      headers: !token
        ? defaultHeaders$a
        : { ...defaultHeaders$a, Authorization: `Bearer ${token}` },
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
    createModelRegistration: createModelRegistration,
    invokeModel: invokeModel
  });

  const defaultHeaders$9 = { "Content-Type": "application/json" };

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
          ? defaultHeaders$9
          : { ...defaultHeaders$9, Authorization: `Bearer ${token}` },
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

  const defaultHeaders$8 = { "Content-Type": "application/json" };

  const invokeParameterSetRecommenderModel = async ({
    success,
    error,
    onFinally,
    token,
    id,
    version,
    input,
  }) => {
    try {
      const url = getUrl(`api/models/ParameterSetRecommenders/${id}/invoke`);
      const result = await fetch(`${url}?version=${version || "default"}`, {
        headers: !token
          ? defaultHeaders$8
          : { ...defaultHeaders$8, Authorization: `Bearer ${token}` },
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
    invokeGenericModel: invokeGenericModel,
    invokeParameterSetRecommenderModel: invokeParameterSetRecommenderModel
  });

  const defaultHeaders$7 = { "Content-Type": "application/json" };

  const fetchOffers = async ({ success, error, token, page }) => {
    const url = getUrl("api/offers");
    let path = `${url}?${pageQuery(page)}`;
    const response = await fetch(path, {
      headers: !token
        ? defaultHeaders$7
        : { ...defaultHeaders$7, Authorization: `Bearer ${token}` },
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
        ? defaultHeaders$7
        : { ...defaultHeaders$7, Authorization: `Bearer ${token}` },
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
        ? defaultHeaders$7
        : { ...defaultHeaders$7, Authorization: `Bearer ${token}` },
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

  const defaultHeaders$6 = { "Content-Type": "application/json" };

  let config = null; // caches this because it rarely change
  const fetchAuth0Configuration = async () => {
    if (!config) {
      console.log("fetching auth0 from server...");
      const result = await fetch(getUrl("api/reactConfig/auth0"), {
        headers: defaultHeaders$6,
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

  const defaultHeaders$5 = { "Content-Type": "application/json" };

  const fetchReports = async ({ success, error, token }) => {
    const url = getUrl("api/reports");
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

  const downloadReport = async ({ success, error, token, reportName }) => {
    const url = getUrl("api/reports/download");
    let path = `${url}?report=${reportName}`;

    const response = await fetch(path, {
      headers: !token
        ? defaultHeaders$5
        : { ...defaultHeaders$5, Authorization: `Bearer ${token}` },
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

  const defaultHeaders$4 = { "Content-Type": "application/json" };

  const fetchSegments = async ({ success, error, token, page }) => {
    const url = getUrl("api/segments");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: !token
        ? defaultHeaders$4
        : { ...defaultHeaders$4, Authorization: `Bearer ${token}` },
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
        ? defaultHeaders$4
        : { ...defaultHeaders$4, Authorization: `Bearer ${token}` },
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
        ? defaultHeaders$4
        : { ...defaultHeaders$4, Authorization: `Bearer ${token}` },
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

  const defaultHeaders$3 = { "Content-Type": "application/json" };

  const fetchTouchpoints = async ({ success, error, token, page }) => {
    const url = getUrl("api/touchpoints");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: !token
        ? defaultHeaders$3
        : { ...defaultHeaders$3, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const results = await response.json();
      success(results);
    } else {
      error(await response.json());
    }
  };

  const fetchTouchpoint = async ({ success, error, token, id }) => {
    const url = getUrl(`api/touchpoints/${id}`);
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$3
        : { ...defaultHeaders$3, Authorization: `Bearer ${token}` },
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
  }) => {
    const url = getUrl("api/touchpoints");
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$3
        : { ...defaultHeaders$3, Authorization: `Bearer ${token}` },
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
        ? defaultHeaders$3
        : { ...defaultHeaders$3, Authorization: `Bearer ${token}` },
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
        ? defaultHeaders$3
        : { ...defaultHeaders$3, Authorization: `Bearer ${token}` },
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
        ? defaultHeaders$3
        : { ...defaultHeaders$3, Authorization: `Bearer ${token}` },
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
        ? defaultHeaders$3
        : { ...defaultHeaders$3, Authorization: `Bearer ${token}` },
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
  const defaultHeaders$2 = { "Content-Type": "application/json" };

  const fetchTrackedUsers = async ({ success, error, token, page }) => {
    const url = getUrl("api/trackedUsers");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const trackedUsers = await response.json();
      success(trackedUsers);
    } else {
      error(await response.json());
    }
  };

  const fetchTrackedUser = async ({ success, error, token, id }) => {
    const url = getUrl(`api/trackedUsers/${id}`);
    const response = await fetch(url, {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      const trackedUser = await response.json();
      success(trackedUser);
    } else {
      error(await response.json());
    }
  };

  const uploadUserData = async ({ success, error, token, payload }) => {
    const url = getUrl(`api/trackedUsers`);
    const payloads = chunkArray(payload.users, MAX_ARRAY).map((users) => ({
      users,
    }));
    const responses = [];
    for (const p of payloads) {
      const response = await fetch(url, {
        headers: !token
          ? defaultHeaders$2
          : { ...defaultHeaders$2, Authorization: `Bearer ${token}` },
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
      headers: !token
        ? defaultHeaders$2
        : { ...defaultHeaders$2, Authorization: `Bearer ${token}` },
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
    fetchTrackedUsers: fetchTrackedUsers,
    fetchTrackedUser: fetchTrackedUser,
    uploadUserData: uploadUserData,
    createOrUpdateTrackedUser: createOrUpdateTrackedUser
  });

  const defaultHeaders$1 = { "Content-Type": "application/json" };

  const fetchParameters = async ({ success, error, token, page }) => {
    const url = getUrl("api/parameters");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
      headers: !token
        ? defaultHeaders$1
        : { ...defaultHeaders$1, Authorization: `Bearer ${token}` },
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
      headers: !token
        ? defaultHeaders$1
        : { ...defaultHeaders$1, Authorization: `Bearer ${token}` },
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const createParameter = async ({ success, error, token, payload }) => {
    const url = getUrl("api/parameters");
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders$1
        : { ...defaultHeaders$1, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify(payload),
    });

    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  var parametersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchParameters: fetchParameters,
    fetchParameter: fetchParameter,
    createParameter: createParameter
  });

  const defaultHeaders = { "Content-Type": "application/json" };

  const fetchParameterSetRecommenders = async ({
    success,
    error,
    token,
    page,
  }) => {
    const url = getUrl("api/recommenders/ParameterSetRecommenders");
    const response = await fetch(`${url}?${pageQuery(page)}`, {
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

  const fetchParameterSetRecommender = async ({
    success,
    error,
    token,
    id,
  }) => {
    const url = getUrl(`api/recommenders/ParameterSetRecommenders/${id}`);
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

  const createParameterSetRecommender = async ({
    success,
    error,
    token,
    payload,
  }) => {
    const url = getUrl("api/recommenders/ParameterSetRecommenders");
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders
        : { ...defaultHeaders, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify(payload),
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const createLinkRegisteredModel = async ({
    success,
    error,
    token,
    id,
    modelId,
  }) => {
    const url = getUrl(
      `api/recommenders/ParameterSetRecommenders/${id}/ModelRegistration`
    );
    const response = await fetch(url, {
      headers: !token
        ? defaultHeaders
        : { ...defaultHeaders, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify({ modelId }),
    });
    if (response.ok) {
      success(await response.json());
    } else {
      error(await response.json());
    }
  };

  const fetchLinkedRegisteredModel = async ({
    success,
    error,
    token,
    id,
  }) => {
    const url = getUrl(
      `api/recommenders/ParameterSetRecommenders/${id}/ModelRegistration`
    );
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

  var parameterSetRecommendersApi = /*#__PURE__*/Object.freeze({
    __proto__: null,
    fetchParameterSetRecommenders: fetchParameterSetRecommenders,
    fetchParameterSetRecommender: fetchParameterSetRecommender,
    createParameterSetRecommender: createParameterSetRecommender,
    createLinkRegisteredModel: createLinkRegisteredModel,
    fetchLinkedRegisteredModel: fetchLinkedRegisteredModel
  });

  // fix missing fetch is node environments
  const fetch$1 = require("node-fetch");
  if (!globalThis.fetch) {
    globalThis.fetch = fetch$1;
  }

  exports.apiKeys = apiKeyApi;
  exports.dataSummary = dataSummaryApi;
  exports.deployment = deploymentApi;
  exports.events = eventsApi;
  exports.experiments = experimentsApi;
  exports.integratedSystems = integratedSystemsApi;
  exports.modelRegistrations = modelRegistrationsApi;
  exports.models = index;
  exports.offers = offersApi;
  exports.paging = paging;
  exports.parameterSetRecommenders = parameterSetRecommendersApi;
  exports.parameters = parametersApi;
  exports.reactConfig = reactConfigApi;
  exports.reports = reportsApi;
  exports.segments = segmentsApi;
  exports.setBaseUrl = setBaseUrl;
  exports.touchpoints = touchpointsApi;
  exports.trackedUsers = trackedUsersApi;

  Object.defineProperty(exports, '__esModule', { value: true });

})));
