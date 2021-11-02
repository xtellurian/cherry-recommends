import { executeFetch } from "./client/apiClient";

export const fetchUniqueActionNamesAsync = async ({ token, page, term }) => {
  return await executeFetch({
    path: "api/actions/distinct-groups",
    token,
    page,
    query: {
      term,
    },
  });
};

export const fetchDistinctGroupsAsync = async ({ token, page, term }) => {
  return await executeFetch({
    path: "api/actions/distinct-groups",
    token,
    page,
    query: {
      term,
    },
  });
};
