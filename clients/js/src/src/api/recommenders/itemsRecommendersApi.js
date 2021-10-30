import { pageQuery } from "../paging";
import { getUrl } from "../../baseUrl";
import { headers } from "../headers";

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
  const url = getUrl("api/recommenders/ItemsRecommenders");
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchItemsRecommenderAsync = async ({ token, id }) => {
  const url = getUrl(`api/recommenders/ItemsRecommenders/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchItemsRecommendationsAsync = async ({ token, page, id }) => {
  const url = getUrl(
    `api/recommenders/ItemsRecommenders/${id}/Recommendations`
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

export const deleteItemsRecommenderAsync = async ({ token, id }) => {
  const url = getUrl(`api/recommenders/ItemsRecommenders/${id}`);
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

export const createItemsRecommenderAsync = async ({ token, payload }) => {
  const url = getUrl("api/recommenders/ItemsRecommenders");
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

export const fetchItemsAsync = async ({ token, id }) => {
  const url = getUrl(`api/recommenders/ItemsRecommenders/${id}/Items`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const addItemAsync = async ({ token, id, item }) => {
  const url = getUrl(`api/recommenders/ItemsRecommenders/${id}/Items`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(item),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const removeItemAsync = async ({ token, id, itemId }) => {
  const url = getUrl(
    `api/recommenders/ItemsRecommenders/${id}/Items/${itemId}`
  );
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

export const setDefaultItemAsync = async ({ token, id, itemId }) => {
  const url = getUrl(`api/recommenders/ItemsRecommenders/${id}/DefaultItem`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify({ itemId }),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const getDefaultItemAsync = async ({ token, id }) => {
  const url = getUrl(`api/recommenders/ItemsRecommenders/${id}/DefaultItem`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
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
  const url = getUrl(`api/recommenders/ItemsRecommenders/${id}/Invoke`);
  const result = await fetch(url, {
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
