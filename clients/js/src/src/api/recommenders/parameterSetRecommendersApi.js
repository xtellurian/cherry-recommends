import { pageQuery } from "../paging";
import { getUrl } from "../../baseUrl";
import { headers } from "../headers";

import * as link from "./common/linkRegisteredModels";

import * as actions from "./common/trackedUserActions";
import * as eh from "./common/errorHandling";
import * as tv from "./common/targetvariables";
import * as il from "./common/invokationLogs";
import * as ar from "./common/args";
import * as st from "./common/settings";
import * as ds from "./common/destinations";

const recommenderApiName = "ParameterSetRecommenders";

export const fetchParameterSetRecommendersAsync = async ({ token, page }) => {
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

export const fetchParameterSetRecommenders = async ({
  success,
  error,
  token,
  page,
}) => {
  fetchParameterSetRecommendersAsync({ token, page })
    .then(success)
    .catch(error);
};

export const fetchParameterSetRecommenderAsync = async ({
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

export const fetchParameterSetRecommender = async ({
  success,
  error,
  token,
  id,
}) => {
  fetchParameterSetRecommenderAsync({ id, token }).then(success).catch(error);
};

export const createParameterSetRecommenderAsync = async ({
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
export const createParameterSetRecommender = async ({
  success,
  error,
  token,
  payload,
}) => {
  createParameterSetRecommenderAsync({ token, payload })
    .then(success)
    .catch(error);
};

export const deleteParameterSetRecommender = async ({
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

export const fetchParameterSetRecommendationsAsync = async ({
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

export const invokeParameterSetRecommenderAsync = async ({
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
export const invokeParameterSetRecommender = ({
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
    recommenderApiName: "ParameterSetRecommenders",
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
