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
import * as dataSummary from "./api/dataSummaryApi";
import * as deployment from "./api/deploymentApi";
import * as events from "./api/eventsApi/index";
import * as environments from "./api/environmentsApi";
import * as featureGenerators from "./api/featureGeneratorsApi";
import * as features from "./api/featuresApi";
import * as integratedSystems from "./api/integratedSystemsApi";
import * as modelRegistrations from "./api/modelRegistrationsApi";
import * as models from "./api/models/index";
import * as offers from "./api/offersApi";
import * as paging from "./api/paging";
import * as parameters from "./api/parametersApi";
import * as parameterSetRecommenders from "./api/recommenders/parameterSetRecommendersApi";
import * as productRecommenders from "./api/recommenders/productRecommendersApi";
import * as itemsRecommenders from "./api/recommenders/itemsRecommendersApi";
import * as products from "./api/productsApi";
import * as reactConfig from "./api/reactConfigApi";
import * as recommendableItems from "./api/recommendableItemsApi";
import * as reports from "./api/reportsApi";
import * as segments from "./api/segmentsApi";
import * as touchpoints from "./api/touchpointsApi";
import * as trackedUsers from "./api/trackedUsersApi";
import * as rewardSelectors from "./api/rewardSelectorsApi";
import * as actions from "./api/actionsApi";

import { setBaseUrl } from "./baseUrl";
import { setDefaultEnvironmentId, setDefaultApiKey } from "./api/headers";

export {
  actions,
  apiKeys,
  dataSummary,
  deployment,
  events,
  environments,
  featureGenerators,
  features,
  integratedSystems,
  itemsRecommenders,
  modelRegistrations,
  models,
  offers,
  paging,
  parameters,
  parameterSetRecommenders,
  productRecommenders,
  products,
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
};
