import { executeFetch } from "../client/apiClient";

import * as link from "./common/linkRegisteredModels";

import * as actions from "./common/trackedUserActions";
import * as ar from "./common/args";
import * as tv from "./common/targetvariables";
import * as il from "./common/invokationLogs";
import * as eh from "./common/errorHandling";
import * as st from "./common/settings";
import * as ds from "./common/destinations";

const recommenderApiName = "ItemsRecommenders";

export const fetchItemsRecommendersAsync = async ({ token, page }) => {
  return await executeFetch({
    token,
    path: "api/recommenders/ItemsRecommenders",
    page,
  });
};

export const fetchItemsRecommenderAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}`,
    token,
  });
};

export const fetchItemsRecommendationsAsync = async ({ token, page, id }) => {
  return await executeFetch({
    token,
    path: `api/recommenders/ItemsRecommenders/${id}/Recommendations`,
    page,
  });
};

export const deleteItemsRecommenderAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}`,
    token,
    method: "delete",
  });
};

export const createItemsRecommenderAsync = async ({ token, payload }) => {
  return await executeFetch({
    path: "api/recommenders/ItemsRecommenders",
    token,
    method: "post",
    body: payload,
  });
};

export const fetchItemsAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}/Items`,
    token,
  });
};

export const addItemAsync = async ({ token, id, item }) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}/Items`,
    token,
    method: "post",
    body: item,
  });
};

export const removeItemAsync = async ({ token, id, itemId }) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}/Items/${itemId}`,
    token,
    method: "post",
  });
};

export const setDefaultItemAsync = async ({ token, id, itemId }) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}/DefaultItem`,
    token,
    method: "post",
    body: { itemId },
  });
};

export const getDefaultItemAsync = async ({ token, id }) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}/DefaultItem`,
    token,
  });
};

export const createLinkRegisteredModelAsync = async ({
  token,
  id,
  modelId,
}) => {
  return await link.createLinkedRegisteredModelAsync({
    recommenderApiName,
    id,
    modelId,
    token,
  });
};

export const fetchLinkedRegisteredModelAsync = async ({ token, id }) => {
  return await link.fetchLinkedRegisteredModelAsync({
    recommenderApiName,
    id,
    token,
  });
};

export const invokeItemsRecommenderAsync = async ({ token, id, input }) => {
  return await executeFetch({
    path: `api/recommenders/ItemsRecommenders/${id}/Invoke`,
    token,
    method: "post",
    body: input,
  });
};

export const fetchInvokationLogsAsync = async ({ id, token, page }) => {
  return await il.fetchRecommenderInvokationLogsAsync({
    recommenderApiName,
    id,
    token,
    page,
  });
};

export const fetchTargetVariablesAsync = async ({ id, token, name }) => {
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
}) => {
  return await tv.createRecommenderTargetVariableValueAsync({
    recommenderApiName,
    id,
    token,
    targetVariableValue,
  });
};

export const updateErrorHandlingAsync = async ({
  id,
  token,
  errorHandling,
}) => {
  return await eh.updateErrorHandlingAsync({
    recommenderApiName,
    id,
    token,
    errorHandling,
  });
};

export const setSettingsAsync = async ({ id, token, settings }) => {
  return await st.setSettingsAsync({
    recommenderApiName,
    id,
    token,
    settings,
  });
};

export const fetchRecommenderTrackedUserActionsAsync = async ({
  id,
  token,
  page,
  revenueOnly,
}) => {
  return await actions.fetchRecommenderTrackedUserActionsAsync({
    recommenderApiName,
    id,
    token,
    page,
    revenueOnly,
  });
};

export const setArgumentsAsync = async ({ id, token, args }) => {
  return await ar.setArgumentsAsync({
    recommenderApiName,
    id,
    token,
    args,
  });
};

export const fetchDestinationsAsync = async ({ id, token }) => {
  return await ds.fetchDestinationsAsync({
    recommenderApiName,
    id,
    token,
  });
};

export const createDestinationAsync = async ({ id, token, destination }) => {
  return await ds.createDestinationAsync({
    recommenderApiName,
    id,
    token,
    destination,
  });
};

export const removeDestinationAsync = async ({ id, token, destinationId }) => {
  return await ds.removeDestinationAsync({
    recommenderApiName,
    id,
    token,
    destinationId,
  });
};
