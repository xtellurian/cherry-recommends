import { executeFetch } from "../client/apiClient";
import { components } from "../../model/api";
import * as link from "./common/linkRegisteredModels";
import * as tv from "./common/targetvariables";
import * as il from "./common/invokationLogs";
import * as ar from "./common/args";
import * as st from "./common/settings";
import * as ds from "./common/destinations";
import * as trig from "./common/trigger";
import * as lf from "./common/learningFeatures";
import * as lm from "./common/learningMetrics";
import * as ri from "./common/reportImages";

import {
  AuthenticatedRequest,
  EntityRequest,
  EntitySearchRequest,
  PaginatedEntityRequest,
  PaginatedRequest,
} from "../../interfaces";

const campaignApiName = "ParameterSetCampaigns";

export const fetchParameterSetCampaignsAsync = async ({
  token,
  page,
}: PaginatedRequest) => {
  return await executeFetch({
    path: "api/campaigns/ParameterSetCampaigns",
    token,
    page,
  });
};

export const fetchParameterSetCampaignAsync = async ({
  token,
  id,
  searchTerm,
}: EntitySearchRequest) => {
  return await executeFetch({
    path: `api/campaigns/ParameterSetCampaigns/${id}`,
    token,
    query: {
      "q.term": searchTerm,
    },
  });
};

interface CreateParameterSetCampaignRequest extends AuthenticatedRequest {
  payload: {
    name: string;
    commonId: string;
    settings: components["schemas"]["CampaignSettingsDto"];
    parameters: string[];
    bounds: components["schemas"]["ParameterBounds"][];
    arguments: components["schemas"]["CreateOrUpdateCampaignArgument"][];
  };
}
export const createParameterSetCampaignAsync = async ({
  token,
  payload,
}: CreateParameterSetCampaignRequest) => {
  return await executeFetch({
    path: "api/campaigns/ParameterSetCampaigns",
    token,
    method: "post",
    body: payload,
  });
};

export const deleteParameterSetCampaignAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/campaigns/ParameterSetCampaigns/${id}`,
    token,
    method: "delete",
  });
};

export const fetchParameterSetRecommendationsAsync = async ({
  token,
  page,
  pageSize,
  id,
}: PaginatedEntityRequest) => {
  return await executeFetch({
    path: `api/campaigns/ParameterSetCampaigns/${id}/recommendations`,
    token,
    page,
    pageSize,
  });
};

type LinkRegisteredModelRequest = EntityRequest &
  components["schemas"]["LinkModel"];
export const createLinkRegisteredModelAsync = async ({
  token,
  id,
  modelId,
}: LinkRegisteredModelRequest) => {
  return await link.createLinkedRegisteredModelAsync({
    campaignApiName,
    id,
    modelId,
    token,
  });
};

export const fetchLinkedRegisteredModelAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await link.fetchLinkedRegisteredModelAsync({
    campaignApiName,
    id,
    token,
  });
};

interface InvokeParameterSetCampaignRequest extends EntityRequest {
  input: components["schemas"]["ModelInputDto"];
}
export const invokeParameterSetCampaignAsync = async ({
  token,
  id,
  input,
}: InvokeParameterSetCampaignRequest) => {
  return await executeFetch({
    path: `api/campaigns/ParameterSetCampaigns/${id}/invoke`,
    token,
    method: "post",
    body: input,
  });
};

export const fetchInvokationLogsAsync = async ({
  id,
  token,
  page,
  pageSize,
}: PaginatedEntityRequest) => {
  return await il.fetchCampaignInvokationLogsAsync({
    campaignApiName,
    id,
    token,
    page,
    pageSize,
  });
};

export const fetchTargetVariablesAsync = async ({ id, token, name }: any) => {
  return await tv.fetchCampaignTargetVariableValuesAsync({
    campaignApiName,
    id,
    token,
    name,
  });
};

export const createTargetVariableAsync = async ({
  id,
  token,
  targetVariableValue,
}: any) => {
  return await tv.createCampaignTargetVariableValueAsync({
    campaignApiName,
    id,
    token,
    targetVariableValue,
  });
};

interface SetSettingsRequest extends EntityRequest {
  settings: components["schemas"]["CampaignSettingsDto"];
}
export const setSettingsAsync = async ({
  id,
  token,
  settings,
}: SetSettingsRequest) => {
  return await st.setSettingsAsync({
    campaignApiName,
    id,
    token,
    settings,
  });
};
type CreateArgument = components["schemas"]["CreateOrUpdateCampaignArgument"];
type Argument = components["schemas"]["CampaignArgument"];

interface SetArgumentsRequest extends EntityRequest {
  args: CreateArgument[];
}

export const fetchArgumentsAsync = async ({ id, token }: EntityRequest) => {
  return await ar.fetchArgumentsAsync({
    campaignApiName,
    id,
    token,
  });
};

export const setArgumentsAsync = async ({
  id,
  token,
  args,
}: SetArgumentsRequest): Promise<Argument[]> => {
  return await ar.setArgumentsAsync({
    campaignApiName,
    id,
    token,
    args,
  });
};
export const fetchDestinationsAsync = async ({ id, token }: EntityRequest) => {
  return await ds.fetchDestinationsAsync({
    campaignApiName,
    id,
    token,
  });
};

interface CreateDestinationRequest extends EntityRequest {
  destination: components["schemas"]["CreateDestinationDto"];
}
export const createDestinationAsync = async ({
  id,
  token,
  destination,
}: CreateDestinationRequest) => {
  return await ds.createDestinationAsync({
    campaignApiName,
    id,
    token,
    destination,
  });
};

interface RemoveDestinationRequest extends EntityRequest {
  destinationId: string | number;
}
export const removeDestinationAsync = async ({
  id,
  token,
  destinationId,
}: RemoveDestinationRequest) => {
  return await ds.removeDestinationAsync({
    campaignApiName,
    id,
    token,
    destinationId,
  });
};

export const fetchTriggerAsync = async ({ id, token }: EntityRequest) => {
  return await trig.fetchTriggerAsync({
    campaignApiName,
    id,
    token,
  });
};

interface SetTriggerRequest extends EntityRequest {
  trigger: components["schemas"]["SetTriggersDto"];
}
export const setTriggerAsync = async ({
  id,
  token,
  trigger,
}: SetTriggerRequest) => {
  return await trig.setTriggerAsync({
    campaignApiName,
    id,
    token,
    trigger,
  });
};

export const fetchLearningFeaturesAsync = async ({
  id,
  token,
  useInternalId,
}: EntityRequest) => {
  return await lf.fetchLearningFeaturesAsync({
    campaignApiName,
    id,
    token,
    useInternalId,
  });
};

interface SetLearningFeaturesRequest extends EntityRequest {
  featureIds: string[] | number[];
}
export const setLearningFeaturesAsync = async ({
  id,
  token,
  featureIds,
  useInternalId,
}: SetLearningFeaturesRequest) => {
  return await lf.setLearningFeaturesAsync({
    campaignApiName,
    id,
    token,
    useInternalId,
    featureIds,
  });
};

export const fetchLearningMetricsAsync = async ({
  id,
  token,
  useInternalId,
}: EntityRequest) => {
  return await lm.fetchLearningMetricsAsync({
    campaignApiName,
    id,
    token,
    useInternalId,
  });
};

interface SetLearningMetricsRequest extends EntityRequest {
  metricIds: string[];
}
export const setLearningMetricsAsync = async ({
  id,
  token,
  metricIds,
  useInternalId,
}: SetLearningMetricsRequest) => {
  return await lm.setLearningMetricsAsync({
    campaignApiName,
    id,
    token,
    useInternalId,
    metricIds,
  });
};

type CampaignStatistics = components["schemas"]["CampaignStatistics"];
export const fetchStatisticsAsync = async ({
  id,
  token,
}: EntityRequest): Promise<CampaignStatistics> => {
  return await executeFetch({
    path: `api/campaigns/ParameterSetCampaigns/${id}/Statistics`,
    token,
  });
};

export const fetchReportImageBlobUrlAsync = async ({
  id,
  token,
  useInternalId,
}: EntityRequest): Promise<any> => {
  return await ri.fetchReportImageBlobUrlAsync({
    campaignApiName,
    id,
    token,
    useInternalId,
  });
};
