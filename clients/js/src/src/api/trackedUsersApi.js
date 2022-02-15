import * as customers from "./customersApi";

// backwards compatible shim TrackedUser => Customer

export const fetchTrackedUsersAsync = customers.fetchCustomersAsync;

export const updateMergePropertiesAsync = customers.updateMergePropertiesAsync;

export const fetchTrackedUserAsync = customers.fetchCustomerAsync;

export const fetchUniqueTrackedUserActionGroupsAsync =
  customers.fetchUniqueCustomerActionGroupsAsync;

export const fetchLatestRecommendationsAsync =
  customers.fetchLatestRecommendationsAsync;

export const fetchTrackedUserActionAsync = customers.fetchCustomerActionAsync;

export const uploadUserDataAsync = customers.uploadUserDataAsync;

export const createOrUpdateTrackedUserAsync =
  customers.createOrUpdateCustomerAsync;

export const fetchTrackedUsersActionsAsync =
  customers.fetchCustomersActionsAsync;

export const deleteCustomerAsync = customers.deleteCustomerAsync;
