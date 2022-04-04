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

const recommenderApiName = "ParameterSetRecommenders";

export const fetchParameterSetRecommendersAsync = async ({
  token,
  page,
}: PaginatedRequest) => {
  return await executeFetch({
    path: "api/recommenders/ParameterSetRecommenders",
    token,
    page,
  });
};

export const fetchParameterSetRecommenderAsync = async ({
  token,
  id,
  searchTerm,
}: EntitySearchRequest) => {
  return await executeFetch({
    path: `api/recommenders/ParameterSetRecommenders/${id}`,
    token,
    query: {
      "q.term": searchTerm,
    },
  });
};

interface CreateParameterSetRecommenderRequest extends AuthenticatedRequest {
  payload: {
    name: string;
    commonId: string;
    settings: components["schemas"]["RecommenderSettingsDto"];
    parameters: string[];
    bounds: components["schemas"]["ParameterBounds"][];
    arguments: components["schemas"]["CreateOrUpdateRecommenderArgument"][];
  };
}
export const createParameterSetRecommenderAsync = async ({
  token,
  payload,
}: CreateParameterSetRecommenderRequest) => {
  return await executeFetch({
    path: "api/recommenders/ParameterSetRecommenders",
    token,
    method: "post",
    body: payload,
  });
};

export const deleteParameterSetRecommenderAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/recommenders/ParameterSetRecommenders/${id}`,
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
    path: `api/recommenders/ParameterSetRecommenders/${id}/recommendations`,
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
    recommenderApiName,
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
    recommenderApiName,
    id,
    token,
  });
};

interface InvokeParameterSetRecommenderRequest extends EntityRequest {
  input: components["schemas"]["ModelInputDto"];
}
export const invokeParameterSetRecommenderAsync = async ({
  token,
  id,
  input,
}: InvokeParameterSetRecommenderRequest) => {
  return await executeFetch({
    path: `api/recommenders/ParameterSetRecommenders/${id}/invoke`,
    token,
    method: "post",
    body: input,
  });
};

export const fetchInvokationLogsAsync = async ({
  id,
  token,
  page,
}: PaginatedEntityRequest) => {
  return await il.fetchRecommenderInvokationLogsAsync({
    recommenderApiName,
    id,
    token,
    page,
  });
};

export const fetchTargetVariablesAsync = async ({ id, token, name }: any) => {
  return await tv.fetchRecommenderTargetVariableValuesAsync({
    recommenderApiName,
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
  return await tv.createRecommenderTargetVariableValueAsync({
    recommenderApiName,
    id,
    token,
    targetVariableValue,
  });
};

interface SetSettingsRequest extends EntityRequest {
  settings: components["schemas"]["RecommenderSettingsDto"];
}
export const setSettingsAsync = async ({
  id,
  token,
  settings,
}: SetSettingsRequest) => {
  return await st.setSettingsAsync({
    recommenderApiName,
    id,
    token,
    settings,
  });
};

interface SetArgumentsRequest extends EntityRequest {
  args: components["schemas"]["CreateOrUpdateRecommenderArgument"][];
}
export const setArgumentsAsync = async ({
  id,
  token,
  args,
}: SetArgumentsRequest) => {
  return await ar.setArgumentsAsync({
    recommenderApiName,
    id,
    token,
    args,
  });
};

export const fetchDestinationsAsync = async ({ id, token }: EntityRequest) => {
  return await ds.fetchDestinationsAsync({
    recommenderApiName,
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
    recommenderApiName,
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
    recommenderApiName,
    id,
    token,
    destinationId,
  });
};

export const fetchTriggerAsync = async ({ id, token }: EntityRequest) => {
  return await trig.fetchTriggerAsync({
    recommenderApiName,
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
    recommenderApiName,
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
    recommenderApiName,
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
    recommenderApiName,
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
    recommenderApiName,
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
    recommenderApiName,
    id,
    token,
    useInternalId,
    metricIds,
  });
};

type RecommenderStatistics = components["schemas"]["RecommenderStatistics"];
export const fetchStatisticsAsync = async ({
  id,
  token,
}: EntityRequest): Promise<RecommenderStatistics> => {
  return await executeFetch({
    path: `api/recommenders/ParameterSetRecommenders/${id}/Statistics`,
    token,
  });
};

export const fetchReportImageBlobUrlAsync = async ({
  id,
  token,
  useInternalId,
}: EntityRequest): Promise<any> => {
  return await ri.fetchReportImageBlobUrlAsync({
    recommenderApiName,
    id,
    token,
    useInternalId,
  });
};
