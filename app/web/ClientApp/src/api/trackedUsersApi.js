import { trackedUsers } from "signalbox.js";

export const fetchTrackedUsersAsync = trackedUsers.fetchTrackedUsersAsync;

export const fetchTrackedUser = trackedUsers.fetchTrackedUser;
export const fetchTrackedUserAsync = trackedUsers.fetchTrackedUserAsync;

export const fetchUniqueTrackedUserActionGroupsAsync =
  trackedUsers.fetchUniqueTrackedUserActionGroupsAsync;

export const fetchLatestRecommendationsAsync =
  trackedUsers.fetchLatestRecommendationsAsync;

export const fetchTrackedUserActionAsync =
  trackedUsers.fetchTrackedUserActionAsync;

export const uploadUserDataAsync = trackedUsers.uploadUserDataAsync;

export const fetchTrackedUsersActionsAsync =
  trackedUsers.fetchTrackedUsersActionsAsync;

export const createOrUpdateTrackedUserAsync =
  trackedUsers.createOrUpdateTrackedUserAsync;
export const updateMergePropertiesAsync =
  trackedUsers.updateMergePropertiesAsync;

const defaultHeaders = { "Content-Type": "application/json" };
const headers = (token) =>
  !token
    ? defaultHeaders
    : { ...defaultHeaders, Authorization: `Bearer ${token}` };

export const setTrackedUserFeatureAsync = async ({
  id,
  featureId,
  value,
  token,
}) => {
  const url = `api/TrackedUsers/${id}/features/${featureId}`;
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
