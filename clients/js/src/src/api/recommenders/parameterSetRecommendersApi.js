import { pageQuery } from "../paging";
import { getUrl } from "../../baseUrl";
import { headers } from "../headers";

import {
  fetchLinkedRegisteredModelAsync,
  createLinkedRegisteredModelAsync,
} from "./common/linkRegisteredModels";

import * as eh from "./common/errorHandling";
import * as tv from "./common/targetvariables";
import * as il from "./common/invokationLogs";

export const fetchParameterSetRecommenders = async ({
  success,
  error,
  token,
  page,
}) => {
  const url = getUrl("api/recommenders/ParameterSetRecommenders");
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: headers(token),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};

export const fetchParameterSetRecommender = async ({
  success,
  error,
  token,
  id,
}) => {
  const url = getUrl(`api/recommenders/ParameterSetRecommenders/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};

export const createParameterSetRecommender = async ({
  success,
  error,
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
    success(await response.json());
  } else {
    error(await response.json());
  }
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

export const createLinkRegisteredModel = async ({
  success,
  error,
  token,
  id,
  modelId,
}) => {
  createLinkedRegisteredModelAsync({
    recommenderApiName: "ParameterSetRecommenders",
    id,
    modelId,
    token,
  })
    .then(success)
    .catch(error);
};

export const fetchLinkedRegisteredModel = async ({
  success,
  error,
  token,
  id,
}) => {
  fetchLinkedRegisteredModelAsync({
    recommenderApiName: "ParameterSetRecommenders",
    id,
    token,
  })
    .then(success)
    .catch(error);
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
    recommenderApiName: "ParameterSetRecommenders",
    id,
    token,
    page,
  });
};

export const fetchTargetVariablesAsync = async ({ id, token, name }) => {
  return await tv.fetchRecommenderTargetVariableValuesAsync({
    recommenderApiName: "ParameterSetRecommenders",
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
    recommenderApiName: "ParameterSetRecommenders",
    id,
    token,
    errorHandling,
  });
};
