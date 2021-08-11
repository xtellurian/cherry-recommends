import { pageQuery } from "./paging";
import { chunkArray } from "../utilities/chunk";
import { getUrl } from "../baseUrl";
import { headers } from "./headers";
const MAX_ARRAY = 5000;

const searchEntities = (term) => {
  if (term) {
    return `&q.term=${term}`;
  } else {
    return "";
  }
};
export const fetchTrackedUsersAsync = async ({ token, page, searchTerm }) => {
  const url = getUrl("api/trackedUsers");
  const response = await fetch(
    `${url}?${pageQuery(page)}${searchEntities(searchTerm)}`,
    {
      headers: headers(token),
    }
  );
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
export const fetchTrackedUsers = async ({
  success,
  error,
  token,
  page,
  searchTerm,
}) => {
  fetchTrackedUsersAsync({ token, page, searchTerm })
    .then(success)
    .catch(error);
};

export const updateMergePropertiesAsync = async ({ token, id, properties }) => {
  const url = getUrl(`api/trackedUsers/${id}/properties`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(properties),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchTrackedUser = async ({ success, error, token, id }) => {
  const url = getUrl(`api/trackedUsers/${id}`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    const trackedUser = await response.json();
    success(trackedUser);
  } else {
    error(await response.json());
  }
};

export const fetchUniqueTrackedUserActionGroupsAsync = async ({
  token,
  id,
}) => {
  const url = getUrl(`api/trackedUsers/${id}/action-groups`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchTrackedUserActionAsync = async ({
  token,
  id,
  category,
  actionName,
}) => {
  let url = getUrl(`api/trackedUsers/${id}/actions/${category}`);
  if (actionName) {
    url = url + `?actionName=${actionName}`;
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
export const fetchTrackedUserAction = async ({
  success,
  error,
  token,
  id,
  category,
  actionName,
}) => {
  fetchTrackedUserActionAsync({ id, token, category, actionName })
    .then(success)
    .catch(error);
};

export const uploadUserData = async ({ success, error, token, payload }) => {
  const url = getUrl(`api/trackedUsers`);
  const payloads = chunkArray(payload.users, MAX_ARRAY).map((users) => ({
    users,
  }));
  const responses = [];
  for (const p of payloads) {
    const response = await fetch(url, {
      headers: headers(token),
      method: "put",
      body: JSON.stringify(p),
    });
    if (response.ok) {
      responses.push(await response.json());
    } else {
      error(await response.json());
    }
  }
  success(responses);
};

export const createOrUpdateTrackedUser = async ({
  success,
  error,
  token,
  user,
}) => {
  const url = getUrl(`api/trackedUsers/`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(user),
  });
  if (response.ok) {
    success(await response.json());
  } else {
    error(await response.json());
  }
};
