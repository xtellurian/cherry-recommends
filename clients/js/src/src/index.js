// fix missing fetch is node environments
const fetch = require("node-fetch");
if (typeof globalThis === "object") {
  // See: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/globalThis
  globalThis.fetch = fetch;
} else if (typeof global === "object") {
  // For Node <12
  global.fetch = fetch;
} else {
  // Everything else is not supported
  throw new Error("Unknown JavaScript environment: Not supported");
}

import * as apiKeys from "./api/apiKeyApi";
import * as businesses from "./api/businessesApi";
import * as customers from "./api/customersApi";
import * as dataSummary from "./api/dataSummaryApi";
import * as deployment from "./api/deploymentApi";
import * as events from "./api/eventsApi";
import * as environments from "./api/environmentsApi";
import * as featureGenerators from "./api/featureGeneratorsApi";
import * as features from "./api/featuresApi";
import * as metrics from "./api/metricsApi";
import * as metricGenerators from "./api/metricGeneratorsApi";
import * as integratedSystems from "./api/integratedSystemsApi";
import * as modelRegistrations from "./api/modelRegistrationsApi";
import * as models from "./api/models/index";
import * as parameters from "./api/parametersApi";
import * as parameterSetRecommenders from "./api/recommenders/parameterSetRecommendersApi";
import * as profile from "./api/profileApi";
import * as itemsRecommenders from "./api/recommenders/itemsRecommendersApi";
import * as reactConfig from "./api/reactConfigApi";
import * as recommendableItems from "./api/recommendableItemsApi";
import * as reports from "./api/reportsApi";
import * as segments from "./api/segmentsApi";
import * as touchpoints from "./api/touchpointsApi";
import * as trackedUsers from "./api/trackedUsersApi";
import * as rewardSelectors from "./api/rewardSelectorsApi";
import * as actions from "./api/actionsApi";
import { components } from "./model/api";

import { setBaseUrl } from "./api/client/baseUrl";
import {
  setDefaultEnvironmentId,
  setDefaultApiKey,
} from "./api/client/headers";
import * as errorHandling from "./utilities/errorHandling";

export {
  actions,
  apiKeys,
  businesses,
  customers,
  dataSummary,
  deployment,
  events,
  environments,
  featureGenerators,
  features,
  integratedSystems,
  itemsRecommenders,
  metrics,
  metricGenerators,
  modelRegistrations,
  models,
  parameters,
  parameterSetRecommenders,
  profile,
  reactConfig,
  recommendableItems,
  reports,
  rewardSelectors,
  segments,
  touchpoints,
  trackedUsers,
  setBaseUrl,
  setDefaultEnvironmentId,
  setDefaultApiKey,
  errorHandling,
  components
};
