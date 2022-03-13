import { customers } from "cherry.ai";

export const fetchCustomersAsync = customers.fetchCustomersAsync;
export const fetchCustomerAsync = customers.fetchCustomerAsync;

export const fetchLatestRecommendationsAsync =
  customers.fetchLatestRecommendationsAsync;

export const uploadUserDataAsync = customers.uploadUserDataAsync;

export const createOrUpdateCustomerAsync =
  customers.createOrUpdateCustomerAsync;
export const updateMergePropertiesAsync = customers.updateMergePropertiesAsync;
export const deleteCustomerAsync = customers.deleteCustomerAsync;

const defaultHeaders = { "Content-Type": "application/json" };
const headers = (token) =>
  !token
    ? defaultHeaders
    : { ...defaultHeaders, Authorization: `Bearer ${token}` };

export const setCustomerMetricAsync = customers.setCustomerMetricAsync;

export const fetchCustomerSegmentsAsync = customers.fetchCustomerSegmentsAsync;
