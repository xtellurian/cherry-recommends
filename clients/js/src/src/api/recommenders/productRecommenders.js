import { pageQuery } from "../paging";
import { getUrl } from "../../baseUrl";
import { headers } from "../headers";

import {
  fetchLinkedRegisteredModelAsync,
  createLinkedRegisteredModelAsync,
} from "./common/linkRegisteredModels";

import * as tv from "./common/targetvariables";
import * as il from "./common/invokationLogs";

export const fetchProductRecommenders = async ({
  success,
  error,
  token,
  page,
}) => {
  const url = getUrl("api/recommenders/ProductRecommenders");
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: headers(token),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};

export const fetchProductRecommender = async ({
  success,
  error,
  token,
  id,
}) => {
  const url = getUrl(`api/recommenders/ProductRecommenders/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};

export const fetchProductRecommendations = async ({
  success,
  error,
  token,
  page,
  id,
}) => {
  const url = getUrl(
    `api/recommenders/ProductRecommenders/${id}/recommendations`
  );
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: headers(token),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};

export const deleteProductRecommender = async ({
  success,
  error,
  token,
  id,
}) => {
  const url = getUrl(`api/recommenders/ProductRecommenders/${id}`);
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

export const createProductRecommender = async ({
  success,
  error,
  token,
  payload,
  onFinally,
}) => {
  try {
    const url = getUrl("api/recommenders/ProductRecommenders");
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
  } finally {
    if (onFinally) {
      onFinally();
    }
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
    recommenderApiName: "ProductRecommenders",
    id,
    modelId,
    token,
  })
    .then(success)
    .then(error);
};

export const fetchLinkedRegisteredModel = async ({
  success,
  error,
  token,
  id,
}) => {
  fetchLinkedRegisteredModelAsync({
    recommenderApiName: "ProductRecommenders",
    id,
    token,
  })
    .then(success)
    .catch(error);
};

export const invokeProductRecommender = async ({
  success,
  error,
  onFinally,
  token,
  id,
  version,
  input,
}) => {
  try {
    const url = getUrl(`api/recommenders/ProductRecommenders/${id}/invoke`);
    const result = await fetch(`${url}?version=${version || "default"}`, {
      headers: headers(token),
      method: "post",
      body: JSON.stringify(input),
    });
    if (result.ok) {
      success(await result.json());
    } else {
      error(await result.json());
    }
  } finally {
    if (onFinally) {
      onFinally();
    }
  }
};

export const fetchInvokationLogsAsync = async ({ id, token, page }) => {
  return await il.fetchRecommenderInvokationLogsAsync({
    recommenderApiName: "ProductRecommenders",
    id,
    token,
    page,
  });
};

export const fetchTargetVariablesAsync = async ({ id, token, name }) => {
  return await tv.fetchRecommenderTargetVariableValuesAsync({
    recommenderApiName: "ProductRecommenders",
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
    recommenderApiName: "ProductRecommenders",
    id,
    token,
    targetVariableValue,
  });
};
