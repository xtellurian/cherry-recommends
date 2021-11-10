import { chunkArray } from "../utilities/chunk";
import { handleErrorResponse } from "../utilities/errorHandling";
import { executeFetch } from "./client/apiClient";

const MAX_ARRAY = 5000;

export const fetchTrackedUsersAsync = async ({ token, page, searchTerm }) => {
  return await executeFetch({
    path: "api/trackedUsers",
    token,
    page,
    query: {
      "q.term": searchTerm,
    },
  });
};

export const updateMergePropertiesAsync = async ({ token, id, properties }) => {
  return await executeFetch({
    token,
    path: `api/trackedUsers/${id}/properties`,
    method: "post",
    body: properties,
  });
};

export const fetchTrackedUserAsync = async ({ token, id, useInternalId }) => {
  return await executeFetch({
    path: `api/trackedUsers/${id}`,
    token,
    query: {
      useInternalId,
    },
  });
};

export const fetchUniqueTrackedUserActionGroupsAsync = async ({
  token,
  id,
}) => {
  return await executeFetch({
    token,
    path: `api/trackedUsers/${id}/action-groups`,
  });
};

export const fetchLatestRecommendationsAsync = async ({ token, id }) => {
  return await executeFetch({
    token,
    path: `api/trackedUsers/${id}/latest-recommendations`,
  });
};

export const fetchTrackedUserActionAsync = async ({
  token,
  id,
  category,
  actionName,
}) => {
  return await executeFetch({
    path: `api/trackedUsers/${id}/actions/${category}`,
    token,
    query: {
      actionName,
    },
  });
};

export const uploadUserDataAsync = async ({ token, payload }) => {
  const payloads = chunkArray(payload.users, MAX_ARRAY).map((users) => ({
    users,
  }));
  const responses = [];
  for (const p of payloads) {
    const response = await executeFetch({
      token,
      path: "api/trackedUsers",
      method: "put",
      body: p,
    });
    if (response.ok) {
      responses.push(await response.json());
    } else {
      return await handleErrorResponse(response);
    }
  }
  return responses;
};

export const createOrUpdateTrackedUserAsync = async ({ token, user }) => {
  return await executeFetch({
    path: "api/trackedUsers",
    method: "post",
    body: user,
    token
  });
};

export const fetchTrackedUsersActionsAsync = async ({
  token,
  page,
  id,
  revenueOnly,
}) => {
  return await executeFetch({
    path: `api/TrackedUsers/${id}/Actions`,
    token,
    page,
    query: {
      revenueOnly: !!revenueOnly,
    },
  });
};
