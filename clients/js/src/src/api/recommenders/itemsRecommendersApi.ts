import { executeFetch } from "../client/apiClientTs";
import * as link from "./common/linkRegisteredModels";

import * as ar from "./common/args";
import * as tv from "./common/targetvariables";
import * as il from "./common/invokationLogs";
import * as eh from "./common/errorHandling";
import * as st from "./common/settings";
import * as ds from "./common/destinations";
import * as trig from "./common/trigger";
import * as lf from "./common/learningFeatures";
import {
  PaginatedRequest,
  EntityRequest,
  DeleteRequest,
  AuthenticatedRequest,
  ModelInput,
  ItemsRecommendation,
} from "../../interfaces";

const recommenderApiName = "ItemsRecommenders";

export const fetchItemsRecommendersAsync = async ({
  token,
  page,
}: PaginatedRequest) => {
  return await executeFetch({
    token,
    path: "api/recommenders/ItemsRecommenders",
    page,
  });
};

export const fetchItemsRecommenderAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}`,
    token,
  });
};

interface ItemsRecommendationsRequest extends EntityRequest {
  page: number;
}

export const fetchItemsRecommendationsAsync = async ({
  token,
  page,
  id,
}: ItemsRecommendationsRequest) => {
  return await executeFetch({
    token,
    path: `api/recommenders/ItemsRecommenders/${id}/Recommendations`,
    page,
  });
};

export const deleteItemsRecommenderAsync = async ({
  token,
  id,
}: DeleteRequest) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}`,
    token,
    method: "delete",
  });
};

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
export const createItemsRecommenderAsync = async ({
  token,
  payload,
}: CreateItemsRecommenderRequest) => {
  return await executeFetch({
    path: "api/recommenders/ItemsRecommenders",
    token,
    method: "post",
    body: payload,
  });
};

export const fetchItemsAsync = async ({ token, id }: EntityRequest) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}/Items`,
    token,
  });
};

interface AddItemPayload {
  id: number | undefined;
  commonId: string | undefined;
}
interface AddItemRequest extends EntityRequest {
  item: AddItemPayload;
}
export const addItemAsync = async ({ token, id, item }: AddItemRequest) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}/Items`,
    token,
    method: "post",
    body: item,
  });
};

interface RemoveItemRequest extends EntityRequest {
  itemId: string | number;
}
export const removeItemAsync = async ({
  token,
  id,
  itemId,
}: RemoveItemRequest) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}/Items/${itemId}`,
    token,
    method: "post",
  });
};

interface SetDefaultItemRequest extends EntityRequest {
  itemId: string | number;
}
export const setDefaultItemAsync = async ({
  token,
  id,
  itemId,
}: SetDefaultItemRequest) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}/DefaultItem`,
    token,
    method: "post",
    body: { itemId },
  });
};

export const getDefaultItemAsync = async ({ token, id }: EntityRequest) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}/DefaultItem`,
    token,
  });
};

interface LinkRegisteredModelRequest extends EntityRequest {
  modelId: number;
}
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

interface InvokeItemRecommenderRequest extends EntityRequest {
  input: ModelInput;
}
export const invokeItemsRecommenderAsync = async ({
  token,
  id,
  input,
}: InvokeItemRecommenderRequest): Promise<ItemsRecommendation> => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}/Invoke`,
    token,
    method: "post",
    body: input,
  });
};

interface FetchInvokationLogsRequest extends EntityRequest {
  page: number;
}
export const fetchInvokationLogsAsync = async ({
  id,
  token,
  page,
}: FetchInvokationLogsRequest) => {
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

interface RecommenderSettings {
  requireConsumptionEvent: boolean;
  throwOnBadInput: boolean;
  recommendationCacheTime: string;
}
interface SetSettingsRequest extends EntityRequest {
  settings: RecommenderSettings;
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

interface Argument {
  commonId: string;
  argumentType: "Numerical" | "Categorical";
  defaultValue: string | number;
  isRequired: boolean;
}
interface SetArgumentsRequest extends EntityRequest {
  args: Argument[];
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

interface Destination {
  destinationType:
    | "Webhook"
    | "SegmentSourceFunction"
    | "HubspotContactProperty";
  endpoint: string;
  integratedSystemId: number;
}
interface CreateDestinationRequest extends EntityRequest {
  destination: Destination;
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
  destinationId: number;
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

interface Trigger {
  featuresChanged: any;
}
interface SetTriggerRequest extends EntityRequest {
  trigger: Trigger;
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
  featureIds: string[];
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
