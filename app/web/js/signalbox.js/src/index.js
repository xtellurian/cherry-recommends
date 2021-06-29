// fix missing fetch is node environments
const fetch = require("node-fetch");
if (!globalThis.fetch) {
  globalThis.fetch = fetch;
}

import * as apiKeys from "./api/apiKeyApi";
import * as dataSummary from "./api/dataSummaryApi";
import * as deployment from "./api/deploymentApi";
import * as events from "./api/eventsApi";
import * as experiments from "./api/experimentsApi";
import * as integratedSystems from "./api/integratedSystemsApi";
import * as modelRegistrations from "./api/modelRegistrationsApi";
import * as models from "./api/models/index";
import * as offers from "./api/offersApi";
import * as paging from "./api/paging";
import * as reactConfig from "./api/reactConfigApi";
import * as reports from "./api/reportsApi";
import * as segments from "./api/segmentsApi";
import * as touchpoints from "./api/touchpointsApi";
import * as trackedUsers from "./api/trackedUsersApi";
import * as parameters from "./api/parametersApi";
import * as parameterSetRecommenders from "./api/parameterSetRecommendersApi";

import { setBaseUrl } from "./baseUrl";

export {
  apiKeys,
  dataSummary,
  deployment,
  events,
  experiments,
  integratedSystems,
  modelRegistrations,
  models,
  offers,
  paging,
  parameters,
  parameterSetRecommenders,
  reactConfig,
  reports,
  segments,
  touchpoints,
  trackedUsers,
  setBaseUrl,
};
