import { customers } from "cherry.ai";

export const fetchCustomersAsync = customers.fetchCustomersAsync;
export const fetchCustomerAsync = customers.fetchCustomerAsync;

export const fetchLatestRecommendationsAsync =
  customers.fetchLatestRecommendationsAsync;

export const uploadUserDataAsync = customers.uploadUserDataAsync;

export const createOrUpdateCustomerAsync =
  customers.createOrUpdateCustomerAsync;
export const updateMergePropertiesAsync = customers.updateMergePropertiesAsync;

const defaultHeaders = { "Content-Type": "application/json" };
const headers = (token) =>
  !token
    ? defaultHeaders
    : { ...defaultHeaders, Authorization: `Bearer ${token}` };

export const setCustomerFeatureAsync = async ({
  id,
  featureId,
  value,
  token,
}) => {
  const url = `api/customers/${id}/features/${featureId}`;
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify({ value }),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
