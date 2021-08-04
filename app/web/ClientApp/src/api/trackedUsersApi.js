import { trackedUsers } from "signalbox.js";

export const fetchTrackedUsers = trackedUsers.fetchTrackedUsers;

export const fetchTrackedUser = trackedUsers.fetchTrackedUser;

export const fetchUniqueTrackedUserActions =
  trackedUsers.fetchUniqueTrackedUserActions;

export const fetchTrackedUserAction = trackedUsers.fetchTrackedUserAction;

export const uploadUserData = trackedUsers.uploadUserData;

export const createOrUpdateTrackedUser = trackedUsers.createOrUpdateTrackedUser;

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
